using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    /// <summary>現在のゲームの状態</summary>
    [SerializeField] GameState _currentGameState;
    [SerializeField]TimeControl _timeControl;
    [SerializeField] SlowManager _slowManager;
    [SerializeField] TimeManager _timeManager;
    ScoreManager _scoreManager = new ScoreManager();
    PauseManager _pauseManager = new PauseManager();
    /// <summary>スコア格納用変数</summary>
    public static int _score = 0;
    /// <summary>Playerの属性</summary>
    PlayerAttribute _playerAttribute = PlayerAttribute.Ice;
    public PlayerAttribute PlayerAttribute => _playerAttribute;
    public TimeControl TimeControl => _timeControl;
    public SlowManager SlowManager => _slowManager;
    public PauseManager PauseManager => _pauseManager;
    public TimeManager TimeManager => _timeManager;
    public static GameManager Instance
    {
        //読み取り時
        get
        {
            //instanceがnullだったら
            if (!_instance)
            {
                //シーン内のGameobjectにアタッチされているTを取得
                _instance = FindObjectOfType<GameManager>();
                //アタッチされていなかったら
                if (!_instance)
                {
                    //エラーを出す
                    Debug.LogError("Scene内に" + typeof(GameManager).Name + "をアタッチしているGameObjectがありません");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _timeManager.Start();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        //インゲーム中だったら
        if(_currentGameState == GameState.Game)
        {
            _timeManager.Update();
            //インゲームが終わったら
            if(_timeManager.GamePlayElapsedTime < 0)
            {
                //リザルトに行く
                ChangeGameState(GameState.Result);
                //ここにシーン遷移のメソッドを呼ぶ
            }
        }
    }

    /// <summary>スコアのリセット</summary>
    void ScoreReset()
    {
        _score = 0;
    }

    void TimerReset()
    {
        _timeManager.TimerReset();
    }
    /// <summary>Playerの属性を保存する処理を行うメソッド</summary>
    /// <param name="isIce">氷属性かどうか</param>
    public void PlayerAttributeSelect(bool isIce)
    {
        if(isIce)
        {
            _playerAttribute = PlayerAttribute.Ice;
        }
        else
        {
            _playerAttribute = PlayerAttribute.Wood;
        }
    }

    /// <summary>現在のゲームの状態を変える処理をおこなう</summary>
    /// <param name="changeGameState">変えたい状態のこと</param>
    public void ChangeGameState(GameState changeGameState)
    {
        _currentGameState = changeGameState;
        
    }

    public void Result()
    {
        _score = _scoreManager.ScoreCaster(_timeManager.GamePlayElapsedTime, 10);
    }
}
/// <summary>全体のゲームの状態を管理するenum</summary>
public enum GameState
{
    /// <summary>タイトル</summary>
    Title,
    /// <summary>ゲーム中</summary>
    Game,
    /// <summary>敵生成中</summary>
    Break,
    /// <summary>リザルト</summary>
    Result
}

public enum PlayerAttribute
{
    /// <summary>氷属性</summary>
    Ice,
    /// <summary>木属性</summary>
    Wood
}
