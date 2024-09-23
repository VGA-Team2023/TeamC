using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    /// <summary>現在のゲームの状態</summary>
    [Header("現在のシーン")]
    [SerializeField] GameState _currentGameState;

    [SerializeField, HideInInspector]
    SlowManager _slowManager;

    [SerializeField, HideInInspector]
    TimeManager _timeManager;

    ScoreManager _scoreManager = new ScoreManager();

    PauseManager _pauseManager = new PauseManager();

    SpecialMovingPauseManager _specialPauseManager = new SpecialMovingPauseManager();

    MinutesSecondsVer _clearTime;

    bool _isGameMove = false;

    /// <summary>Playerの属性</summary>
    PlayerAttribute _playerAttribute = PlayerAttribute.Ice;

    public PlayerAttribute PlayerAttribute => _playerAttribute;
    public SlowManager SlowManager => _slowManager;
    public PauseManager PauseManager => _pauseManager;
    public TimeManager TimeManager => _timeManager;
    public ScoreManager ScoreManager => _scoreManager;
    public SpecialMovingPauseManager SpecialMovingPauseManager => _specialPauseManager;

    /// <summary>クリア時間</summary>
    public MinutesSecondsVer ClearTime => _clearTime;

    /// <summary>InGameで戦闘中かどうか</summary>
    public bool IsGameMove => _isGameMove;

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
        //何もなかったら
        if (_instance == null)
        {
            //ゲームシーンだったら
            if (_currentGameState == GameState.Game)
            {
                InGameStart();
            }

            //自分を入れる
            _instance = this;

            _timeManager.Start();
            ChangeBGMState(_instance._currentGameState);
            DontDestroyOnLoad(this);

        }
        //先に読み取りが発生した時
        else if (_instance == this)
        {
            if (_currentGameState == GameState.Game)
            {
                //InGameStart();
            }

            // _timeManager.Start();
            ChangeBGMState(_instance._currentGameState);

            DontDestroyOnLoad(this);
        }
        //すでにある場合
        else
        {
            //自分のゲーム状態とInstanceが持つ状態が違ったら
            if (_instance._currentGameState != this._currentGameState)
            {
                //自分のゲーム状態をInstanceに上書き
                _instance.ChangeGameState(this._currentGameState);
                //音変更
                _instance.ChangeBGMState(this._currentGameState);
            }
            //二回目以降のゲームシーンに遷移したら
            if (_currentGameState == GameState.Game)
            {
                //タイマーリセット
                _instance._timeManager.TimerReset();
                _instance._scoreManager.ScoreReset();
                //_instance.InGameStart();
            }
            Destroy(this);
        }
    }

    private void Update()
    {
        Debug.Log("G" + _isGameMove);
        //インゲーム中だったら
        if (_currentGameState == GameState.Game)
        {
            _timeManager.Update();
            //インゲームが終わったら
            if (_timeManager.GamePlayElapsedTime >= _timeManager.GamePlayTime)
            {
                GameEndWaitCall();
            }
        }
    }

    public void SEStopAll()
    {
        AudioController.Instance.SE.StopAll();
    }

    public void BGMStop()
    {
        AudioController.Instance.BGM.Stop();
    }

    public void VoiceStopAll()
    {
        AudioController.Instance.Voice.StopAll();
    }


    /// <summary>InGame中ゲーム終了時直後に呼ぶメソッド</summary>
    public void GameEndWaitCall()
    {
        _isGameMove = false;
        GameEndWait gameEndWait = FindObjectOfType<GameEndWait>();
        gameEndWait.GameEnd();
    }

    /// <summary>ゲーム中判定にする</summary>
    public void InGameStart()
    {
        //相羽書き換え



        _isGameMove = true;
        _timeManager.Start();
    }

    /// <summary>クリアタイム保存</summary>
    public void ResultProcess()
    {
        _scoreManager.ClearTime = _timeManager.MinutesSecondsCast();
    }

    /// <summary>Playerの属性を変える処理を行うメソッド</summary>
    /// <param name="isEnumNumber">変えたい属性</param>
    public void PlayerAttributeChange(PlayerAttribute attributes)
    {
        _playerAttribute = attributes;
    }

    /// <summary>現在のゲームの状態を変える処理をおこなう</summary>
    /// <param name="changeGameState">変えたい状態のこと</param>
    public void ChangeGameState(GameState changeGameState)
    {
        _currentGameState = changeGameState;
    }

    /// <summary>現在のゲームの状態に対してBGMを変える処理</summary>
    /// <param name="state"></param>
    public void ChangeBGMState(GameState state)
    {
        if (state == GameState.Break) { return; }
        switch (state)
        {
            case GameState.Title:
                AudioController.Instance.BGM.Play(BGMState.Title); break;
            case GameState.Tutorial:
                AudioController.Instance.BGM.Play(BGMState.Tutorial); break;
            case GameState.Game:
                AudioController.Instance.BGM.Play(BGMState.Battle); break;
            case GameState.Result:
                AudioController.Instance.BGM.Play(BGMState.Result); break;
        }
    }
}

/// <summary>全体のゲームの状態を管理するenum</summary>
public enum GameState
{
    /// <summary>タイトル</summary>
    Title,
    /// <summary>チュートリアル</summary>
    Tutorial,
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
    /// <summary>草属性</summary>
    Grass
}
