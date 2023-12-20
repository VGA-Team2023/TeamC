using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.PlayerDamage.Damage();
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        //移動処理
        _stateMachine.PlayerController.Move.Move(PlayerMove.MoveType.Walk);

        //カメラのFOV設定
        _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetDefaultFOV();

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

        //属性変更のクールタイム
        _stateMachine.PlayerController.PlayerAttributeControl.CoolTime();

        //ダメージ、無敵時間計測
        _stateMachine.PlayerController.PlayerDamage.CountDamageTime();

        if (!_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
        }

    }
}
