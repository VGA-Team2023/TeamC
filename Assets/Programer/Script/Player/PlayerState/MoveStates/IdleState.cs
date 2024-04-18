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
        //LockOn機能
        _stateMachine.PlayerController.LockOn.CheckLockOn();

        //属性変更確認


        //回避のクールタイム計測
        _stateMachine.PlayerController.Avoid.CountCoolTime();

        //属性変更のクールタイム
        _stateMachine.PlayerController.PlayerAttributeControl.CoolTime();

        //ダメージ、無敵時間計測
        _stateMachine.PlayerController.PlayerDamage.CountDamageTime();

        if (_stateMachine.PlayerController.PlayerHp.IsDead)
        {
            _stateMachine.TransitionTo(_stateMachine.DeadState);
            return;
        }   //瀕死

        if(_stateMachine.PlayerController.PlayerDamage.IsDamage)
        {
            _stateMachine.TransitionTo(_stateMachine.DamageState);
            return;
        }   //ダメージ

        if (_stateMachine.PlayerController.FinishingAttack.IsCanFinishing && _stateMachine.PlayerController.InputManager.IsFinishAttackDown)
        {
            _stateMachine.TransitionTo(_stateMachine.FinishAttackState);
            return;
        }   //トドメ


        if (_stateMachine.PlayerController.IsNewAttack)
        {
            if (_stateMachine.PlayerController.InputManager.IsAttack && _stateMachine.PlayerController.Attack2.IsCanTransitionAttackState)
            {
                _stateMachine.TransitionTo(_stateMachine.AttackState);
                return;
            }   //攻撃
        }
        else
        {
            if (_stateMachine.PlayerController.InputManager.IsAttack && !_stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.IsEndMagic)
            {
                _stateMachine.TransitionTo(_stateMachine.AttackState);
                return;
            }   //攻撃
        }

        //属性変更
        if (_stateMachine.PlayerController.InputManager.IsChangeAttribute)
        {
            _stateMachine.TransitionTo(_stateMachine.ChangeAttributeState);
            return;
        }

        if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
        {
            _stateMachine.PlayerController.Avoid.SetAvoidDir();
            _stateMachine.TransitionTo(_stateMachine.AvoidState);
            return;
        }   //回避


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
