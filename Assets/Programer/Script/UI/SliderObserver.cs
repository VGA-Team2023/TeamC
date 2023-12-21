using UnityEngine;
using UnityEngine.UI;

public class SliderObserver : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private  AudioController _audioController;
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
}
