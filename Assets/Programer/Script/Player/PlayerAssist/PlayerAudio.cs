using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private List<AudioSource> _audioSources;


    [Header("攻撃の弾を飛ばす音_氷")]
    [SerializeField] private AudioClip _iceFire;

    [Header("氷トレイル")]
    [SerializeField] private AudioClip _iceTrail;

    [Header("攻撃の弾を飛ばす音_氷")]
    [SerializeField] private AudioSource _iceCharge;

    [Header("====草属性===")]
    [Header("攻撃の弾を飛ばす音_草")]
    [SerializeField] private AudioClip _grassFire;

    [Header("草トレイル")]
    [SerializeField] private AudioClip _grassTrail;

    [Header("攻撃の弾を飛ばす音_草")]
    [SerializeField] private AudioSource _grassCharge;



    public void Fire(int num, bool isIce)
    {
        StartCoroutine(IceFires(num,isIce));
        StartCoroutine(IceTrails(num, isIce));
    }

    public IEnumerator IceFires(int num, bool isIce)
    {
        for (int i = 0; i < num; i++)
        {
            foreach (AudioSource source in _audioSources)
            {
                if (!source.isPlaying)
                {
                    if (isIce)
                    {
                        source.PlayOneShot(_iceFire);
                    }
                    else
                    {
                        source.PlayOneShot(_grassFire);
                    }

                    break;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator IceTrails(int num, bool isIce)
    {
        for (int i = 0; i < num; i++)
        {
            foreach (AudioSource source in _audioSources)
            {
                if (!source.isPlaying)
                {
                    if (isIce)
                    {
                    //    source.PlayOneShot(_iceTrail);
                    }
                    else
                    {
                      //  source.PlayOneShot(_grassTrail);
                    }

                    break;
                }
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
