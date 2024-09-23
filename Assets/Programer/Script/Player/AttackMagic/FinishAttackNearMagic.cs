using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishAttackNearMagic
{
    [Header("準備の魔法_氷")]
    [SerializeField] private GameObject _effectIce;

    [Header("発動した時の魔法_氷")]
    [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    [Header("準備の魔法_草")]
    [SerializeField] private GameObject _effectGrass;



    [Header("発動した時の魔法_草")]
    [SerializeField] private List<ParticleSystem> particleSystemsGrass = new List<ParticleSystem>();

    [Header("中止した時の魔法_氷")]
    [SerializeField] private List<ParticleSystem> _stopParticleSystemIce = new List<ParticleSystem>();

    [Header("中止した時の魔法_草")]
    [SerializeField] private List<ParticleSystem> _stopParticleSystemsGrass = new List<ParticleSystem>();



    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Stop(PlayerAttribute attribute)
    {
        if(attribute == PlayerAttribute.Ice)
        {
            foreach (var a in _stopParticleSystemIce)
            {
                a.Play();
            }
        }
        else
        {
            foreach (var a in _stopParticleSystemsGrass)
            {
                a.Play();
            }
        }
    }

    public void SetEffect()
    {
        if (_playerControl.PlayerAttributeControl.PlayerAttribute == PlayerAttribute.Ice)
        {
            _effectIce?.SetActive(true);
            //foreach (var a in _setUpparticleSystems)
            //{
            //    a.Play();
            //}
        }
        else
        {
            _effectGrass?.SetActive(true);
            //foreach (var a in _setUpparticleSystemsGrass)
            //{
            //    a.Play();
            //}
        }
    }

    public void SetFinishEffect()
    {
        if (_playerControl.PlayerAttributeControl.PlayerAttribute == PlayerAttribute.Ice)
        {
            //foreach (var a in particleSystems)
            //{
            //    a.time = 0;
            //    a.Play();
            //}
        }
        else
        {
            //foreach (var a in particleSystemsGrass)
            //{
            //    a.time = 0;
            //    a.Play();
            //}
        }
    }

    public void Stop()
    {
        _effectIce?.SetActive(false);
        _effectGrass?.SetActive(false);
    }

    public void End()
    {
        _effectIce?.SetActive(false);
    }

}
