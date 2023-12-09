using UnityEngine;
using UnityEngine.UI;
public class LoadingPanel : MonoBehaviour
{
    [SerializeField] Image[] _images;
    [SerializeField] Image _leftImage;
    [SerializeField] Image _rightImage;
    [SerializeField] GameObject _rotatateObj;
    [SerializeField] Text _tips;
    [SerializeField] string[] _tipsData;
    private int _leftInd = 0;
    private int _rightInd = 0;
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

        _leftInd = Random.Range(0, _images.Length);

        if (_images.Length > 1)
        {
            while (_rightInd == _leftInd)
            {
                _rightInd = Random.Range(0, _images.Length);
            }
        }

        _leftImage.sprite = _images[_leftInd].sprite;
        _rightImage.sprite = _images[_rightInd].sprite;

        PrintTips();
    }
    private void Update()
    {
        _totalRotate = (_totalRotate + 1) % 360;
        _rotatateObj.transform.rotation = Quaternion.Euler(0, 0, _totalRotate);
    }
    public void PrintTips()
    {
        int random = Random.Range(0, _tipsData.Length);
        _tips.text = _tipsData[random];
    }
}
