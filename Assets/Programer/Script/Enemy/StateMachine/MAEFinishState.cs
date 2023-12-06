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
        //タイマーをスタートさせる
        _isTimeStart = true;
        _timer = 0;
    }

    public void Exit()
    {
        _isTimeStart = false;
        _enemy.StopFinishing();
    }

    public void Update()
    {
        if (_isTimeStart)
        {
            //一定時間が経過したらとどめ可能な状態から通常状態に戻る
            _timer += Time.deltaTime;
            if (_timer > _enemy.FinishStopInterval)
            {
                _enemy.StateChange(EnemyBase.MoveState.FreeMove);
                Exit();
            }
        }
    }
}