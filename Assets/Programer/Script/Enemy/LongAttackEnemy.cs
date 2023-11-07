using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : EnemyBase
{
    [SerializeField, Tooltip("移動先の場所")]
    List<Transform> _movePosition = new List<Transform>();
    [SerializeField, Tooltip("どれくらい移動先に近づいたら次の地点に行くか")]
    float _changePointDistance = 0.5f;
    [SerializeField, Tooltip("発射する弾のPrefab")]
    GameObject _bulletPrefab;
    [SerializeField, Tooltip("弾の発射位置")]
    Transform _muzzle;

    Rigidbody _rb;
    public Rigidbody Rb { get => _rb; set => _rb = value; }

    PlayerControl _player;
    MoveState _state = MoveState.FreeMove;
    LAEFreeMoveState _freeMove;
    LAEAttackState _attack;

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
        List<Vector3> patrolPoint = new List<Vector3> { transform.position};
        foreach(var point in _movePosition)
        {
            patrolPoint.Add(point.position);
        }
        _freeMove = new LAEFreeMoveState(this, _player, patrolPoint, _changePointDistance, SearchRange, Speed);
        _attack = new LAEAttackState(this, _player, SearchRange, AttackInterval);
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
        }
    }

    public void StateChange(MoveState changeState)
    {
        _state = changeState;
    }

    public void Attack()
    {
        Instantiate(_bulletPrefab, _muzzle.transform.position, Quaternion.identity);
    }
}
