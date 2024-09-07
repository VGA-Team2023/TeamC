using UnityEngine;
using CriWare;
using System;

/// <summary>CueNameのデータと音をの再生する機能を保持・管理するクラス</summary>
public class AudioController : MonoBehaviour,IPause
{
    /// <summary>シングルトン化</summary>
    static AudioController _instance;

    CriAudioManager _criAudioManager;

    GameManager _gameManager;

    [Header("音量調整")]

    [SerializeField, Tooltip("全体の音量"), Range(0, 1)]
    float _masterVolume = 1;

    [SerializeField, Tooltip("BGM"), Range(0,1)]
    float _bgmVolume = 1; 

    [SerializeField, Tooltip("SE"), Range(0, 1)]
    float _seVolume = 1; 

    [SerializeField, Tooltip("Voice"), Range(0, 1)]
    float _voiceVolume = 1;

    [SerializeField, Tooltip("ME"), Range(0, 1)] 
    float _meVolume = 1;

    [Space]
    [Space]

    [Header("CueSheetName・CueNameの設定(プログラマー用)")]

    [SerializeField,Tooltip("キューシートの名前")]
    string _cueSheetName = "Sound";

    [SerializeField,Tooltip("SEのCueNameのデータ")]
    SEAudioControlle _se;

    [SerializeField,Tooltip("BGMのCueNameのデータ")]
    BGMAudioControlle _bgm;

    [SerializeField,Tooltip("VoiceのCueNameのデータ")] 
    VoiceAudioControlle _voice;

    /// <summary>シーン上にあるリスナーコンポーネント</summary>
    CriAtomListener _listener = null;
    public SEAudioControlle SE => _se;
    public BGMAudioControlle BGM => _bgm;
    public VoiceAudioControlle Voice => _voice;

    public static AudioController Instance
    {
        get
        {
            //空だったら
            if (!_instance)
            {
                //シーン上にあるものから探す
                _instance = FindObjectOfType<AudioController>();
                
                //それでも空だったら
                if (!_instance)
                {
                    //エラーを出す
                    Debug.LogError("Scene内に" + typeof(AudioController).Name + "をアタッチしているGameObjectがありません");
                }

                _instance._criAudioManager = CriAudioManager.Instance;
                _instance.CueSheetNameSet();
                _instance.SetListener();
            }
            return _instance;
        }
    }

    public void OnEnable()
    {
        //一時停止の登録
        _instance._gameManager = GameManager.Instance;
        _instance._gameManager.PauseManager.Add(this);
    }

    private void Update()
    {
        //ボリューム変更しても元データにわたるようにする
        _criAudioManager.MasterVolume.Value = _masterVolume;
        _criAudioManager.BGM.Volume.Value = _bgmVolume;
        _criAudioManager.SE.Volume.Value = _seVolume;
        _criAudioManager.Voice.Volume.Value = _voiceVolume;
    }
    public void OnDisable()
    {
        //一時停止の解除
        _instance._gameManager.PauseManager.Remove(this);
    }
    private void Awake()
    {
        //空だったら
        if (_instance == null)
        {
            _instance = this;
            _instance._criAudioManager = CriAudioManager.Instance;
            CueSheetNameSet();
            SetListener();
            DontDestroyOnLoad(this);
        }
        //先に参照されておりすでに自分が入っていた場合
        else if (_instance == this)
        {
            DontDestroyOnLoad(this);
        }
        //すでに他が入っていた場合
        else
        {
            _instance._criAudioManager = CriAudioManager.Instance;
            CueSheetNameSet();
            SetListener();
            //自分を消す
            Destroy(this);
        }
    }
    public void CueSheetNameSet()
    {
        //シートの登録
        _instance._bgm.CueSheetName = _cueSheetName;
        _instance._se.CueSheetName = _cueSheetName;
        _instance._voice.CueSheetName = _cueSheetName;
    }

    /// <summary>リスナーの設定</summary>
    public void SetListener()
    {
        //リスナーを取得し、保持
        _instance._listener = Camera.main.GetComponent<CriAtomListener>();
        //それぞれの音チャンネルにリスナー設定
        _instance._criAudioManager.BGM.SetListenerAll(_instance._listener);
        _instance._criAudioManager.SE.SetListenerAll(_instance._listener);
        _instance._criAudioManager.Voice.SetListenerAll(_instance._listener);
    }

    /// <summary>BGMなどのボリューム値設定</summary>
    /// <param name="type">設定したい音</param>
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

    /// <summary>BGMなどのボリューム値取得</summary>
    /// <param name="type">取得したい音</param>
    /// <returns>ボリューム値</returns>
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
        _se.PauseAll();
        _voice.PauseAll();
    }

    public void Resume()
    {
        _se.ResumeAll();
        _voice.ResumeAll();
    }
}

public enum VolumeChangeType
{
    Master,
    BGM,
    SE,
    Voice,
}

