using UnityEngine;
using UnityEngine.UI;
public class LoadingPanel : MonoBehaviour
{
    [SerializeField] GameObject _rotatateObj;
    [SerializeField] Text _tips;
    [SerializeField] string[] _tipsData;
    [SerializeField] TipData[] _tipDatas;
    [SerializeField] Text _teacherText;
    [SerializeField] Text _magicText;
    [SerializeField] Text _academyText;
    [SerializeField] Text _operationInstructionsText;
    [SerializeField] Text _mainCharactorText;
    private int _totalRotate = 0;
    private static LoadingPanel _instance = null;
    public static LoadingPanel Instance
    {
        get { return _instance; }
    }
    private void OnEnable()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);

        PrintTips();
    }
    private void Update()
    {
        _totalRotate = (_totalRotate + 1) % 360;
        _rotatateObj.transform.rotation = Quaternion.Euler(0, 0, _totalRotate);
    }
    public void PrintTips()
    {
        _mainCharactorText.gameObject.SetActive(false);
        _operationInstructionsText.gameObject.SetActive(false);
        _academyText.gameObject.SetActive(false);
        _magicText.gameObject.SetActive(false);
        _teacherText.gameObject.SetActive(false);

        int random = Random.Range(0, _tipDatas.Length);
        _tips.text = _tipDatas[random].TipText;

        if (_tipDatas[random].Type == TipData.TipsType.Teacher) _teacherText.gameObject.SetActive(true);
        else if (_tipDatas[random].Type == TipData.TipsType.MainCharacter) _mainCharactorText.gameObject.SetActive(true);
        else if (_tipDatas[random].Type == TipData.TipsType.OperationInstructions) _operationInstructionsText.gameObject.SetActive(true);
        else if (_tipDatas[random].Type == TipData.TipsType.Academy) _academyText.gameObject.SetActive(true);
        else if (_tipDatas[random].Type == TipData.TipsType.Magic) _magicText.gameObject.SetActive(true);
    }
}
