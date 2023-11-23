using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState : PlayerStateBase
{
    public override void Enter()
    {
        _stateMachine.PlayerController.Animator.Play("Player_RelodeMagick 0");
        _stateMachine.PlayerController.Animator.SetBool("IsAttack", true);
        _stateMachine.PlayerController.Attack.DoAttack();
    }

    public override void Exit()
    {
        _stateMachine.PlayerController.Animator.SetBool("IsAttack", false);
        _stateMachine.PlayerController.Attack.EndAttack();
    }

    public override void FixedUpdate()
    {
        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Move();
        _stateMachine.PlayerController.Attack.ShortChantingMagicAttack.ShortChantingMagicAttackMove.Rotation();

        //ƒgƒhƒ‚ð‚³‚¹‚é“G‚ð’T‚·
        _stateMachine.PlayerController.FinishingAttack.SearchFinishingEnemy();
    }

    public override void LateUpdate()
    {

    }

    public override void Update()
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
        }   //UŒ‚



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
