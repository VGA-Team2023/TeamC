using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFinishState : BossStateBase
{
    public override void Enter()
    {
        _stateMachine.BossController.BossAnimControl.IsBlend(false);
        _stateMachine.BossController.BossAttack.StopAttack();
        _stateMachine.BossController.Move.StopMove();
    }



    public override void Exit()
    {
        _stateMachine.BossController.BossAnimControl.IsBlend(true);

    }


    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        //ダウン状態の時間計測
        _stateMachine.BossController.BossHp.CountKnockDownTime();

        if (_stateMachine.BossController.IsDeath)
        {
            _stateMachine.TransitionTo(_stateMachine.StateDeath);
            return;
        }//死亡

        if (!_stateMachine.BossController.BossHp.IsKnockDown)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }   //トドメ
    }

    public override void LateUpdate()
    {


    }

}
