using UnityEngine;
using CriWare;
using System;

/// <summary>CueNameのデータと音をの再生する機能を保持・管理するクラス</summary>
public class AudioController : MonoBehaviour,IPause,ISlow
{
    /// <summary>シングルトン化</summary>
    static AudioController _instance;
    CriAudioManager _criAudioManager;
    GameManager _gameManager;
    [Header("音量調整")]
    [SerializeField, Tooltip("全体の音量"), Range(0, 1)] float _masterVolume = 1;
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

    public void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _gameManager.PauseManager.Add(this);
        _gameManager.SlowManager.Add(this);
    }

    private void Update()
    {
        _criAudioManager.MasterVolume.Value = _masterVolume;
        _criAudioManager.BGM.Volume.Value = _bgmVolume;
        _criAudioManager.SE.Volume.Value = _seVolume;
        _criAudioManager.Voice.Volume.Value = _voiceVolume;
    }
    public void OnDisable()
    {
        _gameManager.PauseManager.Remove(this);
        _gameManager.SlowManager.Remove(this);
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
    
    public void SetVolume(float value, VolumeChangeType type)
    {
        switch (type)
        {
            case VolumeChangeType.Master:
                _instance._masterVolume = value; break;
            case VolumeChangeType.BGM:
                _instance._bgmVolume = value; break;
            case VolumeChangeType.SE:
                _instance._seVolume = value; break;
            case VolumeChangeType.Voice:
                _instance._voiceVolume = value; break;
        }
    }
    public float GetVolume(VolumeChangeType type)
    {
        float volume = 0;
        switch (type)
        {
            case VolumeChangeType.Master:
                return _instance._masterVolume;
            case VolumeChangeType.BGM:
                return _instance._bgmVolume;
            case VolumeChangeType.SE:
                return _instance._seVolume;
            case VolumeChangeType.Voice:
                return _instance._voiceVolume;
        }
        return volume;
    }

    public void Pause()
    {
        _bgm.PauseAll();
        _se.PauseAll();
        _voice.PauseAll();
    }

    public void Resume()
    {
        _bgm.ResumeAll();
        _se.ResumeAll();
        _voice.ResumeAll();

    }

    public void OnSlow(float slowSpeedRate)
    {
        _bgm.OnSlow(0.5f);
        _se.OnSlow(0.5f);
    }

    public void OffSlow()
    {
        _bgm.OffSlow();
        _se.OffSlow();
    }
}

public enum VolumeChangeType
{
    Master,
    BGM,
    SE,
    Voice,
}

