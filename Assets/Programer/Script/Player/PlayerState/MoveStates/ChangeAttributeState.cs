using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChangeAttributeState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.PlayerAttributeControl.ChangeAttribute();
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
        //クールタイム計測
        _stateMachine.PlayerController.PlayerAttributeControl.CoolTime();

        //LockOn機能
        _stateMachine.PlayerController.LockOn.CheckLockOn();

        //属性変更のクールタイム
        _stateMachine.PlayerController.PlayerAttributeControl.CoolTime();

        //回避のクールタイム計測
        _stateMachine.PlayerController.Avoid.CountCoolTime();

        //ダメージ、無敵時間計測
        _stateMachine.PlayerController.PlayerDamage.CountDamageTime();

        if (_stateMachine.PlayerController.PlayerHp.IsDead)
        {
            _stateMachine.PlayerController.PlayerAttributeControl.StopChange();
            _stateMachine.TransitionTo(_stateMachine.DeadState);
            return;
        }   //瀕死ステート

        if (_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.PlayerController.PlayerAttributeControl.StopChange();
            _stateMachine.TransitionTo(_stateMachine.DamageState);
            return;
        }   //ダメージ

        if (_stateMachine.PlayerController.PlayerAttributeControl.IsCoolTimeEnd)
        {
            if (_stateMachine.PlayerController.InputManager.HorizontalInput == 0 && _stateMachine.PlayerController.InputManager.VerticalInput == 0)
            {
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
                return;
            }
            else
            {
                _stateMachine.TransitionTo(_stateMachine.StateWalk);
                return;
            }

        }
    }
}
