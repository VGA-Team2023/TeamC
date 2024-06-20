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
        //カメラ変更
        _stateMachine.PlayerController.CameraControl.UseDefultCamera(true);
    }

    public override void FixedUpdate()
    {
        if (_stateMachine.PlayerController.Avoid.IsEndAvoid)
        {
            _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetDefaultFOV();
        }
        else
        {
            //カメラのFOV設定
            _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.AvoidFov();
        }


        //回避の移動処理
        _stateMachine.PlayerController.Avoid.DoAvoid();

        //トドメをさせる敵を探す
        _stateMachine.PlayerController.FinishingAttack.SearchFinishingEnemy();

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

        //属性変更のクールタイム
        _stateMachine.PlayerController.PlayerAttributeControl.CoolTime();

        //ダメージ、無敵時間計測
        _stateMachine.PlayerController.PlayerDamage.CountDamageTime();

        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.ParticleStopUpdata();

        if (_stateMachine.PlayerController.Avoid.IsEndAnim)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }

    }
}