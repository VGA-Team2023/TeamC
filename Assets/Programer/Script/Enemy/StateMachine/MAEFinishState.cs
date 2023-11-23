using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAEFinishState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    float _timer;
    bool _isTimeStart  =false;

    public MAEFinishState(MeleeAttackEnemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _isTimeStart = true;
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        _isTimeStart = false;
        _timer = 0;
        _enemy.StopFinishing();
    }

    public void Update()
    {
        if (_isTimeStart)
        {
            _timer += Time.deltaTime;
            if (_timer > 50f)
            {
                Exit();
                //_enemy.StateChange(EnemyBase.MoveState.FreeMove);
            }
        }
    }
}