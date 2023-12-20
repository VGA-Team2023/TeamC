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
    private GetNumber nums;
    private void Start()
    {
        nums = new GetNumber();
    }
    private void LateUpdate()
    {
        if (_shortEnemyDefeatCount != nums.GetMeleeNum || _longEnemyDefeatCount!= nums.GetLongDef)
        {
            _shortEnemyDefeatCount = nums.GetMeleeNum;
            _longEnemyDefeatCount = nums.GetLongDef;
            ChanageDisplayEnemyDefeatCount();
        }       
    }
    private void OnValidate()
    {
        ChanageDisplayEnemyDefeatCount();
    }
    private void ChanageDisplayEnemyDefeatCount()
    {
        _meleeEnemyDefeatCountText.text = _shortEnemyDefeatCount.ToString() + "/"+_allMeleeEnemyCount;
        _longEnemyDefeatCountText.text = _longEnemyDefeatCount.ToString()+"/"+_allLongEnemyCount;
    }
}
