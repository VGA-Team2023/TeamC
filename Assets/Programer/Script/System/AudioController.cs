using UnityEngine;
using System.Collections;
using CriWare;
using System;
using System.Reflection;

/// <summary>CueNameのデータと音をの再生する機能を保持・管理するクラス</summary>
public class AudioController : MonoBehaviour
{
    /// <summary>シングルトン化</summary>
    static AudioController _instance;

    [SerializeField,Tooltip("キューシートの名前")] string _cueSheetName = "Sound";
    [SerializeField,Tooltip("SEのCueNameのデータ")] SEAudioControlle _se;
    [SerializeField,Tooltip("BGMのCueNameのデータ")] BGMAudioControlle _bgm;
    [SerializeField,Tooltip("VoiceのCueNameのデータ")] VoiceAudioControlle _voice;

    /// <summary>シーン上にあるリスナーコンポーネント</summary>
    CriAtomListener _listener = null;

    VolumeChange volumeChange = new();
    
    public SEAudioControlle SE => _se;
    public BGMAudioControlle BGM => _bgm;
    public VoiceAudioControlle Voice => _voice;
    /// <summary>ボリューム設定</summary>
    public VolumeChange VolumeChange => volumeChange;
    public static AudioController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<AudioController>();
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
            CueSheetNameSet();
            SetListener();
            DontDestroyOnLoad(this);
        }
        else if (_instance == this)
        {
            CueSheetNameSet();
            SetListener();
            DontDestroyOnLoad(this);
        }
        else
        {
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
        CriAudioManager.Instance.SE.SetListenerAll(_listener);
        CriAudioManager.Instance.Voice.SetListenerAll(_listener);
    }
}

public class VolumeChange
{
    public enum Type
    {
        Master,
        BGM,
        SE,
        Voice,
    }
    public void OnVolumeChange(float value, Type type)
    {
        switch (type)
        {
            case Type.Master:
                CriAudioManager.Instance.MasterVolume.Value = value; break;
            case Type.BGM:
                CriAudioManager.Instance.BGM.Volume.Value = value; break;
            case Type.SE:
                CriAudioManager.Instance.SE.Volume.Value = value; break;
            case Type.Voice:
                CriAudioManager.Instance.Voice.Volume.Value = value; break;
        }
    }
}

