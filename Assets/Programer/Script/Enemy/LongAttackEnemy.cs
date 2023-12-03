using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : EnemyBase, IEnemyDamageble, IFinishingDamgeble, IPause, ISlow
{
    [Header("敵の挙動に関する項目")]
    [SerializeField, Tooltip("移動先の場所")]
    List<Transform> _movePosition = new List<Transform>();

    [SerializeField, Tooltip("アニメーション")]
    Animator _anim;
    public Animator Animator => _anim;

    [SerializeField, Tooltip("どれくらい移動先に近づいたら次の地点に行くか")]
    float _changePointDistance = 0.5f;
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
    int _defaultHp = 0;

    PlayerControl _player;
    MoveState _state = MoveState.FreeMove;
    public MoveState State
    {
        get => _state;
        set
        {
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
        foreach(var point in _movePosition)
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
        List<Vector3> patrolPoint = new List<Vector3> { transform.position};
        foreach(var point in _movePosition)
        {
            patrolPoint.Add(point.position);
        }
        _freeMove = new LAEFreeMoveState(this, _player, patrolPoint);
        _attack = new LAEAttackState(this, _player);
        _finish = new LAEFinishState(this);
        base.OnEnemyDestroy += StartFinishing;
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
    }

    void Update()
    {
        switch(_state)
        {
            case MoveState.FreeMove:
                _freeMove.Update();
                break;
            case MoveState.Attack:
                _attack.Update();
                break;
            case MoveState.Finish:
                _finish.Update();
                break;
        }
    }

    public void StateChange(MoveState changeState)
    {
        State = changeState;
    }

    public void Attack()
    {
        var dir = new Vector3(_muzzle.transform.position.x, 0, _muzzle.transform.position.z);
        var bullet = Instantiate(_bulletPrefab, dir, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().Init((_player.transform.position - transform.position).normalized, base.Attack);
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        _rb.velocity = Vector3.zero;
        _magicType = attackHitTyp;
        if (attackType == AttackType.ShortChantingMagick)
        {
            if (attackHitTyp == MagickType.Ice)
            {
                GameObject iceAttack = Instantiate(_iceAttackEffect, transform.position, Quaternion.identity);
                Destroy(iceAttack, 0.3f);
            }
            else if (attackHitTyp == MagickType.Grass)
            {
                GameObject grassAttack = Instantiate(_grassAttackEffect, transform.position, Quaternion.identity);
                Destroy(grassAttack, 0.3f);
            }
            Vector3 dir = transform.position - _player.transform.position;
            _rb.AddForce(((dir.normalized / 2) + (Vector3.up * 0.5f)) * 5, ForceMode.Impulse);
            HP--;
        }
        else
        {
            HP -= (int)damage;
        }
    }

    public void StartFinishing()
    {
        gameObject.layer = FinishLayer;
        _rb.velocity = Vector3.zero;
        Core.SetActive(true);
        StateChange(MoveState.Finish);
    }

    public void StopFinishing()
    {
        Core.SetActive(false);
        gameObject.layer = DefaultLayer;
        HP = _defaultHp;
    }

    //public void EndFinishing()
    //{
    //    if (_magicType == MagickType.Ice)
    //    {
    //        GameObject iceAttack = Instantiate(_iceFinishEffect, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
    //        Destroy(iceAttack, 3f);
    //    }
    //    else if (_magicType == MagickType.Grass)
    //    {
    //        GameObject grassAttack = Instantiate(_grassFinishEffect, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
    //        Destroy(grassAttack, 3f);
    //    }
    //    Vector3 dir = transform.position - _player.transform.position;
    //    _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
    //    base.OnEnemyDestroy -= StartFinishing;
    //    EnemyFinish();
    //    GameManager.Instance.PauseManager.Remove(this);
    //    GameManager.Instance.SlowManager.Remove(this);
    //    Destroy(gameObject, 1f);
    //}

    public void Pause()
    {
        _defaultSpeed = Speed;
        Speed = 0;
    }

    public void Resume()
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

    public void EndFinishing(MagickType attackHitTyp)
    {
        if (attackHitTyp == MagickType.Ice)
        {
            GameObject iceAttack = Instantiate(_iceFinishEffect, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            Destroy(iceAttack, 3f);
        }
        else if (attackHitTyp == MagickType.Grass)
        {
            GameObject grassAttack = Instantiate(_grassFinishEffect, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            Destroy(grassAttack, 3f);
        }
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
        base.OnEnemyDestroy -= StartFinishing;
        EnemyFinish();
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        Destroy(gameObject, 1f);
    }
}
