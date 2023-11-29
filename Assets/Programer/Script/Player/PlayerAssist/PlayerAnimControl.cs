using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimControl
{
    [Header("èeÇ…ïœçX")]
    [SerializeField] private string _toGunAnim = "Player_ChangeWeapon_ToGun";
    [Header("åïÇ…à⁄çs")]
    [SerializeField] private string _toSword = "Player_ChangeWeapon_ToSword";

    [Header("çUåÇâÒêî")]
    [SerializeField] private string _attackNum = "AttackNum";

    [Header("çUåÇÇÃTrigger")]
    [SerializeField] private string _attackTrigger = "";

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
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
        _playerControl.Animator.SetFloat("SpeedY", _playerControl.Rb.velocity.y);
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

    public void StartFinishAttack(AttackType weaponType)
    {
        _playerControl.Animator.SetBool("IsFinishAttack", true);
        if (weaponType == AttackType.ShortChantingMagick)
        {
            _playerControl.Animator.Play("Player_FinishingGun_1");
        }
        else
        {
            _playerControl.Animator.Play("Player_FinishingNear_1");
        }
    }

    public void SetBlendAnimUnderBody(bool isOn)
    {
        _playerControl.Animator.SetBool("IsNoBlendAnimation", isOn);
    }

    public void StopFinishAttack()
    {
        _playerControl.Animator.SetBool("IsFinishAttack", false);
    }

    public void EndFinishAttack(AttackType weaponType)
    {
        if (weaponType == AttackType.ShortChantingMagick)
        {
            _playerControl.Animator.Play("Player_FinishingGun_1_Complet");
        }
        else
        {
            _playerControl.Animator.Play("Player_FinishingNear_1_Complet");
        }
    }

    public void SetIsChanting(bool isOn)
    {
        _playerControl.Animator.SetBool("IsChanting", isOn);
    }

    public void ChangeWeapon(bool isGun)
    {
        if (isGun)
        {
            _playerControl.Animator.Play(_toGunAnim);
        }
        else
        {
            _playerControl.Animator.Play(_toSword);
        }
    }

    public void SetIsSetUp(bool isSetUp)
    {
        _playerControl.Animator.SetBool("IsSetUp", isSetUp);
    }


    /// <summary>çUåÇâÒêîÇê›íËÇ∑ÇÈ</summary>
    /// <param name="num"></param>
    public void SetAttackNum(int num)
    {
        _playerControl.Animator.SetInteger(_attackNum, num);
    }

    /// <summary>çUåÇÇÇ∑ÇÈ</summary>
    public void SetAttackTrigger()
    {
        _playerControl.Animator.SetTrigger(_attackTrigger);
    }

}
