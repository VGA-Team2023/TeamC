using UnityEngine;
using UnityEngine.UI;
public class TimerController : MonoBehaviour
{
    [SerializeField] Image _backImage;

    private void Update()
    {
        _backImage.fillAmount = (GameManager.Instance.TimeManager.GamePlayTime -
            GameManager.Instance.TimeManager.GamePlayElapsedTime)/GameManager.Instance.TimeManager.GamePlayTime;
    }
}
