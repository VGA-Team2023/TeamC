using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>主にスコアの保存をしておくクラス</summary>
[System.Serializable]
public class ScoreManager
{
    MinutesSecondsVer _clearTime = new();

    int _longEnemyDefeatedNum = 0;

    int _shortEnemyDefeatedNum = 0;

    int _playerDownNum = 0;

    bool _isBossDestroy = false;

    /// <summary>クリア時間</summary>
    public MinutesSecondsVer ClearTime { get { return _clearTime; } set { _clearTime = value; } }
    /// <summary>遠距離敵撃破数</summary>
    public int LongEnemyDefeatedNum { get { return _longEnemyDefeatedNum; } set { _longEnemyDefeatedNum = value; } }
    /// <summary>近距離敵撃破数</summary>
    public int ShortEnemyDefeatedNum { get { return _shortEnemyDefeatedNum; } set { _shortEnemyDefeatedNum = value; } }
    /// <summary>Playerのダウン数</summary>
    public int PlayerDownNum { get { return _playerDownNum; } set { _playerDownNum = value; } }
    /// <summary>ボス敵を倒したかどうか</summary>
    public bool IsBossDestroy { get => _isBossDestroy; set => _isBossDestroy = value; }

    /// <summary>スコアのリセット</summary>
    public void ScoreReset()
    {
        _isBossDestroy = false;
        _clearTime = new();
        _longEnemyDefeatedNum = 0;
        _shortEnemyDefeatedNum = 0;
        _playerDownNum = 0;
    }
}
