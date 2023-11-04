using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>ゲームの経過時間を操作するClass</summary>
public class TimeManager : ISlow,IPause
{
    float _currentTimeSpeedRate = 1;
    /// <summary>ゲームのプレイ時間</summary>
    [SerializeField] float _gamePlayTime = 60;
    /// <summary>ゲーム中の経過時間</summary>
    float _gamePlayElapsedTime = 0;
    public float GamePlayElapsedTime => _gamePlayElapsedTime;
    public void Start()
    {
        TimerReset();
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
    }
    /// <summary>主にタイムの時間を減らす処理を行う関数</summary>
    public void Update()
    {
        _gamePlayElapsedTime -= Time.deltaTime * _currentTimeSpeedRate;
    }
    /// <summary>ゲーム時間のリセット</summary>
    public void TimerReset()
    {
        _gamePlayElapsedTime = _gamePlayTime;
    }

    void IPause.Pause()
    {
        _currentTimeSpeedRate = 0;
    }

    void IPause.Resume()
    {
        _currentTimeSpeedRate = 1;
    }

    void ISlow.OffSlow() 
    {
        _currentTimeSpeedRate = 1;
    }

    void ISlow.OnSlow(float slowSpeedRate)
    {
        _currentTimeSpeedRate = slowSpeedRate;
    }
}
