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
    public static LoadingPanel _instance = null;
    private int _leftInd = 0;
    private int _rightInd = 0;
    private int _totalRotate = 0;
    private void OnEnable()
    {      
        if (FindObjectsOfType<LoadingPanel>().Length < 2)
        {
            if (_instance == null)
            {
                _instance = this;               
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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
        _totalRotate += 1;
        _rotatateObj.transform.rotation = Quaternion.Euler(0, 0, _totalRotate);
    }
    public void PrintTips()
    {
        int random = Random.Range(0, _tipsData.Length);
        _tips.text = _tipsData[random];
    }
}
