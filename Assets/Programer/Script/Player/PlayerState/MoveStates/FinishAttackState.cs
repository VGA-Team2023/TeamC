using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishAttackState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.FinishingAttack.StartFinishingAttack();

        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimUnderBody(true);
    }

    public override void Exit()
    {
        //エフェクトを消すかどうか確認する
        _stateMachine.PlayerController.FinishingAttack.FinishEffectCheck();

        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimUnderBody(false);
    }


    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.FinishingAttack.FinishingAttackMove.Move();
        _stateMachine.PlayerController.FinishingAttack.FinishingAttackMove.Rotation();

        _stateMachine.PlayerController.FinishingAttack.SetUI();


        _stateMachine.PlayerController.CameraControl.FinishAttackCamera.DoFinishCameraSettingFirst();

        _stateMachine.PlayerController.CameraControl.FinishAttackCamera.DoChangeDutch();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        if (!_stateMachine.PlayerController.FinishingAttack.DoFinishing() || _stateMachine.PlayerController.FinishingAttack.IsEndFinishAnim)
        {
            //魔法陣を出す
            _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.SetMagick();

            _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }


    }
}