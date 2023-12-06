using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossDeathState : BossStateBase
{
    public override void Enter()
    {
        _stateMachine.BossController.BossDeath.FirstSetting();

        _stateMachine.BossController.BossAnimControl.IsBlend(false);
        _stateMachine.BossController.BossAttack.StopAttack();
        _stateMachine.BossController.Move.StopMove();
    }



    public override void Exit()
    {

    }


    public override void FixedUpdate()
    {

    }

    public override void Update()
    {

    }

    public override void LateUpdate()
    {


    }
}
