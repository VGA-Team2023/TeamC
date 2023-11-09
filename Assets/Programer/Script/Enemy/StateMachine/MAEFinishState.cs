using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAEFinishState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    float _timer;
    public MAEFinishState(MeleeAttackEnemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        _enemy.StopFinishing();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > 5f)
        {
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        }
    }
}
