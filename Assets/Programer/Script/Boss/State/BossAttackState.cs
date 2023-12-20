using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttackState : BossStateBase
{
    public override void Enter()
    {

    }



    public override void Exit()
    {


    }


    public override void FixedUpdate()
    {
        //回転設定
        _stateMachine.BossController.BossRotate.SetRotation();

        _stateMachine.BossController.Move.CheckPlayerDir();

        if (_stateMachine.BossController.BossAttack.BossAttackMagicTypes == BossAttackType.Follow)
        {

        }
        else
        {
            _stateMachine.BossController.Move.Move();
        }

    }

    public override void Update()
    {
        //攻撃
        _stateMachine.BossController.BossAttack.Updata();

        _stateMachine.BossController.Move.CheckWall();
        _stateMachine.BossController.Move.CountMoveTime();

        if (_stateMachine.BossController.BossHp.IsKnockDown)
        {
            _stateMachine.TransitionTo(_stateMachine.StateFinish);
            return;
        }   //トドメ

        if (!_stateMachine.BossController.BossAttack.IsAttackNow)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }   //トドメ

    }

    public override void LateUpdate()
    {


    }


}
