using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeControl 
{

    private GameManager _gameManager;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void ResetTimScale()
    {
        Time.timeScale = 1;
    }

}
