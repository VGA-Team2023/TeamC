using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvoidState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Avoid.StartAvoid();
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        //ƒJƒƒ‰‚ÌFOVİ’è
        _stateMachine.PlayerController.CameraControl.SetUpCameraSetting.SetDefaultFOV();

        //‰ñ”ğ‚ÌˆÚ“®ˆ—
        _stateMachine.PlayerController.Avoid.DoAvoid();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //‰ñ”ğ‚ÌÀsŠÔ‚ÌŒv‘ª
        _stateMachine.PlayerController.Avoid.CountAvoidTime();

        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.ParticleStopUpdata();

        if (_stateMachine.PlayerController.Avoid.IsEndAnim)
        {
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }

    }
}