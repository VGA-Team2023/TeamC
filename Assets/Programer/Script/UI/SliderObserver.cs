using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderObserver : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _defaltColor;
    [SerializeField] private Text _text = null;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Sprite _buttonsSelectedSprite;
    [SerializeField] private Sprite _buttonsDeSelectedSprite;
    [SerializeField] private Text _buttonText;
    private AudioController _audioController;   
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
        _buttonText.color = _selectColor;
        _menuButton.image.sprite = _buttonsSelectedSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _text.color = _defaltColor;
        _menuButton.image.sprite = _buttonsDeSelectedSprite;
    }
}
