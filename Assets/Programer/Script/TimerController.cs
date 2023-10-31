using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
public class TimerController : MonoBehaviour
{
    [SerializeField] Image _upperImage;
    [SerializeField] Image _midImage;
    [SerializeField] Text _countText;
    [SerializeField] GameObject _timerObject;
    [SerializeField] float _rotateTimer;
    private Image _tmpUpperImage;
    private Image _tmpMidImage;
    private float _timer;
    private float _changeForSecond;
    private float _totalRotation = 0f;
    private int _rotateCount = 0;

    private void Start()
    {
        _midImage.fillAmount = 0;
        _upperImage.fillAmount = 1;
        _changeForSecond = 1.0f / _rotateTimer;
        _tmpUpperImage = _upperImage;
        _tmpMidImage = _midImage;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 1.0f)
        {
            _timer = 0;
            _tmpUpperImage.fillAmount -= _changeForSecond;
            _tmpMidImage.fillAmount += _changeForSecond;
            if (_tmpUpperImage.fillAmount == 0)
            {
                RotateTimer(_tmpUpperImage, _tmpMidImage);
            }
        }
    }

    private void RotateTimer(Image upper, Image mid)
    {
        Debug.Log("a");
        _rotateCount += 1;
        if (_countText) _countText.text = _rotateCount.ToString();

        _totalRotation += 180f;
        _timerObject.transform.rotation = Quaternion.Euler(0, 0, _totalRotation);
        //_timerObject.transform.DOrotate(new Vector3(0, 0, 1) * 180, 1.0f);

        _tmpUpperImage = mid;
        _tmpMidImage = upper;

        _ = _tmpUpperImage.fillOrigin == 1 ? _tmpUpperImage.fillOrigin = 0 : _tmpUpperImage.fillOrigin = 1;
        _ = _tmpMidImage.fillOrigin == 0 ? _tmpMidImage.fillOrigin = 1 : _tmpMidImage.fillOrigin = 0;
    }
}
