using UnityEngine;
using UnityEngine.UI;
public class DisplayEnemyDefeatCount : MonoBehaviour
{
    [SerializeField, Tooltip("エネミーの撃破数を表示するテキスト")]
    Text _enemyDefeatCountText;
    private int _enemyDefeatCount
    {
        get => GameManager.Instance.ScoreManager.EnemyDefeatedNum;
        set
        {
            _enemyDefeatCount = value;
        }
    }
    private void OnValidate()
    {
        //ChanageDisplayEnemyDefeatCount();
    }
    private void ChanageDisplayEnemyDefeatCount()
    {
       // _enemyDefeatCountText.text = _enemyDefeatCount.ToString();
    }
}
