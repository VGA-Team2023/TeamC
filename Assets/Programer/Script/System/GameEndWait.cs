using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndWait : MonoBehaviour
{

    /// <summary>ゲーム終了時に呼ぶ </summary>
    public void GameEnd()
    {

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

}
