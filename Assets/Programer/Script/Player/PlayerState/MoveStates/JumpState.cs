using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JumpState : PlayerStateBase
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        if (_stateMachine.PlayerController.Rb.velocity.y > 0)
        {
            _stateMachine.TransitionTo(_stateMachine.StateUpAir);
            return;
        }
    }
}
