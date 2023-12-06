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
        //時間を遅くする
       // GameManager.Instance.SlowManager.OnOffSlow(false);
    }

}
