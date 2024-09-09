using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTimeUITemplate : MonoBehaviour
{
    [SerializeField] Text _minutes;
    [SerializeField] Text _seconds;
    [SerializeField] Text _enemyDefeatedNum;
    [SerializeField] Text _playerDownNum;
    // Start is called before the first frame update
    void Start()
    {
        //クリア時間(分)
        _minutes.text = $"{GameManager.Instance.ScoreManager.ClearTime.Minutes}分";
        //クリア時間(秒)
        _seconds.text = $"{GameManager.Instance.ScoreManager.ClearTime.Seconds}秒";
        //敵撃破数
        _enemyDefeatedNum.text = $"{GameManager.Instance.ScoreManager.LongEnemyDefeatedNum}体";
        //プレイヤーダウン数
        _playerDownNum.text = $"{GameManager.Instance.ScoreManager.PlayerDownNum}体";
    }
}
