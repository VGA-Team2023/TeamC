using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private List<AudioSource> _audioSources;

    [Header("攻撃の弾を飛ばす音_氷")]
    [SerializeField] private AudioSource _iceCharge;

    [Header("====草属性===")]
    [Header("攻撃の弾を飛ばす音_草")]
    [SerializeField] private AudioSource _grassCharge;

    [Header("トドメ声＿溜め")]
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();

    [Header("氷とどめ")]
    [SerializeField] private List<AudioClip> _clipsFinish = new List<AudioClip>();

    [Header("氷とどめ")]
    [SerializeField] private List<AudioClip> _clipsFinishGrass = new List<AudioClip>();

    [Header("トドメ氷の音")]
    [SerializeField] private AudioSource _completeFinishIce;

    [Header("トドメ草の音")]
    [SerializeField] private AudioSource _completeFinishGrass;

    [Header("トドメ氷の音_溜め")]
    [SerializeField] private AudioSource _chargeFinishIce;

    [Header("トドメ草の音_溜め")]
    [SerializeField] private AudioSource _chargeFinishGrass;

    public enum PlayMagicAudioType
    {
        Play,
        Stop,
        Updata,
    }


    public void PlayFinishVoice(bool isChage, bool isIce)
    {
        if (isChage)
        {
            if (isIce)
            {
                _chargeFinishIce.Play();
            }
            else
            {
                _chargeFinishGrass.Play();
            }

            foreach (var a in _audioSources)
            {
                if (!a.isPlaying)
                {
                    var r = Random.Range(0, _clips.Count);
                    a.PlayOneShot(_clips[r]);
                    return;
                }
            }
        }
        else
        {
            if (isIce)
            {
                _chargeFinishIce.Stop();
            }
            else
            {
                _chargeFinishGrass.Stop();
            }


            if (isIce)
            {
                _completeFinishIce.Play();
                foreach (var a in _audioSources)
                {
                    if (!a.isPlaying)
                    {
                        var r = Random.Range(0, _clipsFinish.Count);
                        a.PlayOneShot(_clipsFinish[r]);
                        return;
                    }
                }
            }
            else
            {
                _completeFinishGrass.Play();
                foreach (var a in _audioSources)
                {
                    if (!a.isPlaying)
                    {
                        var r = Random.Range(0, _clipsFinishGrass.Count);
                        a.PlayOneShot(_clipsFinishGrass[r]);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>音を流す</summary>
    /// <param name="isPlay"></param>
    public void AudioSet(SEState audio, PlayMagicAudioType playType)
    {
        //音の再生方法に応じて分ける
        if (playType == PlayMagicAudioType.Play)
        {
            AudioController.Instance.SE.Play3D(audio, transform.position);
        }
        else if (playType == PlayMagicAudioType.Stop)
        {
            AudioController.Instance.SE.Stop(audio);
        }
        else if (playType == PlayMagicAudioType.Updata)
        {
            AudioController.Instance.SE.Update3DPos(audio, transform.position);
        }
    }

    public void Fire(int num, bool isIce)
    {
        StartCoroutine(IceFires(num, isIce));
    }

    public IEnumerator IceFires(int num, bool isIce)
    {
        for (int i = 0; i < num; i++)
        {
            if (isIce)
            {
                AudioSet(SEState.PlayerShootIce, PlayMagicAudioType.Play);
            }
            else
            {
                AudioSet(SEState.PlayerShootGrass, PlayMagicAudioType.Play);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void AttackCharge(bool isPlay, bool isIce)
    {
        if (isIce)
        {
            if (isPlay)
            {
                _iceCharge.Play();
            }
            else
            {
                _iceCharge.Stop();
            }
        }
        else
        {
            if (isPlay)
            {
                _grassCharge.Play();
            }
            else
            {
                _grassCharge.Stop();
            }
        }

    }

}
