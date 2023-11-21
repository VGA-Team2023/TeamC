using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeleeAttackEnemy : EnemyBase, IEnemyDamageble, IFinishingDamgeble
{

    [SerializeField, Range(0, 10)]
    float random;
    public float Random => random;
    [SerializeField, Tooltip("移動の範囲(黄色の円)"), Range(0, 10)]
    float _moveRange;
    public float MoveRange => _moveRange;

    [SerializeField, Tooltip("どれくらいベース位置に近づいたら次の目標に向かうか")]
    float _distance;
    public float Distance => _distance;

    [SerializeField]
    float _chaseDistance;
    public float ChaseDistance => _chaseDistance;

    [SerializeField]
    float _radius;

    [SerializeField]
    float _thetaSpeed;

    [SerializeField, Tooltip("氷魔法の通常攻撃エフェクト")]
    GameObject _iceAttackEffect;

    [SerializeField, Tooltip("氷魔法のとどめ攻撃エフェクト")]
    GameObject _iceFinishEffect;

    Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    MoveState _state = MoveState.FreeMove;
    MoveState _nextState = MoveState.FreeMove;
    MagickType _magicType;

    PlayerControl _player;
    MAEFreeMoveState _freeMoveState;
    MAEAttackState _attack;
    MAEFinishState _finish;
    MAEChaseState _chase;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerControl>();
        _freeMoveState = new MAEFreeMoveState(this, _player, SearchRange, _distance, _moveRange, Speed);
        _attack = new MAEAttackState(this, _player);
        _finish = new MAEFinishState(this);
        _chase  = new MAEChaseState(this, _player);
        base.OnEnemyDestroy += StartFinishing;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Damage(AttackType.ShortChantingMagick, MagickType.Ice, 3f);
        }
        if(gameObject.layer == 10 && Input.GetMouseButtonDown(1))
        {
            EndFinishing();
        }
        //float x = transform.position.x + _radius * Mathf.Cos(Time.time * _thetaSpeed);
        //float y = transform.position.y + _radius * Mathf.Sin(Time.time * _thetaSpeed) * Mathf.Cos(Time.time * _thetaSpeed);
        //float z = 0;
        //transform.position = new Vector3(x, y, z);
        if (_state != _nextState)
        {
            switch (_nextState)
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
            _state = _nextState;
        }
        switch (_state)
        {
            case MoveState.FreeMove:
                _freeMoveState.Update();
                break;
            case MoveState.Attack:
                _attack.Update(); 
                break;
            case MoveState.Finish:
                Debug.Log("FinishState");
                _finish.Update();
                break;
            case MoveState.Chase:
                _chase.Update();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state == MoveState.FreeMove)
        {
            _freeMoveState.WallHit();
        }
    }

    public void StateChange(MoveState changeState)
    {
        _nextState = changeState;
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        _rb.velocity = Vector3.zero;
        _magicType  = attackHitTyp;
        if (attackType == AttackType.ShortChantingMagick)
        {
            if(attackHitTyp == MagickType.Ice)
            {
                GameObject iceAttack = Instantiate(_iceAttackEffect, transform.position, Quaternion.identity);
                Destroy(iceAttack, 0.3f);
            }
            else if(attackHitTyp == MagickType.Grass)
            {

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
        gameObject.layer = 10;
        _rb.velocity = Vector3.zero;
        Core.SetActive(true);
        StateChange(MoveState.Finish);
    }

    public void StopFinishing()
    {
        Core.SetActive(false);
        gameObject.layer = 3;
    }

    public void EndFinishing()
    {
        if(_magicType == MagickType.Ice)
        {
            GameObject iceAttack = Instantiate(_iceFinishEffect, transform.position, Quaternion.identity);
            Destroy(iceAttack, 3f);
        }
        else if(_magicType == MagickType.Grass)
        {

        }
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
        base.OnEnemyDestroy -= StartFinishing;
        Destroy(gameObject, 1f);
    }
}
