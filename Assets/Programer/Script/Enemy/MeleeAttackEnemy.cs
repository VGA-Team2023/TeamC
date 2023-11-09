using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeleeAttackEnemy : EnemyBase, IEnemyDamageble, IFinishingDamgeble
{
    [SerializeField, Tooltip("移動の範囲(黄色の円)"), Range(0, 10)]
    float _moveRange;
    public float MoveRange => _moveRange;
    [SerializeField, Tooltip("どれくらいベース位置に近づいたら次の目標に向かうか")]
    float _distance;
    Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    MoveState _state = MoveState.FreeMove;
    MoveState _nextState = MoveState.FreeMove;
    PlayerControl _player;
    MAEFreeMoveState _freeMoveState;
    MAEAttackState _attack;
    MAEFinishState _finish;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerControl>();
        _freeMoveState = new MAEFreeMoveState(this, _player, SearchRange, _distance, _moveRange, Speed);
        _attack = new MAEAttackState(this, _player, SearchRange, _distance, Speed);
        _finish = new MAEFinishState(this);
        base.OnEnemyDestroy += OnEnemyDestroy;
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
        }
        if(_state != _nextState)
        {
            switch (_nextState)
            {
                case MoveState.FreeMove:
                    _freeMoveState.Enter();
                    break;
                case MoveState.Attack:
                    _attack.Enter();
                    break;
            }
            _state = _nextState;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayerControl>();
        if (_state == MoveState.FreeMove)
        {
            _freeMoveState.WallHit();
        }
        if(player)
        {
            player.Damage(Attack);
        }
    }

    public void OnEnemyDestroy()
    {
        gameObject.layer = FinishLayer;
    }

    public void StateChange(MoveState changeState)
    {
        _nextState = changeState;
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        _rb.velocity = Vector3.zero;
        if(attackType == AttackType.ShortChantingMagick)
        {
            if(attackHitTyp == MagickType.Ice)
            {

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
        Core.SetActive(true);
        _state = MoveState.Finish;
    }

    public void StopFinishing()
    {
        Core.SetActive(false);
    }

    public void EndFinishing()
    {
        Vector3 dir = transform.position - _player.transform.position;
        _rb.AddForce((dir.normalized / 2 + Vector3.up) * 10, ForceMode.Impulse);
        Destroy(gameObject);
    }
}
