using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishAttackNearMagic
{
    [Header("èÄîıÇÃñÇñ@")]
    [SerializeField] private GameObject _effect;

    [Header("î≠ìÆÇµÇΩéûÇÃñÇñ@")]
    [SerializeField] private List<ParticleSystem> _setUpparticleSystems = new List<ParticleSystem>();

    [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetEffect()
    {
        _effect?.SetActive(true);
        foreach (var a in _setUpparticleSystems)
        {
            a.Play();
        }
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
        _effect?.SetActive(false);
    }

    public void End()
    {
        _effect?.SetActive(false);
    }

}
