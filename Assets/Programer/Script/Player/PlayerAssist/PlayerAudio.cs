using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private List<AudioSource> _audioSources;


    [Header("�U���̒e���΂���_�X")]
    [SerializeField] private AudioClip _iceFire;

    [Header("�X�g���C��")]
    [SerializeField] private AudioClip _iceTrail;

    [Header("�U���̒e���΂���_�X")]
    [SerializeField] private AudioSource _iceCharge;


    public void IceFire(int num)
    {
        StartCoroutine(IceFires(num));
        StartCoroutine(IceTrails(num));
    }

    public IEnumerator IceFires(int num)
    {
        for (int i = 0; i < num; i++)
        {
            foreach (AudioSource source in _audioSources)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(_iceFire);
                    break;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator IceTrails(int num)
    {
        for (int i = 0; i < num; i++)
        {
            foreach (AudioSource source in _audioSources)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(_iceTrail);
                    break;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }


    public void IceCharge(bool isPlay)
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

}