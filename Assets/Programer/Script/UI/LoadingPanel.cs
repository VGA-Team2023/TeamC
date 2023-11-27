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
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            Destroy(gameObject);
        }

        _leftInd = Random.Range(0, _images.Length);
        if (_images.Length > 1)
        {
            while (_rightInd == _leftInd)
            {
                _rightInd = Random.Range(0, _images.Length);
            }
        }
        _leftImage = _images[_leftInd];
        _rightImage = _images[_rightInd];
        //_leftImage.color = _images[_leftInd].color;
        //_rightImage.color = _images[_rightInd].color;
    }
    private void OnDisable()
    {

    }
    private void Update()
    {
        _totalRotate += 1;
        _rotatateObj.transform.rotation = Quaternion.Euler(0, 0, _totalRotate);
    }
    public void PrintString()
    {
        int random = Random.Range(0, _tipsData.Length);
        _tips.text = _tipsData[random];
    }
}
