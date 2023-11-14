using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAEChaseState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    public MAEChaseState(MeleeAttackEnemy enemy, PlayerControl player)
    {
        _enemy = enemy;
        _player = player;
    }
    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        float distance = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if(distance < _enemy.ChaseDistance)
        {
            _enemy.StateChange(EnemyBase.MoveState.Attack);
        }
        if(distance > _enemy.SearchRange)
        {
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        }
        var dir = (_player.transform.position - _enemy.transform.position).normalized;
        _enemy.transform.forward = new Vector3(dir.x, 0, dir.z);
        _enemy.Rb.velocity = (_enemy.transform.forward * _enemy.Speed)  + new Vector3(0, dir.y, 0);

    }
}
