using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFollowAttack
{
    [Header("攻撃の種類設定")]
    [SerializeField] private List<BossFollowAttackBase> _bossFollowAttackBase = new List<BossFollowAttackBase>();

    [Header("溜め_氷")]
    [SerializeField] private List<ParticleSystem> _chargeIce = new List<ParticleSystem>();

    [Header("溜め_草")]
    [SerializeField] private List<ParticleSystem> _chargeGrass = new List<ParticleSystem>();

    private float _countAnim = 0;

    private bool _isAnimPlay = false;

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
        _setAttack = _bossFollowAttackBase[r];

        if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
        {
            _chargeIce.ForEach(i => i.Play());
        }
        else
        {
            _chargeGrass.ForEach(i => i.Play());
        }

        _isAnimPlay = false;
        _countAnim = 0;
    }

    /// <summary>攻撃中断処理</summary>
    public void StopAttack()
    {
        //アニメーション
        _bossControl.BossAnimControl.IsCharge(false);
        _bossControl.BossAnimControl.Attack(false);

        //エフェクト停止
        _chargeIce.ForEach(i => i.Stop());
        _chargeGrass.ForEach(i => i.Stop());


        _setAttack.StopAtttack();
    }


    public bool DoAttack()
    {
        if (_setAttack.DoAttack())
        {
            _bossControl.BossAnimControl.IsCharge(false);
            _bossControl.BossAnimControl.Attack(true);

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
