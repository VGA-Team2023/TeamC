using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class OptionPanel : MonoBehaviour,IPause
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _voiceSlider;
    [SerializeField] private Slider _seSlider;
    [SerializeField] private Slider _cameraSensitivitySlider;
    [SerializeField] private Button _closeButton;
    private EventSystem _eventSystem = null;
    private void OnEnable()
    {
        if (_eventSystem == null)
        {
            _eventSystem = FindObjectOfType<EventSystem>();
        }
        _eventSystem.SetSelectedGameObject(_cameraSensitivitySlider.gameObject);
        _bgmSlider.value = AudioController.Instance.GetVolume(VolumeChangeType.BGM);
        _voiceSlider.value = AudioController.Instance.GetVolume(VolumeChangeType.Voice);
        _seSlider.value = AudioController.Instance.GetVolume(VolumeChangeType.SE);
        _cameraSensitivitySlider.value = OptionValueRecorder.Instance.CameraSensitivity;
    }
    private void OnDisable()
    {
        AudioController.Instance.SetVolume(_bgmSlider.value,VolumeChangeType.BGM);
        AudioController.Instance.SetVolume(_voiceSlider.value,VolumeChangeType.Voice);
        AudioController.Instance.SetVolume(_seSlider.value, VolumeChangeType.SE);
        OptionValueRecorder.Instance.CameraSensitivity = _cameraSensitivitySlider.value;
        _closeButton.GetComponent<ButtonTextColorChanger>()._target.gameObject.SetActive(false);
        _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
    }
    public void ClosePanel()
    {
        GameManager.Instance.PauseManager.PauseResume(!GameManager.Instance.PauseManager.IsPause);
    }
    public void Pause()
    {
        this.gameObject.SetActive(true);
    }
    public void Resume()
    {
        this.gameObject.SetActive(false);
    }
}
