using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortCantingMagicAttackEffectEvent : MonoBehaviour
{

    [SerializeField] private PlayerControl _playerControl;


    public void ShowShirtCantingMagicAttackCircle()
    {
        _playerControl.Attack.ShortChantingMagicAttack.ShortChantingMagicData.SetOnlyMagic();
    }

    public void EndShortMagicCanting()
    {
        _playerControl.Attack.ShortChantingMagicAttack.ShortChantingMagicData.CanUseMagic();
    }

}
