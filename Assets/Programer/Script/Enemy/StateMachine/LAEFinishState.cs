using UnityEngine;

//遠距離攻撃のとどめがさせる状態のステート
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
        //一定時間経過したらとどめが指せなくなる
        if (_isTimeStart)
        {
            _enemy.Audio(SEState.EnemyStan);
            _timer += Time.deltaTime;
            if (_timer > _enemy.FinishStopInterval)
            {
                _enemy.StateChange(EnemyBase.MoveState.FreeMove);
                Exit();
            }
        }
    }
}
