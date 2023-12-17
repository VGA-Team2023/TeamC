using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class OptionPanel : MonoBehaviour, IPause
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _voiceSlider;
    [SerializeField] private Slider _seSlider;
    [SerializeField] private Slider _cameraSensitivitySlider;
    [SerializeField] private Button _closeButton;
    private EventSystem _eventSystem = null;
    private AudioController _audioController = null;
    private void OnEnable()
    {
        if (_eventSystem == null)
        {
            _eventSystem = FindObjectOfType<EventSystem>();
        }
        if (_audioController == null)
        {
            _audioController = AudioController.Instance;
        }
        if (_eventSystem != null)
        {
            _eventSystem.SetSelectedGameObject(_cameraSensitivitySlider.gameObject);
        }
        _cameraSensitivitySlider.GetComponent<DisplayTargetPointer>()?.TargetImage.gameObject.SetActive(true);
        _bgmSlider.value = AudioController.Instance.GetVolume(VolumeChangeType.BGM);
        _voiceSlider.value = AudioController.Instance.GetVolume(VolumeChangeType.Voice);
        _seSlider.value = AudioController.Instance.GetVolume(VolumeChangeType.SE);
        _cameraSensitivitySlider.value = OptionValueRecorder.Instance.CameraSensitivity;
    }
    private void OnDisable()
    {
        if (_audioController != null)
        {
            _audioController.SetVolume(_bgmSlider.value, VolumeChangeType.BGM);
            _audioController.SetVolume(_voiceSlider.value, VolumeChangeType.Voice);
            _audioController.SetVolume(_seSlider.value, VolumeChangeType.SE);
        }
        if (_eventSystem != null) _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
        if (OptionValueRecorder.Instance != null) OptionValueRecorder.Instance.CameraSensitivity = _cameraSensitivitySlider.value;
        if (_closeButton !=null) _closeButton.GetComponent<ButtonTextColorChanger>().Target.gameObject.SetActive(false);
    }
    public void Pause()
    {
        if (this != null) this.gameObject.SetActive(true);
    }
    public void Resume()
    {
        if (this != null) this.gameObject.SetActive(false);
    }
}
