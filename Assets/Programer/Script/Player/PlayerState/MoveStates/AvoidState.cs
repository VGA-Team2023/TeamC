using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvoidState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Avoid.StartAvoid();
        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimUnderBody(false);
        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimation(false);
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimation(true);
    }

    public override void FixedUpdate()
    {
        //カメラのFOV設定
        _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetDefaultFOV();

        //回避の移動処理
        _stateMachine.PlayerController.Avoid.DoAvoid();

        //LockOnのUI設定
        _stateMachine.PlayerController.LockOn.PlayerLockOnUI.UpdateFinishingUIPosition();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //LockOn機能
        _stateMachine.PlayerController.LockOn.CheckLockOn();

        //回避の実行時間の計測
        _stateMachine.PlayerController.Avoid.CountAvoidTime();

        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.ParticleStopUpdata();

        if (_stateMachine.PlayerController.Avoid.IsEndAnim)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }

    }
}