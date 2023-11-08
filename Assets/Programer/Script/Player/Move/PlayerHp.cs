using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHp
{
    [Header("===UIê›íË===")]
    [SerializeField] private PlayerHpUI _uiHp;

    [Header("PlayerÇÃHp")]
    [SerializeField] private float _hp;

    private float _nowHp = 0;

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
            return true;
        }
        return false;
    }

    public void ReVive()
    {
        _nowHp = _hp;
        _uiHp.SetValue(_nowHp);
    }

}
