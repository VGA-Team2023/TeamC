using UnityEngine;
using UnityEngine.UI;
public class GetNumber
{
    private int meleeDef;
    private int longDef;

    public int GetMeleeNum
    {
        get { return GameManager.Instance.ScoreManager.ShortEnemyDefeatedNum; }
        set { meleeDef = value; }
    }
    public int GetLongDef
    {
        get { return GameManager.Instance.ScoreManager.LongEnemyDefeatedNum; }
        set { longDef = value; }
    }
}   

public class DisplayEnemyDefeatCount : MonoBehaviour
{
    [SerializeField, Tooltip("近距離エネミーの撃破数を表示するテキスト")]
    Text _meleeEnemyDefeatCountText;
    [SerializeField, Tooltip("遠距離エネミーの撃破数を表示するテキスト")]
    Text _longEnemyDefeatCountText;
    [SerializeField]
    private int _shortEnemyDefeatCount;
    [SerializeField]
    private int _longEnemyDefeatCount;
    private GetNumber nums;
    private void Start()
    {
        nums = new GetNumber();
    }
    private void LateUpdate()
    {
        _shortEnemyDefeatCount = nums.GetMeleeNum;
        _longEnemyDefeatCount = nums.GetLongDef;
        ChanageDisplayEnemyDefeatCount();
    }
    private void OnValidate()
    {
        ChanageDisplayEnemyDefeatCount();
    }
    private void ChanageDisplayEnemyDefeatCount()
    {
        _meleeEnemyDefeatCountText.text = _shortEnemyDefeatCount.ToString();
        _longEnemyDefeatCountText.text = _longEnemyDefeatCount.ToString();
    }
}
