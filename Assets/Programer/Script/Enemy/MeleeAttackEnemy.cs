using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[RequireComponent(typeof(Rigidbody))]
public class MeleeAttackEnemy : EnemyBase, IEnemyDamageble, IFinishingDamgeble, IPause, ISlow, ISpecialMovingPause
{
    [Header("敵の挙動に関する数値")]
    [SerializeField, Tooltip("移動の範囲(黄色の円)"), Range(0, 30)]
    float _moveRange;
    public float MoveRange => _moveRange;

    [SerializeField, Tooltip("どれくらいベース位置に近づいたら次の目標に向かうか")]
    float _distance;
    public float Distance => _distance;

    [SerializeField, Tooltip("どれくらいプレイヤーに近づいたら追いかけるステートに入るか")]
    float _chaseDistance;
    public float ChaseDistance => _chaseDistance;

    [SerializeField, Tooltip("スローになった時のプレイヤーのスピード")]
    float _slowSpeed;
    [Header("====================")]

    [Header("モーション関係")]
    [SerializeField, Tooltip("近接敵のアニメーター")]
    Animator _anim;
    public Animator Animator => _anim;

    [Header("====================")]

    [Header("生成するオブジェクト")]
    [SerializeField, Tooltip("氷魔法の通常攻撃エフェクト")]
    GameObject _iceAttackEffect;

    [SerializeField, Tooltip("氷魔法のとどめ攻撃エフェクト")]
    GameObject _iceFinishEffect;

    [SerializeField, Tooltip("草魔法の通常攻撃エフェクト")]
    GameObject _grassAttackEffect;

    [SerializeField, Tooltip("草魔法のとどめ攻撃エフェクト")]
    GameObject _grassFinishEffect;

    Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    float _defaultSpeed = 0;
    float _defaultHp = 0;
    private float _blownPower = 5;
    MoveState _state = MoveState.FreeMove;


    /// <summary>勝手に追加コーナー</summary>↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ 

    private float _countDestroyTime = 0;

    /// <summary>死亡したかどうか</summary>
    private bool _isDeath = false;

    private bool _isFinishAttackNow = false;

    [Header("死亡後消えるまでの時間")]
    [SerializeField] private float _destroyTime = 2f;

    [Header("攻撃のエフェクト")]
    [SerializeField] private List<ParticleSystem> _attackEffect = new List<ParticleSystem>();

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

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }


    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        GameManager.Instance.SpecialMovingPauseManager.Resume(this);
    }

    public void AttackEffectPlay()
    {
        _attackEffect.ForEach(i => i.Play());
    }

    /////↑↑↑↑↑↑↑↑



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
    PlayerControl _player;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerControl>();
        _defaultHp = HP;
        HpBar.maxValue = _defaultHp;
        HpBar.value = _defaultHp;
        States[(int)MoveState.FreeMove] = new MAEFreeMoveState(this, _player);
        States[(int)MoveState.Attack] = new MAEAttackState(this, _player);
        States[(int)MoveState.Finish] = new MAEFinishState(this);
        States[(int)MoveState.Chase] = new MAEChaseState(this, _player);
        base.OnEnemyDestroy += StartFinishing;

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
            // CurrentState.WallHit();
        }
    }

    public bool TryGet<T>(out T returnObject, GameObject checkObject)
    {
        return checkObject.TryGetComponent(out returnObject);
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
    /// ダメージを受けた時の関数
    /// </summary>
    /// <param name="attackType">チャージ攻撃か単発攻撃か</param>
    /// <param name="attackHitTyp">氷属性か草属性か</param>
    /// <param name="damage">ダメージ量</param>
    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        VoiceAudio(VoiceState.EnemyShortDamage, CRIType.Play);
        _rb.velocity = Vector3.zero;
        SeAudio(SEState.EnemyNormalDamage, CRIType.Play);

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
            if (IsDemo) return;

            //ダメージを受けた時ノックバックする
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
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
            if (IsDemo) return;

            //ダメージを受けた時ノックバックする
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// トドメが可能な状態
    /// </summary>
    public void StartFinishing()
    {
        //勝手に追加コーナー
        _isFinishAttackNow = true;

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
        //勝手に追加コーナー
        _isFinishAttackNow = false;

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
        VoiceAudio(VoiceState.EnemyShortSaerch, CRIType.Stop);
        SeAudio(SEState.EnemyFinishDamage, CRIType.Play);

        //氷属性と草属性で必殺技のエフェクトを変える
        if (attackHitTyp == MagickType.Ice)
        {
            SeAudio(SEState.EnemyFinichHitIce, CRIType.Play);
            GameObject iceAttack = Instantiate(_iceFinishEffect);
            iceAttack.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        }
        else if (attackHitTyp == MagickType.Grass)
        {
            SeAudio(SEState.EnemyFinishHitGrass, CRIType.Play);
            GameObject grassAttack = Instantiate(_grassFinishEffect);
            grassAttack.transform.position = transform.position + new Vector3(0, -1.8f, 0);
        }
        if (IsDemo) return;

        _isDeath = true;
        gameObject.layer = DeadLayer;
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        SeAudio(SEState.EnemyOut, CRIType.Play);

        Core.SetActive(false);

        //死亡アニメーション
        _anim.Play("Death");

        //ノックバックをする(通常のダメージより大きいノックバック)
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
    }

    /// <summary>
    /// ポーズボタンを押したときにキャラを止める
    /// </summary>
    public void Pause()
    {
        _anim.speed = 0;
        _defaultSpeed = Speed;
        Speed = 0;
    }

    /// <summary>
    /// ポーズボタンから戻った時にキャラを再び動かす
    /// </summary>
    public void Resume()
    {
        _anim.speed = 1;
        Speed = _defaultSpeed;
    }

    /// <summary>
    /// トドメを指す時にキャラを止める
    /// </summary>
    void ISpecialMovingPause.Pause()
    {
        _anim.speed = 0;
        _defaultSpeed = Speed;
        Speed = 0;
    }

    /// <summary>
    /// トドメのアニメーションが終わったら動く
    /// </summary>
    void ISpecialMovingPause.Resume()
    {
        _anim.speed = 1;
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
