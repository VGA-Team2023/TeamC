using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack2
{
    [Header("@詳細設定")]
    [SerializeField] private AttackMagic _attackMagic;

    private int _attackCount = 0;

    private int _maxAttackCount = 0;

    /// <summary>攻撃中に攻撃ボタンを押したかどうか</summary>
    private bool _isPushAttack = false;

    /// <summary>攻撃可能かどうか</summary>
    private bool _isCanNextAction = false;

    private bool _isAttackFirstSetUp = false;

    private bool _isAttackNow = false;


    private bool _isAttackInputed = true;

    /// <summary>魔法の連続攻撃数により、攻撃可能かどうか</summary>
    private bool _isCanTransitionAttackState = true;

    private PlayerAttribute _firstAttribute;

    public PlayerAttribute FirstAttribute => _firstAttribute;

    public bool IsCanTransitionAttackState { get => _isCanTransitionAttackState; set => _isCanTransitionAttackState = value; }
    public bool IsAttackInputed => _isAttackInputed;
    private PlayerControl _playerControl;
    public bool IsPushAttack => _isPushAttack;

    public AttackMagic AttackMagic => _attackMagic;

    public bool IsAttackNow { get => _isAttackNow; set => _isAttackNow = value; }
    public bool IsCanNextAction { get => _isCanNextAction; set => _isCanNextAction = value; }
    public bool IsAttackFirstGun => _isAttackFirstSetUp;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _attackMagic.Init(playerControl);
        _attackMagic.SetMagicBase(_playerControl.PlayerAttributeControl.PlayerAttribute);
        _maxAttackCount = _attackMagic.MagicBase.MagickData.Count;
    }

    public void ResetAttack()
    {
        _isCanTransitionAttackState = true;
    }

    public void CheckEnd()
    {
        if (_isCanTransitionAttackState == false)
        {
            _isCanTransitionAttackState = true;
        }
        _isCanNextAction = true;
    }

    public void DoAttack()
    {
        //カメラ変更
        _playerControl.CameraControl.UseAttackChargeCamera();

        _firstAttribute = _playerControl.PlayerAttributeControl.PlayerAttribute;

        //属性別の攻撃設定
        if (_playerControl.PlayerAttributeControl.PlayerAttribute == PlayerAttribute.Ice)
        {
            //音
            _playerControl.PlayerAudio.AttackCharge(true, true);

            //ボイス
            AudioController.Instance.Voice.Play(VoiceState.PlayerCastingIce);

            //アニメーション設定
            _playerControl.PlayerAnimControl.SetIsIceAttack(true);
        }
        else
        {
            //音
            _playerControl.PlayerAudio.AttackCharge(true, false);

            //ボイス
            AudioController.Instance.Voice.Play(VoiceState.PlayerCastingGrass);

            //アニメーション設定
            _playerControl.PlayerAnimControl.SetIsIceAttack(false);
        }

        //アニメーション設定
        _playerControl.PlayerAnimControl.SetAttackTrigger(false);

        _isAttackNow = true;
        _isCanNextAction = false;
        _isAttackInputed = false;

        _attackCount++;

        _playerControl.Animator.SetBool("IsDoAttack", false);

        _playerControl.Animator.SetInteger("AttackNum", Random.Range(0, 1));

        _playerControl.Animator.Play("SetUp" + 1, 0);


        if (_attackCount == _maxAttackCount)
        {
            _isCanTransitionAttackState = false;
        }

        _attackMagic.SetMagicBase(_playerControl.PlayerAttributeControl.PlayerAttribute);
        _attackMagic.MagicBase.SetUpMagick();
    }

    public void CheckInput()
    {
        if (!_isAttackInputed)
        {
            _attackMagic.MagicBase.SetUpChargeMagic(_attackCount);
            if (_playerControl.InputManager.IsAttackUp)
            {
                //アニメーション再生
                //_playerControl.PlayerAnimControl.SetAttackTrigger();

                //攻撃ボイス
                AudioController.Instance.Voice.Play(VoiceState.PlayerAttack);

                //音 
                if (_playerControl.PlayerAttributeControl.PlayerAttribute == PlayerAttribute.Ice)
                {
                    _playerControl.PlayerAudio.AttackCharge(false, true);
                }
                else
                {
                    _playerControl.PlayerAudio.AttackCharge(false, false);
                }
                _playerControl.Animator.SetBool("IsDoAttack", true);
                _isAttackInputed = true;
                _attackMagic.Attack(_attackCount);
            }
        }
    }


    /// <summary>攻撃中に攻撃ボタンを押したかどうかを確認する</summary>
    public void AttackInputedCheck()
    {
        if ((_playerControl.InputManager.IsAttacks || _playerControl.InputManager.IsAttack) && _isAttackInputed)
        {
            _isPushAttack = true;
        }
    }

    /// <summary>攻撃終わりの処理</summary>
    public void EndAttack()
    {
        _isPushAttack = false;
        _isCanNextAction = false;
        _playerControl.Animator.SetBool("IsDoAttack", false);

        if (_attackCount == _maxAttackCount)
        {
            _attackCount = 0;
        }
    }

    /// <summary>攻撃の中断処理</summary>
    public void StopAttack()
    {
        _attackMagic.StopMagic(_attackCount);

        _isPushAttack = false;
        _isCanNextAction = true;
        _isCanTransitionAttackState = true;
        _isAttackInputed = false;
        _isAttackNow = false;

        _attackCount = 0;
    }

}
