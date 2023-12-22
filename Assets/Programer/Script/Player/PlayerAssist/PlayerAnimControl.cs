using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimControl
{
    [Header("銃に変更")]
    [SerializeField] private string _toGunAnim = "Player_ChangeWeapon_ToGun";
    [Header("剣に移行")]
    [SerializeField] private string _toSword = "Player_ChangeWeapon_ToSword";

    [Header("攻撃回数")]
    [SerializeField] private string _attackNum = "AttackNum";

    [Header("攻撃のTrigger")]
    [SerializeField] private string _attackTrigger = "";

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void ChangeAttribute()
    {
        _playerControl.Animator.Play("Player_ChangeAttribute");
       // _playerControl.Animator.SetTrigger("IsChangeAttribute");
    }

    public void Avoid(bool isStart)
    {
        if (isStart)
        {
            _playerControl.Animator.Play("Player_Avoid");
        }
        else
        {
            _playerControl.Animator.Play("Player_AvoidEnd");
        }
    }

    public void AnimSet()
    {
        _playerControl.Animator.SetFloat("Speed", _playerControl.Rb.velocity.magnitude);
        _playerControl.Animator.SetFloat("InputX", _playerControl.InputManager.HorizontalInput);
        _playerControl.Animator.SetFloat("InputY", _playerControl.InputManager.VerticalInput);
        _playerControl.Animator.SetBool("IsGround", _playerControl.GroundCheck.IsHit());

        var h = _playerControl.InputManager.HorizontalInput;
        var v = _playerControl.InputManager.VerticalInput;

        if (h != 0 || v != 0)
        {
            _playerControl.Animator.SetBool("IsMoveInput", true);
        }
        else if (h == 0 && v == 0)
        {
            _playerControl.Animator.SetBool("IsMoveInput", false);
        }
    }

    public void SetLongMagic(bool isON)
    {
        _playerControl.Animator.SetBool("IsEndLongMagic", isON);
    }

    public void SetIsIceAttack(bool isAttackIce)
    {
        _playerControl.Animator.SetBool("IsIce", isAttackIce);
    }

    /// <summary>トドメ開始のアニメーション </summary>
    public void StartFinishAttack()
    {
        _playerControl.Animator.SetBool("IsFinishAttack", true);

        int r = Random.Range(0, 2);

        if (r == 0)
        {
            _playerControl.Animator.Play("Player_FinishAttack_Start1");
        }
        else
        {
            _playerControl.Animator.Play("Player_FinishAttack_Start2");
        }
    }

    /// <summary>トドメ中断 </summary>
    public void StopFinishAttack()
    {
        _playerControl.Animator.SetBool("IsFinishAttack", false);
    }

    /// <summary>トドメ完了のアニメーション </summary>
    public void EndFinishAttack()
    {
        _playerControl.Animator.Play("Player_FinishinAttack_Complet1");
    }

    public void PlayDead()
    {
        _playerControl.Animator.Play("Dead");
    }

    public void IsDead(bool isDead)
    {
        _playerControl.Animator.SetBool("IsDead", isDead);
    }

    public void PlayDamage()
    {
        _playerControl.Animator.Play("Damage");
    }

    public void IsDamage(bool isDamage)
    {
        _playerControl.Animator.SetBool("IsDamage", isDamage);
    }


    public void SetBlendAnimUnderBody(bool isOn)
    {
        _playerControl.Animator.SetBool("IsBlendLegAnimation", isOn);
    }

    public void SetBlendAnimation(bool isOn)
    {
        _playerControl.Animator.SetBool("IsBlendAnimation", isOn);
    }




    public void SetIsChanting(bool isOn)
    {
        _playerControl.Animator.SetBool("IsChanting", isOn);
    }


    public void SetIsSetUp(bool isSetUp)
    {
        _playerControl.Animator.SetBool("IsSetUp", isSetUp);
    }


    /// <summary>攻撃回数を設定する</summary>
    /// <param name="num"></param>
    public void SetAttackNum(int num)
    {
        _playerControl.Animator.SetInteger(_attackNum, num);
    }

    public void SetIsAttack(bool isAttack)
    {
        _playerControl.Animator.SetBool("IsAttack", isAttack);
    }


    /// <summary>攻撃をする</summary>
    public void SetAttackTrigger(bool isTrigger)
    {
        if (isTrigger)
        {
            _playerControl.Animator.SetTrigger(_attackTrigger);
        }
        else
        {
            _playerControl.Animator.ResetTrigger(_attackTrigger);
        }

    }

}
