using System;
using UnityEngine;

/// <summary>音データの管理と再生などの操作を行う</summary>
[Serializable]
public struct VoiceAudioSourceControlle : IAudioSourceControlle<VoiceStateAudioSource>
{
    [SerializeField]
    [Tooltip("再生する音のデータ")]
    VoiceAudioSourceData[] _voiceData;

    /// <summary>再生する時に使うAudioSourceコンポーネント</summary>
    AudioSource _audioSource;
    public void Init(AudioSource source)
    {
        _audioSource = source;
    }

    public void Play(VoiceStateAudioSource state)
    {
        if(_audioSource.isPlaying)          //再生中だったら停止
            _audioSource.Stop();           

        _audioSource.clip = _voiceData[(int)state].AudioClip;
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void Pause()
    {
        //if (_audioSource == null) return;

         _audioSource.Pause();
    }

    public void Resume()
    {
        _audioSource.UnPause();
    }
}

/// <summary>AudioSourceコンポーネントで再生する音のデータ</summary>
[Serializable]
public struct VoiceAudioSourceData 
{
    [SerializeField] VoiceStateAudioSource _voiceType;
    [SerializeField] AudioClip _audioClip;

    /// <summary>再生する音の種類</summary>
    public VoiceStateAudioSource VoiceType => _voiceType;
    /// <summary>再生する音データ</summary>
    public AudioClip AudioClip => _audioClip;
}
