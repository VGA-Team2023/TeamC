using UnityEngine;
using UnityEngine.UI;
public class TimerController : MonoBehaviour, IPause
{
    [SerializeField] Image _backImage;
    [SerializeField] Image[] _stars;
    [SerializeField] GameObject _needleObject;
    private GameManager _gm;
    private bool _isPausing = false;
    private bool _isCounting = true;
    private int _targetIndex = 0;
    private float[] _thresholds = { 0.99f, 0.8f, 0.6f, 0.4f, 0.2f};
    private void Update()
    {
        if (!_isPausing)
        {
            float fillAmout = (GameManager.Instance.TimeManager.GamePlayTime -
            GameManager.Instance.TimeManager.GamePlayElapsedTime) / GameManager.Instance.TimeManager.GamePlayTime;
            _backImage.fillAmount = fillAmout;
            _needleObject.transform.rotation = Quaternion.Euler(0, 0, fillAmout * 360);
            if (_isCounting && fillAmout < _thresholds[_targetIndex])
            {
                SetStarsActive(_targetIndex);
            }
        }
    }
    private void SetStarsActive(int targetIndex)
    {
        for (int j = 0; j <= targetIndex; j++)
        {
            _stars[j].gameObject.SetActive(false);
        }
        for (int j = targetIndex + 1; j < _stars.Length; j++)
        {
            _stars[j].gameObject.SetActive(true);
        }
        _targetIndex += 1;
        if (_targetIndex == _thresholds.Length)
        {
            _isCounting = false;
        }
    }
    public void Pause()
    {
        _isPausing = true;
    }
    public void Resume()
    {
        _isPausing = false;
    }
    private void OnEnable()
    {
        _gm = GameManager.Instance;
        _gm.PauseManager.Add(this);
    }
    private void OnDisable()
    {
        _gm.PauseManager.Remove(this);
    }
}
