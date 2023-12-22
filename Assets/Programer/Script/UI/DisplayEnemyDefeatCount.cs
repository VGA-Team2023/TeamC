using UnityEngine;
using UnityEngine.UI;

public class DisplayEnemyDefeatCount : MonoBehaviour
{
    [SerializeField, Tooltip("近距離エネミーの撃破数を表示するテキスト")]
    private Text _meleeEnemyDefeatCountText;
    [SerializeField, Tooltip("遠距離エネミーの撃破数を表示するテキスト")]
    private Text _longEnemyDefeatCountText;
    [SerializeField, Tooltip("近距離敵の全体数")]
    private int _allMeleeEnemyCount = 10;
    [SerializeField, Tooltip("遠距離敵の全体数")]
    private int _allLongEnemyCount = 10;
    [SerializeField]
    private int _shortEnemyDefeatCount;
    [SerializeField]
    private int _longEnemyDefeatCount;
    private void LateUpdate()
    {
        if (_shortEnemyDefeatCount != GameManager.Instance.ScoreManager.ShortEnemyDefeatedNum 
            || _longEnemyDefeatCount != GameManager.Instance.ScoreManager.LongEnemyDefeatedNum)
        {
            _shortEnemyDefeatCount = GameManager.Instance.ScoreManager.ShortEnemyDefeatedNum;
            _longEnemyDefeatCount = GameManager.Instance.ScoreManager.LongEnemyDefeatedNum;
            ChanageDisplayEnemyDefeatCount();
        }
    }
    private void ChanageDisplayEnemyDefeatCount()
    {
        _meleeEnemyDefeatCountText.text = _shortEnemyDefeatCount.ToString() + "/"+_allMeleeEnemyCount;
        _longEnemyDefeatCountText.text = _longEnemyDefeatCount.ToString()+"/"+_allLongEnemyCount;
    }
}
