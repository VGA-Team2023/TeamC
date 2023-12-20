using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndWait : MonoBehaviour, IPause
{
    [Header("ゲーム終了時のUICanvas")]
    [SerializeField] private GameObject _endCancas;

    [Header("待機時間")]
    [SerializeField] private float _waitTime = 0;

    private float _countWaitTime = 0;

    private bool _isGameEnd = false;

    private bool _isPause = false;

    private bool _isCall = false;


    private void Update()
    {
        if (_isGameEnd && !_isPause)
        {
            _countWaitTime += Time.deltaTime;

            if (_countWaitTime >= _waitTime)
            {
                WaitEnd();
            }
        }
    }


    /// <summary>ゲーム終了時に呼ぶ </summary>
    public void GameEnd()
    {
        if (_isCall) return;

        _isGameEnd = true;

        _isCall= true;

        //音を鳴らす
        AudioController.Instance.Voice.Play(VoiceState.InstructorGameClear);

        //UIを出す
        _endCancas.SetActive(true);
    }

    /// <summary>待機時間の終了 </summary>
    public void WaitEnd()
    {
        //リザルト状態に変更
        var _gameManager = FindObjectOfType<GameManager>();
        _gameManager.ChangeGameState(GameState.Result);
        //スコアの計算をここに記述
        //シーン遷移のメソッドを呼ぶ
        _gameManager.ResultProcess();
        Loading sceneControlle = FindObjectOfType<Loading>();
        sceneControlle?.LoadingScene();
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void Resume()
    {
        _isPause = false;
    }




}
