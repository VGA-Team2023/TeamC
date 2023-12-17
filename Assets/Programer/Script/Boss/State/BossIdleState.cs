using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossIdleState : BossStateBase
{

    public override void Enter()
    {

    }



    public override void Exit()
    {


    }


    public override void FixedUpdate()
    {
        //âÒì]ê›íË
        _stateMachine.BossController.BossRotate.SetRotation();

        _stateMachine.BossController.Move.Move();

        _stateMachine.BossController.Move.CheckPlayerDir();
    }

    public override void Update()
    {
        //çUåÇ
        _stateMachine.BossController.BossAttack.Updata();

        _stateMachine.BossController.Move.CheckWall();
        _stateMachine.BossController.Move.CountMoveTime();

        if (_stateMachine.BossController.BossHp.IsKnockDown)
        {
            _stateMachine.TransitionTo(_stateMachine.StateFinish);
            return;
        }   //ÉgÉhÉÅ

    }

    public override void LateUpdate()
    {


    }




}
