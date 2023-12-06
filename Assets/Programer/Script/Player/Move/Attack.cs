using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    [Header("短い詠唱の魔法攻撃設定")]
    [SerializeField] private ShortChantingMagickAttack _shortChantingMagicAttack;

    [SerializeField] private GameObject _notame;

    [SerializeField] private GameObject _tame;

    private float _countTime = 0;

    /// <summary>攻撃中に攻撃ボタンを押したかどうか</summary>
    private bool _isPushAttack = false;

    private bool _isCanNextAttack = false;

    private bool _isAttackFirstSetUp = false;

    private bool _isAttackNow = false;

    private PlayerControl _playerControl;
    private bool _isAttackInput = false;

 
    public bool IsAttackInput => _isAttackInput;
    public bool IsPushAttack => _isPushAttack;
    public ShortChantingMagickAttack ShortChantingMagicAttack => _shortChantingMagicAttack;

    public bool IsAttackNow { get => _isAttackNow; set => _isAttackNow = value; }
    public bool IsCanNextAttack { get => _isCanNextAttack; set => _isCanNextAttack = value; }
    public bool IsAttackFirstGun => _isAttackFirstSetUp;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _shortChantingMagicAttack.Init(playerControl);
    }


    public void DoAttack()
    {
        _notame.SetActive(true);

        _isAttackNow = true;
        _isCanNextAttack = false;
        _isAttackInput = false;

        _shortChantingMagicAttack.ShortChantingMagicData.SetTame();

        _countTime = 0;
    }

    public void CountInput()
    {
        if (!_isAttackInput)
        {
            _countTime += Time.deltaTime;

            if (_countTime > _shortChantingMagicAttack.TameTime && !_tame.activeSelf)
            {
                _notame.SetActive(false);
                _tame.SetActive(true);
            }

            if (_playerControl.InputManager.IsAttackUp)
            {
                _isAttackInput = true;
                _shortChantingMagicAttack.Attack(_countTime);
            }
        }
    }


    /// <summary>攻撃中に攻撃ボタンを押したかどうかを確認する</summary>
    public void AttackInputedCheck()
    {
        if ((_playerControl.InputManager.IsAttacks || _playerControl.InputManager.IsAttack) && _isAttackInput)
        {
            Debug.Log("OK");
            _isPushAttack = true;
        }
    }

    /// <summary>攻撃終わりの処理</summary>
    public void EndAttack()
    {
        _tame.SetActive(false);
        _notame.SetActive(false);

        _isPushAttack = false;
        _isCanNextAttack = false;
    }

    /// <summary>攻撃を完全に終えた際の処理</summary>
    public void EndAttackNoNextAttack()
    {

    }

}

/// <summary>
/// 攻撃のタイプ
/// </summary>
public enum AttackType
{
    /// <summary>短い詠唱の攻撃</summary>
    ShortChantingMagick,
    /// <summary>長い詠唱の攻撃</summary>
    LongChantingMagick,
}

/// <summary>
/// 魔法の属性
/// </summary>
public enum MagickType
{
    /// <summary>氷属性 </summary>
    Ice,

    /// <summary>草属性 </summary>
    Grass,
}