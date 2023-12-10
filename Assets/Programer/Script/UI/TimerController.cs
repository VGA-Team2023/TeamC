using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
public class TimerController : MonoBehaviour
{
    [SerializeField] Image _backImage;
    private float _timer;
    private float _changeForSecond;

    private void Start()
    {
        //_changeForSecond = 1.0f / GameManager.Instance.TimeManager.GamePlayTime;
    }

    private void Update()
    {
        _backImage.fillAmount = (GameManager.Instance.TimeManager.GamePlayTime -
            GameManager.Instance.TimeManager.GamePlayElapsedTime)/GameManager.Instance.TimeManager.GamePlayTime;
    }
}
