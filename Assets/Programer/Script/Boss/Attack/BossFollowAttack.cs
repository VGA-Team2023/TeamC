using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFollowAttack
{
    [Header("çUåÇÇÃéÌóﬁê›íË")]
    [SerializeField] private List<BossFollowAttackBase> _bossFollowAttackBase = new List<BossFollowAttackBase>();

    BossFollowAttackBase _setAttack;

    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;

        foreach (var a in _bossFollowAttackBase)
        {
            a.Init(bossControl);
        }
        SetAttack();
    }

    public void SetAttack()
    {
        int r = Random.Range(0, _bossFollowAttackBase.Count);
        _setAttack = _bossFollowAttackBase[r];
       // _setAttack = _bossFollowAttackBase[0];
    }

    /// <summary>çUåÇíÜífèàóù</summary>
    public void StopAttack()
    {
       _setAttack.StopAtttack();
    }


    public bool DoAttack()
    {
       return _setAttack.DoAttack();
    }

}
