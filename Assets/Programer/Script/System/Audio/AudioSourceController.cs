using UnityEngine;

/// <summary>AudioSource用音再生管理クラス</summary>
public class AudioSourceController : MonoBehaviour, IPause
{
    static AudioSourceController _instance;

    GameManager _gameManager;

    [Header("音量調整")]

    [Tooltip("Voice")]
    [SerializeField, Range(0, 1)]
    float _voiceVolume = 1;

    [Header("音の設定(プログラマー用)")]

    [Tooltip("ボイス専用のAudioSourceコンポーネント")]
    [SerializeField]
    AudioSource _audioSourceVoice;

    [SerializeField]
    VoiceAudioSourceControlle _voice;

    public VoiceAudioSourceControlle Voice => _voice;

    public static AudioSourceController Instance
    {
        get
        {
            //instanceがnullだったら
            if (!_instance)
            {
                //シーン内のGameobjectにアタッチされているTを取得
                _instance = FindObjectOfType<AudioSourceController>();
                //アタッチされていなかったら
                if (!_instance)
                {
                    //エラーを出す
                    Debug.LogError("Scene内に" + typeof(AudioSourceController).Name + "をアタッチしているGameObjectがありません");
                }
            }
            return _instance;
        }
    }

    public void Awake()
    { 
        _instance = this;
        _gameManager = GameManager.Instance;
        _gameManager.PauseManager.Add(this);
        Set();
    }

    public void OnDestroy()
    {
        _gameManager.PauseManager.Remove(this);
        _instance = null;
    }

    public void OnValidate()
    {
        if(_audioSourceVoice != null)
        _audioSourceVoice.volume = _voiceVolume;
    }

    public void Pause()
    {
        _voice.Pause();
    }

    public void Resume()
    {
        _voice.Resume();
    }

    /// <summary>音データ管理をしている所に専用のAudioSourceをセットする</summary>
    public void Set()
    {
        _voice.Init(_audioSourceVoice);
    }
}
