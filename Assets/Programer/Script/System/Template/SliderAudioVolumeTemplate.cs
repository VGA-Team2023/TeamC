using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using UnityEngine.UI;

public class UISliderAudioVolumeTemplate : MonoBehaviour
{
    AudioController _audioController;
    [SerializeField, Tooltip("Option用Canvas")] GameObject _optionCanvas;
    [SerializeField, Tooltip("BGMのSlider")] Slider _bgmSlider;
    [SerializeField, Tooltip("SEのSlider")] Slider _seSlider;
    [SerializeField, Tooltip("VoiceのSlider")] Slider _voiceSlider;
    [SerializeField, Tooltip("MasterのSlider")] Slider _masterSlider;

    public void Awake()
    {
        _audioController = AudioController.Instance;
        //最初に値を取得して、Sliderの値に代入
        //BGM参照
        _bgmSlider.value = _audioController.GetVolume(VolumeChangeType.BGM);
        //SE参照
        _seSlider.value = _audioController.GetVolume(VolumeChangeType.SE);
        //Voice参照
        _voiceSlider.value = _audioController.GetVolume(VolumeChangeType.Voice);
        //全体の音量参照
        _masterSlider.value = _audioController.GetVolume(VolumeChangeType.Master);
    }
    private void Start()
    {
        _bgmSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleBGM);
        _seSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleSE);
        _voiceSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleVoice);
        _masterSlider.onValueChanged.AddListener(OnChangeValueToAudioControlleMaster);
    }
   　
    void OnChangeValueToAudioControlleBGM(float value)
    {
        //BGMの音を変える
        _audioController.SetVolume(value, VolumeChangeType.BGM);
    }

    void OnChangeValueToAudioControlleSE(float value)
    {
        _audioController.SetVolume(value, VolumeChangeType.SE);
    }
    void OnChangeValueToAudioControlleVoice(float value)
    {
        _audioController.SetVolume(value, VolumeChangeType.Voice);
    }
    void OnChangeValueToAudioControlleMaster(float value)
    {
        _audioController.SetVolume(value, VolumeChangeType.Master);
    }

    /// <summary>Optionのキャンバスの表示非表示</summary>
    /// <param name="isActive">表示するかどうか</param>
    public void OptionCanvasActive(bool isActive)
    {
        //表示した時
        if (isActive)
        {
            //それぞれの値を取得して、Sliderの値に代入して誤差をなくす
            _bgmSlider.value = _audioController.GetVolume(VolumeChangeType.BGM);
            _seSlider.value = _audioController.GetVolume(VolumeChangeType.SE); ;
            _voiceSlider.value = _audioController.GetVolume(VolumeChangeType.Voice);
            _masterSlider.value = _audioController.GetVolume(VolumeChangeType.Master);
        }
        _optionCanvas.SetActive(isActive);
    }
}
