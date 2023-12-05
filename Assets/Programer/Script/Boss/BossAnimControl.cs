using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAnimControl
{
    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }


    public void IsDown(bool isDown)
    {
        _bossControl.Animator.SetBool("IsDown", isDown);
    }


    public void IsBlend(bool isBlend)
    {
        _bossControl.Animator.SetBool("IsBlend", isBlend);
    }

    public void SetMoveH(float h)
    {
        _bossControl.Animator.SetFloat("MoveH", h);
    }

}

