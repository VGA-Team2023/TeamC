using System.Collections.Generic;
using UnityEngine;

public class LAEFreeMoveState : IStateMachine
{
    LongAttackEnemy _enemy;
    PlayerControl _player;
    List<Vector3> _patrolPoint = new List<Vector3>();
    int _index;

    public LAEFreeMoveState(LongAttackEnemy enemy, PlayerControl player, List<Vector3> patrolPoint)
    { 
        _enemy = enemy;
        _player = player;
        _patrolPoint = patrolPoint;
    }
    public void Enter()
    {
        Debug.Log("LAEFreeMove:Enter");
    }

    public void Exit()
    {
        Debug.Log("LAEFreeMove:Exit");
    }

    public void Update()
    {
        float playerDistance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(playerDistance < _enemy.SearchRange)
        {
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.Attack);
        }
        float distance = Vector3.Distance(_enemy.transform.position, _patrolPoint[_index % _patrolPoint.Count]);
        if (distance < _enemy.ChangeDistance)
        {
            _index++;
        }
        _enemy.transform.forward = (_patrolPoint[_index % _patrolPoint.Count] - _enemy.transform.position).normalized;
        _enemy.Rb.velocity = _enemy.transform.forward * _enemy.Speed;
    }
}
