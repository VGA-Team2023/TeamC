using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossFollowAttackBase
{
    [SerializeField] private string _name;

    [Header("oŒ»‚³‚¹‚éOffset")]
    [SerializeField] private Vector3 _offSet = new Vector3(0, -1, 0);

    [Header("UŒ‚—p‚Ì–‚–@w_•X")]
    [SerializeField] private GameObject _magicCircleIce;

    [Header("UŒ‚—p‚Ì–‚–@w_‘")]
    [SerializeField] private GameObject _magicCircleGrass;

    [Header("UŒ‚ÀsŠÔ")]
    [SerializeField] private float _attackTime = 10f;

    [Header("–‚–@wİ’u‚ÉŠÔ")]
    [SerializeField] private float _attackSetTime = 1f;

    private float _countAttackTime = 0;

    private float _countAttackSetTime = 0;

    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }


    /// <summary>UŒ‚’†’fˆ—</summary>
    public void StopAtttack()
    {
        _countAttackSetTime = 0;
        _countAttackTime = 0;
    }

    public bool DoAttack()
    {
        _countAttackTime += Time.deltaTime;
        _countAttackSetTime += Time.deltaTime;

        if (_countAttackTime > _attackTime)
        {
            _countAttackTime = 0;
            _countAttackSetTime = 0;
            return true;
        }

        if (_countAttackSetTime > _attackSetTime)
        {
            Attack();
            _countAttackSetTime = 0;
        }   //–‚–@wİ’u

        return false;
    }

    public void Attack()
    {
        GameObject bullet = default;

        if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
        {
            bullet = _magicCircleIce;
        }
        else
        {
            bullet = _magicCircleGrass;
        }
        var go = GameObject.Instantiate(bullet);
        go.transform.position = _bossControl.PlayerT.position + _offSet;
    }


}
