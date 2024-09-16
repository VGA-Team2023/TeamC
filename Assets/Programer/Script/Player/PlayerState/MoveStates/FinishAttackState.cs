using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishAttackState : PlayerStateBase
{
    public override void Enter()
    {
        //LockOnのUIを非表示に
        _stateMachine.PlayerController.LockOn.PlayerLockOnUI.LockOn(false);

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
        //移動
        _stateMachine.PlayerController.FinishingAttack.FinishingAttackMove.Move();
        //回転
        _stateMachine.PlayerController.FinishingAttack.FinishingAttackMove.Rotation();
        //UI
        _stateMachine.PlayerController.FinishingAttack.SetUI();
        //カメラの動き
        _stateMachine.PlayerController.FinishingAttack.FinishCameraFixedUpadata();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //回避のクールタイム計測
        _stateMachine.PlayerController.Avoid.CountCoolTime();

        //属性変更のクールタイム
        _stateMachine.PlayerController.PlayerAttributeControl.CoolTime();

        //ダメージ、無敵時間計測
        _stateMachine.PlayerController.PlayerDamage.CountDamageTime();

        ////ダメージ
        //if (_stateMachine.PlayerController.PlayerDamage.IsDamage)
        //{
        //    _stateMachine.PlayerController.FinishingAttack.StopFinishingAttack();
        //    _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);

        //    _stateMachine.TransitionTo(_stateMachine.DamageState);
        //    return;
        //}

        //死亡
        if (_stateMachine.PlayerController.PlayerHp.IsDead)
        {
            _stateMachine.PlayerController.FinishingAttack.StopFinishingAttack();
            _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);

            _stateMachine.TransitionTo(_stateMachine.DamageState);
            return;
        }

        if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid && !_stateMachine.PlayerController.FinishingAttack.IsCompleted)
        {
            _stateMachine.PlayerController.FinishingAttack.StopFinishingAttack();
            _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);
            _stateMachine.PlayerController.Avoid.SetAvoidDir();
            _stateMachine.TransitionTo(_stateMachine.AvoidState);
            return;
        }   //回避

        if (!_stateMachine.PlayerController.FinishingAttack.DoFinishing() || _stateMachine.PlayerController.FinishingAttack.IsEndFinishAnim)
        {
            _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }


    }
}