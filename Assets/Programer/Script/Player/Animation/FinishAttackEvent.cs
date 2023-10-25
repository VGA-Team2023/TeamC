using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAttackEvent : MonoBehaviour
{
    [SerializeField] private PlayerControl _playerControl;


    public void EndFinishAttack()
    {
        _playerControl.FinishingAttack.EndFinishAnim();
    }

    public void ResetTime()
    {
        //ŽžŠÔ‚ð’x‚­‚·‚é
        GameManager.Instance.TimeControl.SetTimeScale(1f);
    }

}
