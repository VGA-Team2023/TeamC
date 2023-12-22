﻿using UnityEngine;
using UnityEngine.UI;

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
    int _defaultHp = 0;

    MoveState _state = MoveState.FreeMove;
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
                    _freeMoveState.Enter();
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
    PlayerControl _player;

    MAEFreeMoveState _freeMoveState;
    MAEAttackState _attack;
    MAEFinishState _finish;
    MAEChaseState _chase;



    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerControl>();
        _defaultHp = HP;
        _freeMoveState = new MAEFreeMoveState(this, _player);
        _attack = new MAEAttackState(this, _player);
        _finish = new MAEFinishState(this);
        _chase  = new MAEChaseState(this, _player);
        base.OnEnemyDestroy += StartFinishing;
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
    }

    void Update()
    {
        switch (_state)
        {
            case MoveState.FreeMove:
                _freeMoveState.Update();
                break;
            case MoveState.Attack:
                _attack.Update(); 
                break;
            case MoveState.Finish:
                _finish.Update();
                break;
            case MoveState.Chase:
                _chase.Update();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (_state == MoveState.FreeMove)
        //{
        //    _freeMoveState.WallHit();
        //}
    }

    public bool TryGet<T>(out T returnObject, GameObject checkObject)
    {
        return checkObject.TryGetComponent(out returnObject);
    }

    public void StateChange(MoveState changeState)
    {
        State = changeState;
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        VoiceAudio(VoiceState.EnemyShortDamage, CRIType.Play);
        _rb.velocity = Vector3.zero;
        SeAudio(SEState.EnemyNormalDamage, CRIType.Play);
        if (attackHitTyp == MagickType.Ice)
        {
            GameObject iceAttack = Instantiate(_iceAttackEffect, transform.position, Quaternion.identity);
            Destroy(iceAttack, 0.3f);
            if (attackType == AttackType.ShortChantingMagick)
            {
                SeAudio(SEState.EnemyHitIcePatternA, CRIType.Play);
                if (IsDemo) return;
                HP--;
            }
            else
            {
                SeAudio(SEState.EnemyHitIcePatternB, CRIType.Play);
                if (IsDemo) return;
                HP -= (int)damage;
            }
            if (IsDemo) return;
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
        }
        else if (attackHitTyp == MagickType.Grass)
        {
            GameObject grassAttack = Instantiate(_grassAttackEffect, transform.position, Quaternion.identity);
            Destroy(grassAttack, 0.3f);
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
            if (IsDemo) return;
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
        }
    }

    public void StartFinishing()
    {
        gameObject.layer = FinishLayer;
        _rb.velocity = Vector3.zero;
        Core.SetActive(true);
        if (IsDemo) return;
        StateChange(MoveState.Finish);
    }

    public void StopFinishing()
    {
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        Core.SetActive(false);
        gameObject.layer = DefaultLayer;
        HP = _defaultHp;
    }

    public void EndFinishing(MagickType attackHitTyp)
    {
        VoiceAudio(VoiceState.EnemyShortSaerch, CRIType.Stop);
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
        if (IsDemo) return;
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
        base.OnEnemyDestroy -= StartFinishing;
        EnemyFinish();
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        gameObject.layer = DeadLayer;
        SeAudio(SEState.EnemyStan, CRIType.Stop);
        SeAudio(SEState.EnemyOut, CRIType.Play);
        Destroy(gameObject, 1f);
    }

    public void Pause()
    {
        _anim.speed = 0;
        _defaultSpeed = Speed;
        Speed = 0;
    }

    public void Resume()
    {
        _anim.speed = 1;
        Speed = _defaultSpeed;
    }

    void ISpecialMovingPause.Pause()
    {
        _anim.speed = 0;
        _defaultSpeed = Speed;
        Speed = 0;
    }

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

    public void SeAudio(SEState playSe, CRIType criType)
    {
        if(IsAudio)
        {
            if (criType == CRIType.Play)
            {
                AudioController.Instance.SE.Play3D(playSe, transform.position);
            }
            else if(criType == CRIType.Stop)
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
