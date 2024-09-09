using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceTemplate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //再生(属性変更)
        AudioSourceController.Instance.Voice.Play(VoiceStateAudioSource.InstructorTutorialAttributeChange);
    }

    public void Stop()
    {
        //停止
        AudioSourceController.Instance.Voice.Stop();
    }

    public void Pause()
    {
        //一時停止
        AudioSourceController.Instance.Voice.Pause();
    }

    public void Resume()
    {
        //一時停止解除
        AudioSourceController.Instance.Voice.Resume();
    }
}
