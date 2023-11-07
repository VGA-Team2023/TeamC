using UnityEngine;

public class MAETargetMoveState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    Vector3 _basePosition;
    float _playerSearchDistance;
    float _distance;
    float _speed;
    public MAETargetMoveState(MeleeAttackEnemy enemy, PlayerControl player, float playerSearchDistance, float distance, float speed)
    {
        _enemy = enemy;
        _player = player;
        _playerSearchDistance = playerSearchDistance;
        _distance = distance;
        _speed = speed;

        _basePosition = _enemy.transform.position;
    }
    public void Enter()
    {
        Debug.Log("TargetMove:Enter");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        float playerDis = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if(playerDis < _playerSearchDistance)
        {
            _enemy.transform.forward = (_player.transform.position - _enemy.transform.position).normalized;
            _enemy.Rb.velocity = _enemy.transform.forward * _speed;
        }
        else
        {
            float baseDis = Vector3.Distance(_enemy.transform.position, _basePosition);
            if(baseDis > _distance)
            {
                _enemy.transform.forward = (_basePosition - _enemy.transform.position).normalized;
                _enemy.Rb.velocity = _enemy.transform.forward * _speed;
            }
            else
            {
                _enemy.StateChange(EnemyBase.MoveState.FreeMove);
            }
        }

    }
}
