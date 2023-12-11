using UnityEngine;
using CriWare;
using UniRx;
using System;

/// <summary>CueNameのデータと音をの再生する機能を保持・管理するクラス</summary>
public class AudioController : MonoBehaviour
{
    /// <summary>シングルトン化</summary>
    static AudioController _instance;
    CriAudioManager _criAudioManager;
    [Header("音量調整")]
    [SerializeField, Tooltip("BGM"), Range(0,1)] float _bgmVolume = 1; 
    [SerializeField, Tooltip("SE"), Range(0, 1)] float _seVolume = 1; 
    [SerializeField, Tooltip("Voice"), Range(0, 1)] float _voiceVolume = 1;
    [SerializeField, Tooltip("ME"), Range(0, 1)] float _meVolume = 1;
    [Space]
    [Space]
    [Header("CueSheetName・CueNameの設定(プログラマー用)")]
    [SerializeField,Tooltip("キューシートの名前")] string _cueSheetName = "Sound";
    [SerializeField,Tooltip("SEのCueNameのデータ")] SEAudioControlle _se;
    [SerializeField,Tooltip("BGMのCueNameのデータ")] BGMAudioControlle _bgm;
    [SerializeField,Tooltip("VoiceのCueNameのデータ")] VoiceAudioControlle _voice;
    /// <summary>シーン上にあるリスナーコンポーネント</summary>
    CriAtomListener _listener = null;
    public SEAudioControlle SE => _se;
    public BGMAudioControlle BGM => _bgm;
    public VoiceAudioControlle Voice => _voice;

    public float BgmVolume => _bgmVolume;
    public float VoiceVolume => _voiceVolume;
    public float SeVolume => _seVolume;
    public static AudioController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<AudioController>();
                _instance._criAudioManager = CriAudioManager.Instance;
                _instance.CueSheetNameSet();
                _instance.SetListener();

                if (!_instance)
                {
                    Debug.LogError("Scene内に" + typeof(AudioController).Name + "をアタッチしているGameObjectがありません");
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _instance._criAudioManager = CriAudioManager.Instance;
            CueSheetNameSet();
            SetListener();
            DontDestroyOnLoad(this);
        }
        else if (_instance == this)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            _instance._criAudioManager = CriAudioManager.Instance;
            CueSheetNameSet();
            SetListener();
            Destroy(this);
        }
    }

    public void Update()
    {
        _criAudioManager.BGM.Volume.Value = _bgmVolume;
        _criAudioManager.SE.Volume.Value = _seVolume;
        _criAudioManager.Voice.Volume.Value = _voiceVolume;
    }
    public void CueSheetNameSet()
    {
        _instance._bgm.CueSheetName = _cueSheetName;
        _instance._se.CueSheetName = _cueSheetName;
        _instance._voice.CueSheetName = _cueSheetName;
    }

    public void SetListener()
    {
        _instance._listener = Camera.main.GetComponent<CriAtomListener>();
        _instance._criAudioManager.BGM.SetListenerAll(_instance._listener);
        _instance._criAudioManager.SE.SetListenerAll(_instance._listener);
        _instance._criAudioManager.Voice.SetListenerAll(_instance._listener);
    }
    
    public void OnVolumeChange(float value, VolumeChangeType type)
    {
        switch (type)
        {
            //case VolumeChangeType.Master:
            //    CriAudioManager.Instance.MasterVolume.Value = value; break;
            case VolumeChangeType.BGM:
                _instance._bgmVolume = value; break;
            case VolumeChangeType.SE:
                _instance._seVolume = value; break;
            case VolumeChangeType.Voice:
                _instance._voiceVolume = value; break;
        }
    }  
}

public enum VolumeChangeType
{
    //Master,
    BGM,
    SE,
    Voice,
}

