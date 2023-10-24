using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleState : PlayerStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        //カメラのFOV設定
        _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetFOV();

        //トドメをさせる敵を探す
        _stateMachine.PlayerController.FinishingAttack.SearchFinishingEnemy();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.ParticleStopUpdata();

        if (_stateMachine.PlayerController.FinishingAttack.IsCanFinishing &&
_stateMachine.PlayerController.InputManager.IsFinishAttackDown)
        {
            _stateMachine.TransitionTo(_stateMachine.FinishAttackState);
            return;
        }   //トドメ


        if (_stateMachine.PlayerController.InputManager.IsAttack && !_stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.IsEndMagic)
        {
            _stateMachine.TransitionTo(_stateMachine.AttackState);
            return;
        }   //攻撃

        //if (_stateMachine.PlayerController.SetUp.IsSetUp)
        //{
        //    _stateMachine.TransitionTo(_stateMachine.SetUpIdle);
        //    //構えに行く際の設定
        //    _stateMachine.PlayerController.SetUp.ArrangementStartSetUp();
        //    return;
        //}   //構え


        if (_stateMachine.PlayerController.InputManager.HorizontalInput != 0
             || _stateMachine.PlayerController.InputManager.VerticalInput != 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateWalk);
            return;
        }

        //if (_stateMachine.PlayerController.InputManager.IsJumping)
        //{
        //    _stateMachine.TransitionTo(_stateMachine.StateJump);
        //    return;
        //}

    }
}
