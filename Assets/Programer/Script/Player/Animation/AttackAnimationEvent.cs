using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerControl _playerControl;


    public void IsCanNextAttack()
    {
        _playerControl.Attack.IsCanNextAttack = true;
    }



}
