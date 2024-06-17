using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioSourceControlle<TState> where TState : Enum
{
    /// <summary>再生するのに使用するAudioSourceをセットする</summary>
    /// <param name="source">使用するAudioSource</param>
    void Init(AudioSource source);
    /// <summary>再生する</summary>
    /// <param name="state">流したい音(enumで選択)</param>
    void Play(TState state);

    /// <summary>停止する</summary>
    public void Stop();

    /// <summary>一時停止する</summary>
    void Pause();

    /// <summary>一時停止を解除</summary>
    void Resume();
}
