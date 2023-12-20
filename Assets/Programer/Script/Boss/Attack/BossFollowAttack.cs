using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFollowAttack
{
    [Header("UŒ‚‚Ìí—Şİ’è")]
    [SerializeField] private List<BossFollowAttackBase> _bossFollowAttackBase = new List<BossFollowAttackBase>();

    [Header("—­‚ß_•X")]
    [SerializeField] private List<ParticleSystem> _chargeIce = new List<ParticleSystem>();

    [Header("—­‚ß_‘")]
    [SerializeField] private List<ParticleSystem> _chargeGrass = new List<ParticleSystem>();

    BossFollowAttackBase _setAttack;

    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;

        foreach (var a in _bossFollowAttackBase)
        {
            a.Init(bossControl);
        }
    }

    public void SetAttack()
    {
        int r = Random.Range(0, _bossFollowAttackBase.Count);
        _setAttack = _bossFollowAttackBase[1];

        if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
        {
            _chargeIce.ForEach(i => i.Play());
        }
        else
        {
            _chargeGrass.ForEach(i => i.Play());
        }

    }

    /// <summary>UŒ‚’†’fˆ—</summary>
    public void StopAttack()
    {
        if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
        {
            _chargeIce.ForEach(i => i.Stop());
        }
        else
        {
            _chargeGrass.ForEach(i => i.Stop());
        }

        _setAttack.StopAtttack();
    }


    public bool DoAttack()
    {
        if (_setAttack.DoAttack())
        {
            if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
            {
                _chargeIce.ForEach(i => i.Stop());
            }
            else
            {
                _chargeGrass.ForEach(i => i.Stop());
            }
            return true;
        }
        else
        {
            return false;
        }
    }

}
