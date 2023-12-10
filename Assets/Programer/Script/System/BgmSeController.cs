using CriWare;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>PlayerのSEのAudio操作</summary>
[System.Serializable]
public struct SEAudioControlle : ICustomChannel<SEState>
{
    [SerializeField]Sound<SEState>[] _seData;
    string _cueSheetName;
    CriAtomListener _listener;
    public string CueSheetName { set { _cueSheetName = value; } }
    public CriAtomListener CriAtomListener { set { _listener = value; } }

    
    public void Play(SEState se)
    {
        int index = (int)se;
        _seData[index].PlayID = CriAudioManager.Instance.SE.Play(_cueSheetName, _seData[index].SoundCueName);
    }

    public void PauseAll()
    {
        CriAudioManager.Instance.SE.PauseAll();
    }
    /// <summary>SE再生３D対応</summary>
    /// <param name="se">再生させたいSE</param>
    /// <param name="soundPlayPos">自分の位置</param>
    public void Play3D(SEState se, Vector3 soundPlayPos)
    {
        int index = (int)se;
        _seData[index].PlayID = CriAudioManager.Instance.SE.Play3D(soundPlayPos, _cueSheetName, _seData[index].SoundCueName);
        CriAudioManager.Instance.SE.Update3DPos(soundPlayPos, _seData[index].PlayID);
    }

    public void Update3DPos(SEState se,Vector3 soundPlayPos)
    {
        int index = (int)se;
        CriAudioManager.Instance.SE.Update3DPos(soundPlayPos, _seData[index].PlayID);
    }

    public void Stop(SEState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.SE.Stop(_seData[index].PlayID);
    }

    public void StopAll()
    {
        CriAudioManager.Instance.SE.StopAll();
    }
    public void Pause(SEState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.SE.Pause(_seData[index].PlayID);
    }

    public void Resume(SEState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.SE.Resume(_seData[index].PlayID);
    }

    public void ResumeAll()
    {
        CriAudioManager.Instance.SE.ResumeAll();
    }
}
/// <summary>BGM用のAudio操作</summary>
[System.Serializable]
public struct BGMAudioControlle
{
    [SerializeField] Sound<BGMState>[] _bgmData;
    string _cueSheetName;
    public string CueSheetName { set { _cueSheetName = value; } }
    public void Pause(BGMState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.BGM.Pause(_bgmData[index].PlayID);
    }

    public void Play(BGMState se)
    {
        int index = (int)se;
        string cueName = _bgmData[index].SoundCueName;
        _bgmData[index].PlayID = CriAudioManager.Instance.BGM.Play(_cueSheetName, cueName);
    }

    public void Resume(BGMState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.BGM.Resume(_bgmData[index].PlayID);
    }
    public void Stop(BGMState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.SE.Stop(_bgmData[index].PlayID);
    }
}

/// <summary>VoiceのSE用のAudio操作</summary>
[System.Serializable]
public struct VoiceAudioControlle : ICustomChannel<VoiceState>
{
    [SerializeField] Sound<VoiceState>[] _voiceData;
    string _cueSheetName;
    public string CueSheetName { set { _cueSheetName = value; } }
    public void Pause(VoiceState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.Voice.Pause(_voiceData[index].PlayID);
    }

    public void PauseAll()
    {

    }

    public void Play(VoiceState se)
    {
        int index = (int)se;
        _voiceData[index].PlayID = CriAudioManager.Instance.SE.Play(_cueSheetName, _voiceData[index].SoundCueName);
    }

    public void Play3D(VoiceState se, Vector3 soundPlayPos)
    {
        int index = (int)se;
        _voiceData[index].PlayID = CriAudioManager.Instance.SE.Play3D(soundPlayPos, _cueSheetName, _voiceData[index].SoundCueName);
        CriAudioManager.Instance.Voice.Update3DPos(soundPlayPos, _voiceData[index].PlayID);
    }
    public void Update3DPos(VoiceState se, Vector3 soundPlayPos)
    {
        int index = (int)se;
        CriAudioManager.Instance.Voice.Update3DPos(soundPlayPos, _voiceData[index].PlayID);
    }

    public void Resume(VoiceState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.Voice.Resume(_voiceData[index].PlayID);
    }

    public void Stop(VoiceState se)
    {
        int index = (int)se;
        CriAudioManager.Instance.Voice.Stop(_voiceData[index].PlayID);
    }

    public void StopAll()
    {
        CriAudioManager.Instance.Voice.StopAll();
    }

    public void ResumeAll()
    {
        CriAudioManager.Instance.Voice.ResumeAll();
    }
}


