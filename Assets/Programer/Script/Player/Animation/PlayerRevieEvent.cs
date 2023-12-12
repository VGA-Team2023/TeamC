using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevieEvent : MonoBehaviour
{
    [SerializeField] private PlayerControl _playerControl;

    public void Revive()
    {
        GameManager.Instance.SpecialMovingPauseManager.PauseResume(false);
        _playerControl.PlayerHp.ReVive();
    }

}
