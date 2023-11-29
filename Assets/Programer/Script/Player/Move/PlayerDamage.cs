using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDamage
{
    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Damage(float damage)
    {
        if (_playerControl.PlayerHp.AddDamage(damage))
        {

        }
        else
        {

        }
    }


}
