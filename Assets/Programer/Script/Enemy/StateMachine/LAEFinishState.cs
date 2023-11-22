using UnityEngine;

public class LAEFinishState : IStateMachine
{
    LongAttackEnemy _enemy;
    float _timer;
    bool _isTimeStart = false;

    public LAEFinishState(LongAttackEnemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _isTimeStart = true;
        _timer = 0;
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
            if (_timer > _enemy.FinishStopInterval)
            {
                _enemy.StateChange(EnemyBase.MoveState.FreeMove);
                Exit();
            }
        }
    }
}
