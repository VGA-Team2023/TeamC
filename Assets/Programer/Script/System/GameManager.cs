using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    /// <summary>現在のゲームの状態</summary>
    [SerializeField, Header("現在のシーン")] GameState _currentGameState;
    [SerializeField, HideInInspector] TimeControl _timeControl;
    [SerializeField, HideInInspector] SlowManager _slowManager;
    [SerializeField, HideInInspector] TimeManager _timeManager;
    ScoreManager _scoreManager = new ScoreManager();
    PauseManager _pauseManager = new PauseManager();
    SpecialMovingPauseManager _specialPauseManager = new SpecialMovingPauseManager();
    MinutesSecondsVer _clearTime;
    /// <summary>Playerの属性</summary>
    PlayerAttribute _playerAttribute = PlayerAttribute.Ice;
    public PlayerAttribute PlayerAttribute => _playerAttribute;
    public TimeControl TimeControl => _timeControl;
    public SlowManager SlowManager => _slowManager;
    public PauseManager PauseManager => _pauseManager;
    public TimeManager TimeManager => _timeManager;
    public ScoreManager ScoreManager => _scoreManager;
    public SpecialMovingPauseManager SpecialMovingPauseManager => _specialPauseManager;
    /// <summary>クリア時間</summary>
    public MinutesSecondsVer ClearTime => _clearTime;
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
            _instance = this;
            _timeManager.Start();
            ChangeBGMState(_instance._currentGameState);
            DontDestroyOnLoad(this);
            Debug.Log("あ");
        }
        //先に読み取りが発生した時
        else if(_instance == this)
        {
            _timeManager.Start();
            ChangeBGMState(_instance._currentGameState);
            DontDestroyOnLoad(this);
        }
        //すでにある場合
        else if (_instance != null)
        {
            if (_instance._currentGameState != this._currentGameState)
            {
                _instance.ChangeGameState(this._currentGameState);
                _instance.ChangeBGMState(this._currentGameState);
            }
            //二回目以降のゲームシーンに遷移したら
            if (_currentGameState == GameState.Game)
            {
                //タイマーリセット
                _instance._timeManager.TimerReset();
                _instance._scoreManager.ScoreReset();
            }
            Debug.Log("あ");
            Destroy(this);
        }
    }

    private void Update()
    {
        //インゲーム中だったら
        if (_currentGameState == GameState.Game)
        {
            _timeManager.Update();
            //インゲームが終わったら
            if (_timeManager.GamePlayElapsedTime >= _timeManager.GamePlayTime)
            {
                ResultProcess();
                Loading loading = FindObjectOfType<Loading>();
                loading.LoadingScene();
            }
        }
    }

    /// <summary>リザルトシーン遷移処理</summary>
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
