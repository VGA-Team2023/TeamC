using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackTitlePauseOffer : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.PauseManager.IsPause)
        {
            GameManager.Instance.PauseManager.PauseResume(false);
        }

    }
}
