using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderObserver : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _defaltColor;
    private AudioController _audioController;
    private Text _text = null;
    private void OnEnable()
    {
        if (_text == null)
        {
            _text = GetComponentInChildren<Text>();
        }
    }

    public void SetSEValue()
    {
        if (_audioController == null) _audioController = AudioController.Instance;
        _audioController.SetVolume(_slider.value, VolumeChangeType.SE);
    }
    public void SetBGMValue()
    {
        if (_audioController == null) _audioController = AudioController.Instance;
        _audioController.SetVolume(_slider.value, VolumeChangeType.BGM);
    }
    public void SetVoiceValue()
    {
        if (_audioController == null) _audioController = AudioController.Instance;
        _audioController.SetVolume(_slider.value, VolumeChangeType.Voice);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _text.color = _selectColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _text.color = _defaltColor;
    }
}
