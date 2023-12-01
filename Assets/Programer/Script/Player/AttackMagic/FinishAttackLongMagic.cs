using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishAttackLongMagic
{
    [Header("準備の魔法")]
    [SerializeField] private GameObject _effect;

    [Header("発動した時の魔法")]
    [SerializeField] private GameObject _finishEffect;

    [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private PlayerControl _playerControl;



    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetEffect()
    {
        _effect.SetActive(true);
    }

    public void SetFinishEffect()
    {
        foreach (var a in particleSystems)
        {
            a.Play();
        }
    }

    public void Stop()
    {
        _effect.SetActive(false);
    }

    public void End()
    {
        _effect.SetActive(false);
    }

}
