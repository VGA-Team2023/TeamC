using UnityEngine;

public class MAEFreeMoveState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    Vector3 _basePosition;
    Vector3 _dir;
    bool _isArrived = true;
    float _playerSearchDistance;
    float _distance;
    float _moveRange;
    float _speed;

    public MAEFreeMoveState(MeleeAttackEnemy enemy, PlayerControl player, float playerSearchDistance, float distance, float moveRange, float speed)
    {
        _enemy = enemy;
        _player = player;
        _playerSearchDistance = playerSearchDistance;
        _distance = distance;
        _moveRange = moveRange;
        _speed = speed;

        _basePosition = _enemy.transform.position;
        _dir = GetMovePoint();
        _enemy.transform.forward = (_dir - _enemy.transform.position).normalized;
    }
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
        float playerDis = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if (playerDis < _playerSearchDistance)
        {
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.Chase);
        }
        float baseDis = Vector3.Distance(_enemy.transform.position, _basePosition);
        float destinationDis = Vector3.Distance(_enemy.transform.position, _dir);
        if (baseDis < _distance && !_isArrived)
        {
            _dir = GetMovePoint();
            _enemy.transform.forward = (_dir - _enemy.transform.position).normalized;
            _isArrived = true;
        }
        else if (destinationDis < _distance && _isArrived)
        {
            _dir = _basePosition - _enemy.transform.position;
            _enemy.transform.forward = _dir.normalized;
            _isArrived = false;
        }
        _enemy.Rb.velocity = _enemy.transform.forward * _speed;
        _enemy.transform.position = new Vector3(_enemy.transform.position.x, _basePosition.y, _enemy.transform.position.z);
    }

    Vector3 GetMovePoint()
    {
        float random = Random.Range(0, 361);
        var dir = new Vector3(Mathf.Sin(random) * _moveRange + _enemy.transform.position.x, _basePosition.y, Mathf.Cos(random) * _moveRange + _enemy.transform.position.z);
        return dir;
    }

    public void WallHit()
    {
        _dir = _basePosition - _enemy.transform.position;
        _enemy.transform.forward = _dir.normalized;
        _isArrived = false;
    }
}
