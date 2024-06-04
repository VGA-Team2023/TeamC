using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState : PlayerStateBase
{
    public override void Enter()
    {
        //アニメーションbool設定
        _stateMachine.PlayerController.PlayerAnimControl.SetIsAttack(true);
        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimUnderBody(true);

        //攻撃処理
        _stateMachine.PlayerController.Attack2.DoAttack();

        //コントローラーの振動
        _stateMachine.PlayerController.ControllerVibrationManager.StartVibration();
    }

    public override void Exit()
    {
        //アニメーションbool設定
        _stateMachine.PlayerController.PlayerAnimControl.SetIsAttack(false);
        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimUnderBody(false);

        //攻撃終了設定
        _stateMachine.PlayerController.Attack2.EndAttack();

        //コントローラーの振動
        _stateMachine.PlayerController.ControllerVibrationManager.StopVibration();
    }

    public override void FixedUpdate()
    {
        Debug.Log("AttackNOw");
        //移動と回転、設定
        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Move();
        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Rotation();

        //トドメをさせる敵を探す
        _stateMachine.PlayerController.FinishingAttack.SearchFinishingEnemy();

        //LockOnのUI設定
        _stateMachine.PlayerController.LockOn.PlayerLockOnUI.UpdateFinishingUIPosition();

        _stateMachine.PlayerController.CameraControl.AttackCamera.AvoidFov(_stateMachine.PlayerController.InputManager.HorizontalInput);
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
    {
        //LockOn機能
        _stateMachine.PlayerController.LockOn.CheckLockOn();

        //魔法を射出
        _stateMachine.PlayerController.Attack2.AttackMagic.MagicBase.UseMagics();

        //回避のクールタイム計測
        _stateMachine.PlayerController.Avoid.CountCoolTime();

        //属性変更のクールタイム
        _stateMachine.PlayerController.PlayerAttributeControl.CoolTime();

        //ダメージ、無敵時間計測
        _stateMachine.PlayerController.PlayerDamage.CountDamageTime();

        _stateMachine.PlayerController.Attack2.AttackMagic.MagicBase.CountCoolTime();

        if (_stateMachine.PlayerController.PlayerHp.IsDead)
        {
            _stateMachine.PlayerController.Attack2.StopAttack();
            _stateMachine.TransitionTo(_stateMachine.DeadState);
            return;
        }   //瀕死ステート

        if (_stateMachine.PlayerController.InputManager.IsAvoid && _stateMachine.PlayerController.Avoid.IsCanAvoid)
        {
            _stateMachine.PlayerController.Attack2.StopAttack();
            _stateMachine.PlayerController.Avoid.SetAvoidDir();
            _stateMachine.TransitionTo(_stateMachine.AvoidState);
            return;
        }   //回避

        //if (_stateMachine.PlayerController.PlayerDamage.IsDamage)
        //{
        //    _stateMachine.PlayerController.Attack2.StopAttack();
        //    _stateMachine.TransitionTo(_stateMachine.DamageState);
        //    return;
        //}   //ダメージ


        _stateMachine.PlayerController.Attack2.AttackInputedCheck();
        _stateMachine.PlayerController.Attack2.CheckInput();

        //if (_stateMachine.PlayerController.Attack2.IsCanNextAttack &&
        //_stateMachine.PlayerController.Attack2.IsPushAttack)
        //{
        //    if (_stateMachine.PlayerController.Attack2.IsCanTransitionAttackState)
        //    {
        //        Debug.Log("Attack=>Attack");
        //        _stateMachine.TransitionTo(_stateMachine.AttackState);
        //        return;
        //    }
        //}   //攻撃


        if (_stateMachine.PlayerController.Attack2.IsCanNextAction && !_stateMachine.PlayerController.Attack2.IsAttackNow && _stateMachine.PlayerController.Attack2.IsAttackInputed)
        {
            Debug.Log("Attack=>Idle");
            _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);
            _stateMachine.TransitionTo(_stateMachine.StateIdle);
            return;
        }   //Idle

    }
}
