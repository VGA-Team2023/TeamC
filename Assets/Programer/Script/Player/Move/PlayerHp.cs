using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHp
{
    [Header("===UI設定===")]
    [SerializeField] private PlayerHpUI _uiHp;

    [Header("PlayerのHp")]
    [SerializeField] private float _hp;

    private float _nowHp = 0;

    private bool _isDead = false;

    public bool IsDead => _isDead;

    private PlayerControl _playerControl;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _uiHp.Init(playerControl, _hp);
        _nowHp = _hp;
    }

    public bool AddDamage(float hp)
    {
        _nowHp -= hp;
        _uiHp.SetValue(_nowHp);

        if (_nowHp < 0)
        {
            _isDead = true;
            return true;
        }
        return false;
    }

    public void ReVive()
    {
        GameManager.Instance.SpecialMovingPauseManager.PauseResume(false);
        _playerControl.PlayerAnimControl.IsDead(false);

        _isDead = false;
        _nowHp = _hp;
        _uiHp.SetValue(_nowHp);
    }

}
