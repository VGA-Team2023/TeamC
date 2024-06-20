using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : EnemyBase, IEnemyDamageble, IFinishingDamgeble, IPause, ISlow, ISpecialMovingPause
{
    [Header("テストの弾を飛ばす場所")]
    [SerializeField]
    Transform _testPosition;

    [Header("敵の挙動に関する項目")]
    [SerializeField, Tooltip("移動先の場所")]
    List<Transform> _movePosition = new List<Transform>();

    [SerializeField, Tooltip("アニメーション")]
    Animator _anim;
    public Animator Animator => _anim;

    [Min(0.6f)]
    [SerializeField, Tooltip("どれくらい移動先に近づいたら次の地点に行くか")]
    float _changePointDistance = 0.6f;
    public float ChangeDistance => _changePointDistance;

    [SerializeField, Tooltip("スローになった時のプレイヤーのスピード")]
    float _slowSpeed;
    [Header("====================")]

    [Header("弾に関する項目")]
    [SerializeField, Tooltip("発射する弾のPrefab")]
    GameObject _bulletPrefab;

    [SerializeField, Tooltip("弾の発射位置")]
    Transform _muzzle;
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

    PlayerControl _player;
    MoveState _state = MoveState.FreeMove;


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
            switch (_state)
            {
                case MoveState.FreeMove:
                    _freeMove.Enter();
                    break;
                case MoveState.Attack:
                    _attack.Enter();
                    break;
                case MoveState.Finish:
                    _finish.Enter();
                    break;
            }
        }
    }

    MagickType _magicType;

    LAEFreeMoveState _freeMove;
    LAEAttackState _attack;
    LAEFinishState _finish;

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
        _freeMove = new LAEFreeMoveState(this, _player, patrolPoint);
        _attack = new LAEAttackState(this, _player);
        _finish = new LAEFinishState(this);
        base.OnEnemyDestroy += StartFinishing;
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }

    void Update()
    {

        ////勝手に追加コーナー↓↓↓↓
        if (GameManager.Instance.PauseManager.IsPause || GameManager.Instance.SpecialMovingPauseManager.IsPaused) return;
        CountDestroyTime();
        if (_isDeath) return;
        /////↑↑↑↑↑↑↑↑

        switch (_state)
        {
            case MoveState.FreeMove:
                _freeMove.Update();
                break;
            case MoveState.Attack:

                //勝手に追加コーナー
                if (_isFinishAttackNow) return;

                _attack.Update();
                break;
            case MoveState.Finish:
                _finish.Update();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state == MoveState.FreeMove)
        {
            _freeMove.WallHit();
        }
    }

    public void StateChange(MoveState changeState)
    {
        State = changeState;
    }

    public void Attack()
    {
        //var dir = new Vector3(_muzzle.transform.position.x, 0, _muzzle.transform.position.z);
        var bullet = Instantiate(_bulletPrefab, _muzzle.transform.position, Quaternion.identity);
        if (IsDemo)
        {
            bullet.GetComponent<EnemyBullet>().Init(_testPosition.position, base.Attack);
        }
        else
        {
            bullet.GetComponent<EnemyBullet>().Init(_player.transform.position, base.Attack);
        }
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    { 
        VoiceAudio(VoiceState.EnemyLongDamage, EnemyBase.CRIType.Play);
        _anim.Play("Hit");
        _rb.velocity = Vector3.zero;
        if (attackHitTyp == MagickType.Ice)
        {
            GameObject iceAttack = Instantiate(_iceAttackEffect);
            iceAttack.transform.position = transform.position;
            if (attackType == AttackType.ShortChantingMagick)
            {
                SeAudio(SEState.EnemyHitIcePatternA, CRIType.Play);
                if (IsDemo) return;
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
                if (WeekType == attackHitTyp)
                {
                    HP -= damage * WeekDamage;
                }
                else
                {
                    HP -= damage;
                }
            }
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
        }
        else if (attackHitTyp == MagickType.Grass)
        {
            GameObject grassAttack = Instantiate(_grassAttackEffect);
            grassAttack.transform.position = transform.position;
            if (attackType == AttackType.ShortChantingMagick)
            {
                SeAudio(SEState.EnemyHitGrassPatternA, CRIType.Play);
                if (IsDemo) return;
                HP--;
            }
            else
            {
                SeAudio(SEState.EnemyHitGrassPatternB, CRIType.Play);
                if (IsDemo) return;
                HP -= (int)damage;
            }
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
        }
    }

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

    public void StopFinishing()
    {
        _isFinishAttackNow = false;

        _anim.SetBool("isStan", false);
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        Core.SetActive(false);
        gameObject.layer = DefaultLayer;
        HP = _defaultHp;
    }

    public void EndFinishing(MagickType attackHitTyp)
    {
        VoiceAudio(VoiceState.EnemyLongSaerch, CRIType.Stop);
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        SeAudio(SEState.EnemyFinishDamage, CRIType.Play);
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


        //////勝手に追加コーナー↓↓↓↓
        _isDeath = true;
        gameObject.layer = DeadLayer;
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        SeAudio(SEState.EnemyOut, CRIType.Play);
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
        //↑↑↑

        //Vector3 dir = transform.position - _player.transform.position;
        //_rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
        //base.OnEnemyDestroy -= StartFinishing;
        //EnemyFinish();
        //GameManager.Instance.PauseManager.Remove(this);
        //GameManager.Instance.SlowManager.Remove(this);
        //gameObject.layer = DeadLayer;
        //SeAudio(SEState.EnemyOut, CRIType.Play);
        //Destroy(gameObject, 2f);
    }

    public void Pause()
    {
        //_anim.speed = 0;
        _defaultSpeed = Speed;
        Speed = 0;
    }

    public void Resume()
    {
        //_anim.speed = 1;
        Speed = _defaultSpeed;
    }

    void ISpecialMovingPause.Pause()
    {
        //_anim.speed = 0;
        _defaultSpeed = Speed;
        Speed = 0;
    }

    void ISpecialMovingPause.Resume()
    {
        //_anim.speed = 1;
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
