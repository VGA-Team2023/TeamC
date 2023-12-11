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
        _enemy.StartFinishing();
        //タイマーをリセットする
        _timer = 0;
    }

    public void Exit()
    {
        _enemy.StopFinishing();
    }

    public void Update()
    {
        _enemy.Audio(SEState.EnemyStan);
        if (_enemy.IsDemo) return;
        //一定時間が経過したらとどめ可能な状態から通常状態に戻る
        _timer += Time.deltaTime;
        if (_timer > _enemy.FinishStopInterval)
        {
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
            Exit();
        }
    }
}