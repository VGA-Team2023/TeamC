using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : EnemyBase, IEnemyDamageble, IFinishingDamgeble, IPause, ISlow, ISpecialMovingPause
{
    [Header("テストの弾を飛ばす場所")]
    [SerializeField]
    private Transform _testPosition;

    [Header("敵の挙動に関する項目")]
    [SerializeField, Tooltip("移動先の場所")]
    private List<Transform> _movePosition = new List<Transform>();

    [SerializeField, Tooltip("アニメーション")]
    private Animator _anim;
    public Animator Animator => _anim;

    [Min(0.6f)]
    [SerializeField, Tooltip("どれくらい移動先に近づいたら次の地点に行くか")]
    private float _changePointDistance = 0.6f;
    public float ChangeDistance => _changePointDistance;

    [SerializeField, Tooltip("スローになった時のプレイヤーのスピード")]
    private float _slowSpeed;
    [Header("====================")]

    [Header("弾に関する項目")]
    [SerializeField, Tooltip("発射する弾のPrefab")]
    private GameObject _bulletPrefab;

    [SerializeField, Tooltip("弾の発射位置")]
    private Transform _muzzle;
    [Header("====================")]

    [Header("生成するオブジェクト")]
    [SerializeField, Tooltip("氷魔法の通常攻撃エフェクト")]
    private GameObject _iceAttackEffect;

    [SerializeField, Tooltip("氷魔法のとどめ攻撃エフェクト")]
    private GameObject _iceFinishEffect;

    [SerializeField, Tooltip("草魔法の通常攻撃エフェクト")]
    private GameObject _grassAttackEffect;

    [SerializeField, Tooltip("草魔法のとどめ攻撃エフェクト")]
    private GameObject _grassFinishEffect;

    private Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    private float _defaultSpeed = 0;
    private float _defaultHp = 0;
    private float _blownPower = 5;

    private PlayerControl _player;
    private MoveState _state = MoveState.FreeMove;


    /// <summary>勝手に追加コーナー</summary>↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ 

    private float _countDestroyTime = 0;

    /// <summary>死亡したかどうか</summary>
    private bool _isDeath = false;

    private bool _isFinishAttackNow = false;

    [Header("死亡後消えるまでの時間")]
    [SerializeField] private float _destroyTime = 4f;

    private void CountDestroyTime()
    {
        if (!_isDeath) return;

        _countDestroyTime += Time.deltaTime;

        if (_countDestroyTime > _destroyTime)
        {
            base.OnEnemyDestroy -= StartFinishing;
            EnemyFinish();
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        GameManager.Instance.SpecialMovingPauseManager.Resume(this);
    }


    public MoveState State
    {
        get => _state;
        set
        {
            if (IsDemo && _state == MoveState.Finish && value != MoveState.Finish) StopFinishing();
            if (_state == value) return;
            _state = value;
            CurrentState = States[(int)_state];
            CurrentState.Enter();
        }
    }

    public List<Vector3> SetMovePoint()
    {
        List<Vector3> list = new List<Vector3> { transform.position };
        foreach (var point in _movePosition)
        {
            list.Add(point.position);
        }
        return list;
    }

    void Start()
    {
        _player = FindObjectOfType<PlayerControl>();
        _rb = GetComponent<Rigidbody>();
        _defaultHp = HP;
        HpBar.maxValue = _defaultHp;
        HpBar.value = _defaultHp;
        List<Vector3> patrolPoint = new List<Vector3> { transform.position };
        foreach (var point in _movePosition)
        {
            patrolPoint.Add(point.position);
        }
        States[(int)MoveState.FreeMove] = new LAEFreeMoveState(this, _player, patrolPoint);
        States[(int)MoveState.Attack] = new LAEAttackState(this, _player);
        States[(int)MoveState.Finish] = new LAEFinishState(this);
        base.OnEnemyDestroy += StartFinishing;
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
        CurrentState = States[(int)_state];
    }

    void Update()
    {

        ////勝手に追加コーナー↓↓↓↓
        if (GameManager.Instance.PauseManager.IsPause || GameManager.Instance.SpecialMovingPauseManager.IsPaused) return;
        CountDestroyTime();
        if (_isDeath) return;
        /////↑↑↑↑↑↑↑↑


        if (_isFinishAttackNow) return;
        CurrentState.Update();
    }

    /// <summary>
    /// 何かに当たった時に次の場所に移動する
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (_state == MoveState.FreeMove)
        {
            CurrentState.WallHit();
        }
    }

    /// <summary>
    /// ステータスを変える関数
    /// </summary>
    /// <param name="changeState">移行するステート</param>
    public void StateChange(MoveState changeState)
    {
        State = changeState;
    }


    /// <summary>
    /// 遠距離敵の攻撃(弾をインスタンスするだけ)
    /// </summary>
    public void Attack()
    {
        var bullet = Instantiate(_bulletPrefab, _muzzle.transform.position, Quaternion.identity);

        //テストシーンでは特定の位置に弾を飛ばすようにする
        if (IsDemo)
        {
            bullet.GetComponent<EnemyBullet>().Init(_testPosition.position, base.Attack);
        }
        else
        {
            bullet.GetComponent<EnemyBullet>().Init(_player.transform.position, base.Attack);
        }
    }


    /// <summary>
    /// ダメージを受けた時の関数
    /// </summary>
    /// <param name="attackType">チャージ攻撃か単発攻撃か</param>
    /// <param name="attackHitTyp">氷属性か草属性か</param>
    /// <param name="damage">ダメージ量</param>
    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    { 
        VoiceAudio(VoiceState.EnemyLongDamage, EnemyBase.CRIType.Play);
        _anim.Play("Hit");
        _rb.velocity = Vector3.zero;

        //ヒットした属性に応じてヒットエフェクトを変える
        if (attackHitTyp == MagickType.Ice)
        {
            GameObject iceAttack = Instantiate(_iceAttackEffect);
            iceAttack.transform.position = transform.position;

            //単発の魔法とフルチャージの魔法でダメージ量が変わる
            if (attackType == AttackType.ShortChantingMagick)
            {
                SeAudio(SEState.EnemyHitIcePatternA, CRIType.Play);
                if (IsDemo) return;

                //弱点属性の場合ダメージ量が変化する
                if (WeekType == attackHitTyp)
                {
                    HP -= WeekDamage;
                }
                else
                {
                    HP--;
                }
            }
            else
            {
                SeAudio(SEState.EnemyHitIcePatternB, CRIType.Play);
                if (IsDemo) return;

                //弱点属性の場合ダメージ量が変化する
                if (WeekType == attackHitTyp)
                {
                    HP -= damage * WeekDamage;
                }
                else
                {
                    HP -= damage;
                }
            }

            //ダメージを受けた時ノックバックする
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + Vector3.up) * _blownPower, ForceMode.Impulse);
        }
        else if (attackHitTyp == MagickType.Grass)
        {
            GameObject grassAttack = Instantiate(_grassAttackEffect);
            grassAttack.transform.position = transform.position;

            //単発の魔法とフルチャージの魔法でダメージ量が変わる
            if (attackType == AttackType.ShortChantingMagick)
            {
                SeAudio(SEState.EnemyHitGrassPatternA, CRIType.Play);
                if (IsDemo) return;

                //弱点属性の場合ダメージ量が変化する
                if (WeekType == attackHitTyp)
                {
                    HP -= WeekDamage;
                }
                else
                {
                    HP--;
                }
            }
            else
            {
                SeAudio(SEState.EnemyHitGrassPatternB, CRIType.Play);
                if (IsDemo) return;

                //弱点属性の場合ダメージ量が変化する
                if (WeekType == attackHitTyp)
                {
                    HP -= damage * WeekDamage;
                }
                else
                {
                    HP -= damage;
                }
            }

            //ダメージを受けた時ノックバックする
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + Vector3.up) * _blownPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// トドメが可能な状態
    /// </summary>
    public void StartFinishing()
    {
        _isFinishAttackNow = true;

        _anim.SetBool("isStan", true);
        gameObject.layer = FinishLayer;
        _rb.velocity = Vector3.zero;
        Core.SetActive(true);
        if (IsDemo) return;
        StateChange(MoveState.Finish);
    }

    /// <summary>
    /// 時間が経過してトドメがさせなくなる
    /// </summary>
    public void StopFinishing()
    {
        _isFinishAttackNow = false;

        _anim.SetBool("isStan", false);
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        Core.SetActive(false);
        gameObject.layer = DefaultLayer;
        HP = _defaultHp;
    }

    /// <summary>
    /// 敵を倒したときに呼ばれる関数
    /// </summary>
    /// <param name="attackHitTyp">トドメを指したときの魔法属性</param>
    public void EndFinishing(MagickType attackHitTyp)
    {
        VoiceAudio(VoiceState.EnemyLongSaerch, CRIType.Stop);
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        SeAudio(SEState.EnemyFinishDamage, CRIType.Play);

        //氷属性と草属性で必殺技のエフェクトを変える
        if (attackHitTyp == MagickType.Ice)
        {
            SeAudio(SEState.EnemyFinichHitIce, CRIType.Play);
            GameObject iceAttack = Instantiate(_iceFinishEffect, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            Destroy(iceAttack, 3f);
        }
        else if (attackHitTyp == MagickType.Grass)
        {
            SeAudio(SEState.EnemyFinishHitGrass, CRIType.Play);
            GameObject grassAttack = Instantiate(_grassFinishEffect, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            Destroy(grassAttack, 3f);
        }


        _isDeath = true;
        gameObject.layer = DeadLayer;
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        SeAudio(SEState.EnemyOut, CRIType.Play);

        //ノックバックをする(通常のダメージより大きいノックバック)
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * _blownPower * 2, ForceMode.Impulse);
    }

    /// <summary>
    /// ポーズボタンを押したときにキャラを止める
    /// </summary>
    public void Pause()
    {
        _defaultSpeed = Speed;
        Speed = 0;
    }

    /// <summary>
    /// ポーズボタンから戻った時にキャラを再び動かす
    /// </summary>
    public void Resume()
    {
        Speed = _defaultSpeed;
    }

    /// <summary>
    /// トドメを指す時にキャラを止める
    /// </summary>
    void ISpecialMovingPause.Pause()
    {
        _defaultSpeed = Speed;
        Speed = 0;
    }

    /// <summary>
    /// トドメのアニメーションが終わったら動く
    /// </summary>
    void ISpecialMovingPause.Resume()
    {
        Speed = _defaultSpeed;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _defaultSpeed = Speed;
        Speed += _slowSpeed * Speed;
    }

    public void OffSlow()
    {
        Speed = _defaultSpeed;
    }

    /// <summary>
    /// SEを実行する
    /// </summary>
    /// <param name="playSe">どのSEを呼ぶか</param>
    /// <param name="criType">CRIのタイプ</param>
    public void SeAudio(SEState playSe, CRIType criType)
    {
        if (IsAudio)
        {
            if (criType == CRIType.Play)
            {
                AudioController.Instance.SE.Play3D(playSe, transform.position);
            }
            else if (criType == CRIType.Stop)
            {
                AudioController.Instance.SE.Stop(playSe);
            }
            else if (criType == CRIType.Update)
            {
                AudioController.Instance.SE.Update3DPos(playSe, transform.position);
            }
        }
    }

    /// <summary>
    /// キャラボイスを実行する
    /// </summary>
    /// <param name="playSe">どのキャラボイスか</param>
    /// <param name="criType">CRIのタイプ</param>
    public void VoiceAudio(VoiceState playSe, CRIType criType)
    {
        if (IsAudio)
        {
            if (criType == CRIType.Play)
            {
                AudioController.Instance.Voice.Play3D(playSe, transform.position);
            }
            else if (criType == CRIType.Stop)
            {
                AudioController.Instance.Voice.Stop(playSe);
            }
            else if (criType == CRIType.Update)
            {
                AudioController.Instance.Voice.Update3DPos(playSe, transform.position);
            }
        }
    }
}
