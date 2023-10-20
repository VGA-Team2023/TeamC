using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("スクリプトコンポーネント")]
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] TimeManager _timeManager;
    /// <summary>スコア格納用変数</summary>
    public static int _score = 0;
    /// <summary>現在のゲームの状態</summary>
    GameState _currentGameState = GameState.Game;
    private void Awake()
    {
        ScoreReset();
    }

    /// <summary>スコアをリセット処理を行う</summary>
    void ScoreReset()
    {
        _score = 0;
    }

    /// <summary>現在のゲームの状態を変える処理をおこなう</summary>
    /// <param name="changeGameState">変えたい状態のこと</param>
    public void ChangeGameState(GameState changeGameState)
    {
        _currentGameState = changeGameState;
        switch (_currentGameState)
        {
            //ゲームクリアだったら
            case GameState.GameClear:
                _score = _scoreManager.ScoreCaster(_timeManager.ElapsedTime, 10);
                break;
            //ゲームオーバーだったら
            case GameState.GameOver:
                break;
        }
    }
}
/// <summary>全体のゲームの状態を管理するenum</summary>
public enum GameState
{
    /// <summary>ゲーム中</summary>
    Game,
    /// <summary>ゲームオーバー</summary>
    GameOver,
    /// <summary>ゲームクリア</summary>
    GameClear,
}
