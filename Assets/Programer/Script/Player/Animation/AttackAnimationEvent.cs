using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerControl _playerControl;


    public void IsCanNextAttack()
    {
        if (_playerControl.IsNewAttack)
        {
           // _playerControl.Attack2.IsCanNextAttack = true;
        }
        else
        {
            _playerControl.Attack.IsCanNextAttack = true;
        }
    }

    public void CanAttack()
    {
        Debug.Log("REEE");
        _playerControl.Attack2.ResetAttack();
        IsCanNextAttack();
    }



}
