using UnityEngine;

[System.Serializable]
/// <summary>ゲームの経過時間を操作するClass</summary>
public class TimeManager : ISlow,IPause,ISpecialMovingPause
{
    /// <summary>時間経過速度の割合(通常１)</summary>
    float _currentTimeSpeedRate = 1;

    [Tooltip("プレイ時間")]
    [SerializeField] float _gamePlayTime = 60;

    float _gamePlayElapsedTime = 0;

    /// <summary>ゲームのプレイ時間</summary>
    public float GamePlayTime => _gamePlayTime;

    /// <summary>ゲーム中の経過時間</summary>
    public float GamePlayElapsedTime => _gamePlayElapsedTime;

    public TimeManager(float time)
    {
        _gamePlayTime = time;
    }

    /// <summary>タイマーのリセットや一時停止などの登録</summary>
    public void Start()
    {
        TimerReset();
        _currentTimeSpeedRate = 1;
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }

    /// <summary>主にタイムの時間を減らす処理を行う関数</summary>
    public void Update()
    {
        _gamePlayElapsedTime += Time.deltaTime * _currentTimeSpeedRate;
    }

    /// <summary>ゲーム時間のリセット</summary>
    public void TimerReset()
    {
        _gamePlayElapsedTime = 0;
    }

    /// <summary>float型のタイマーを分と秒に計算する関数</summary>
    public MinutesSecondsVer MinutesSecondsCast()
    {
        float minutes = _gamePlayElapsedTime / 60;
        float seconds = _gamePlayElapsedTime - Mathf.Floor(minutes) * 60;
        return new MinutesSecondsVer((int)minutes, (int)seconds);
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

    void ISpecialMovingPause.Pause()
    {
        _currentTimeSpeedRate = 0;
    }
    void ISpecialMovingPause.Resume()
    {
        _currentTimeSpeedRate = 1;
    }
}
public struct MinutesSecondsVer
{
    int minutes;
    int seconds;
    /// <summary>分</summary>
    public int Minutes => minutes;
    /// <summary>秒</summary>
    public int Seconds => seconds;
    public MinutesSecondsVer(int m, int s)
    {
        minutes = m;
        seconds = s;
    }
}
