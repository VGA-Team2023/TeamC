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
        if (_enemy.IsDemo)
        {
            _enemy.StartFinishing();
        }
        //タイマーをリセットする
        _timer = 0;
        _enemy.SeAudio(SEState.EnemyStan, EnemyBase.CRIType.Play);
    }

    public void Exit()
    {
        _enemy.HpBar.gameObject.SetActive(true);
        _enemy.StopFinishing();
    }

    public void Update()
    {
        _enemy.SeAudio(SEState.EnemyStan, EnemyBase.CRIType.Update);
        if (_enemy.IsDemo) return;
        //一定時間が経過したらとどめ可能な状態から通常状態に戻る
        _timer += Time.deltaTime;
        if (_timer > _enemy.FinishStopInterval)
        {
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
            Exit();
        }
    }

    public void WallHit()
    {
        throw new System.NotImplementedException();
    }
}