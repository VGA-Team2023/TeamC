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
        //ˆÚ“®ˆ—
        _stateMachine.PlayerController.Move.Move(PlayerMove.MoveType.Walk);

        //ƒJƒƒ‰‚ÌFOVİ’è
        _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetDefaultFOV();

        //LockOn‚ÌUIİ’è
        _stateMachine.PlayerController.LockOn.PlayerLockOnUI.UpdateFinishingUIPosition();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //LockOn‹@”\
        _stateMachine.PlayerController.LockOn.CheckLockOn();

        _stateMachine.PlayerController.PlayerDamage.CountDamageTime();

        if (!_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
        }

    }
}
