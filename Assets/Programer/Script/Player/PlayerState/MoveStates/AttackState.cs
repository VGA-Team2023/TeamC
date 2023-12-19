using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState : PlayerStateBase
{
    public override void Enter()
    {
        if (_stateMachine.PlayerController.IsNewAttack)
        {
            _stateMachine.PlayerController.Animator.SetBool("IsAttack", true);
            _stateMachine.PlayerController.Attack2.DoAttack();
        }
        else
        {
            _stateMachine.PlayerController.Animator.Play("Player_RelodeMagick 0");
            _stateMachine.PlayerController.Animator.SetBool("IsAttack", true);
            _stateMachine.PlayerController.Attack.DoAttack();
        }
        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimUnderBody(true);
    }

    public override void Exit()
    {
        if (_stateMachine.PlayerController.IsNewAttack)
        {
            _stateMachine.PlayerController.Animator.SetBool("IsAttack", false);
            _stateMachine.PlayerController.Attack2.EndAttack();
        }
        else
        {
            _stateMachine.PlayerController.Animator.SetBool("IsAttack", false);
            _stateMachine.PlayerController.Attack.EndAttack();
        }
        _stateMachine.PlayerController.PlayerAnimControl.SetBlendAnimUnderBody(false);
    }

    public override void FixedUpdate()
    {

        if (_stateMachine.PlayerController.IsNewAttack)
        {
            _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Move();
            _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Rotation();
            //トドメをさせる敵を探す
            _stateMachine.PlayerController.FinishingAttack.SearchFinishingEnemy();
        }
        else
        {
            _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Move();
            _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Rotation();

            //トドメをさせる敵を探す
            _stateMachine.PlayerController.FinishingAttack.SearchFinishingEnemy();
        }

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

        _stateMachine.PlayerController.Attack2.AttackMagic.MagicBase.CountCoolTime();

        if (_stateMachine.PlayerController.PlayerHp.IsDead)
        {
            _stateMachine.PlayerController.Attack2.StopAttack();
            _stateMachine.TransitionTo(_stateMachine.DeadState);
            return;
        }   //瀕死ステート

        //if (_stateMachine.PlayerController.PlayerDamage.IsDamage)
        //{
        //    _stateMachine.PlayerController.Attack2.StopAttack();
        //    _stateMachine.TransitionTo(_stateMachine.DamageState);
        //    return;
        //}   //ダメージ

        if (_stateMachine.PlayerController.IsNewAttack)
        {
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



            if (_stateMachine.PlayerController.Attack2.IsCanNextAttack && !_stateMachine.PlayerController.Attack2.IsAttackNow && _stateMachine.PlayerController.Attack2.IsAttackInput)
            {
                Debug.Log("Attack=>Idle");
                _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
                return;
            }   //Idle
        }
        else
        {
            _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.ParticleStopUpdata();

            _stateMachine.PlayerController.Attack.AttackInputedCheck();
            _stateMachine.PlayerController.Attack.CountInput();

            if (_stateMachine.PlayerController.Attack.IsCanNextAttack &&
            _stateMachine.PlayerController.Attack.IsPushAttack)
            {
                if (!_stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicData.IsEndMagic)
                {
                    Debug.Log("Attack=>Attack");
                    _stateMachine.TransitionTo(_stateMachine.AttackState);
                    return;
                }
            }   //攻撃



            if (_stateMachine.PlayerController.Attack.IsCanNextAttack && !_stateMachine.PlayerController.Attack.IsAttackNow && _stateMachine.PlayerController.Attack.IsAttackInput)
            {
                Debug.Log("Attack=>Idle");
                _stateMachine.PlayerController.PlayerAnimControl.SetIsSetUp(false);
                _stateMachine.PlayerController.Attack.EndAttackNoNextAttack();
                _stateMachine.TransitionTo(_stateMachine.StateIdle);
                return;
            }
        }
    }
}
