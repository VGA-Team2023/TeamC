using UnityEngine;
using System;

public interface ICustomChannel<TState> where TState : Enum
{
    /// <summary>１つの音を再生する関数(Playerの属性によって再生音が変わる)</summary>
    /// <param name="se">流したい音(enumで選択)</param>
    /// <param name="attribute">Playerの属性(enumで選択)</param>
    public void Play(TState se);

    /// <summary>１つの音を再生する関数(3D)(Playerの属性によって再生音が変わる)</summary>
    /// <param name="se">流したい音(enumで選択)</param>
    /// <param name="soundPlayPos">流すPositionのWorldSpace</param>
    /// <param name="attribute">Playerの属性(enumで選択)</param>
    public void Play3D(TState se, Vector3 soundPlayPos);

    /// <summary>3Dの流すPositionを更新する</summary>
    /// <param name="playSoundWorldPos">更新するPosition</param>
    /// <param name="index">変更する音(enumで選択)</param>
    public void Update3DPos(TState se,Vector3 soundPlayPos);

    /// <summary>１つの音を停止する関数</summary>
    /// <param name="se">停止したい音(enumで選択)</param>
    public void Stop(TState se);

    /// <summary>全ての音を停止する関数</summary>
    public void StopAll();

    /// <summary>全ての音を一時停止</summary>
    public void PauseAll();

    /// <summary>１つの音を一時停止する関数</summary>
    /// <param name="se">一時停止したい音(enumで選択)</param>
    public void Pause(TState se);

    /// <summary>一時停止した音の中で１つの音を再生する関数(Playerの属性によって再生音が変わる)</summary>
    /// <param name="se">再生したい音(enumで選択)</param>
    /// <param name="attribute">Playerの属性(enumで選択)</param>
    public void Resume(TState se);

    public void ResumeAll();
}

/// <summary>StateごとのCueNameを保持する構造体</summary>
/// <typeparam name="TState">どのSEのStateか</typeparam>
[System.Serializable]
public class Sound<TState> where TState : Enum
{
    [SerializeField,Tooltip("サウンドのタイプ")] TState state;
    [SerializeField,Tooltip("キューの名前")] string soundCueName;
    int playID = 0;
    public string SoundCueName => soundCueName;
    public int PlayID { get { return playID; } set { playID = value; } }
}