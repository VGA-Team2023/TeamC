using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidAnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerControl _playerControl;


    public void StartAvoid()
    {
        _playerControl.Avoid.StartAvoidAnim();
    }

    public void EndAvoid()
    {
        _playerControl.Avoid.EndAvoidAnim();
    }

}