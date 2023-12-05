using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


[System.Serializable]
public class BossAttack
{
    [Header("’ÊíUŒ‚")]
    [SerializeField] private BossNomalAttack _nomalAttack;

    [Header("’Ç]UŒ‚")]
    [SerializeField] private BossFollowAttack _followAttack;

    [Header("UŒ‚ŠÔ‚ÌƒN[ƒ‹ƒ^ƒCƒ€")]
    [SerializeField] private float _attackCoolTime = 5f;

    [Header("’ÊíUŒ‚‚Ì%")]
    [Range(0, 9)]
    [SerializeField] private int _attackNomalParcent = 4;

    private BossAttackType _bossAttackMagicTypes;

    private float _countCoolTime = 0;

    private bool _isAttackNow = false;

    private bool _isEndAttack = false;

    private BossControl _bossControl;

    public BossFollowAttack BossFollowAttack => _followAttack;
    public BossNomalAttack NomalAttack => _nomalAttack;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
        _nomalAttack.Init(bossControl);
        _followAttack.Init(bossControl);
    }

    public void AttackStartSet()
    {
        _isAttackNow = true;

        var r = Random.Range(0, 10);

        if (_bossAttackMagicTypes == BossAttackType.Nomal)
        {
            _bossAttackMagicTypes = BossAttackType.Follow;
            _nomalAttack.AttackFirstSet();
        }
        else
        {
            _bossAttackMagicTypes = BossAttackType.Nomal;
            _followAttack.SetAttack();
        }
    }

    public void StopAttack()
    {
        if (_bossAttackMagicTypes == BossAttackType.Nomal)
        {
            _nomalAttack.StopAttack();
        }
        else
        {
            _followAttack.StopAttack();
        }
    }


    public void Updata()
    {
        AttackCoolTime();
        AttackDo();
    }

    public void AttackCoolTime()
    {
        if (!_isAttackNow)
        {
            _countCoolTime += Time.deltaTime;

            if (_countCoolTime > _attackCoolTime)
            {
                _countCoolTime = 0;
                AttackStartSet();
            }
        }
    }

    public void AttackDo()
    {
        if (!_isAttackNow) return;

        if (_bossAttackMagicTypes == BossAttackType.Nomal)
        {
            if (_nomalAttack.DoAttack())
            {
                _isEndAttack = true;
                _isAttackNow = false;
            }
        }
        else
        {
            if (_followAttack.DoAttack())
            {
                _isEndAttack = true;
                _isAttackNow = false;
            }
        }
    }


}


public enum BossAttackType
{
    Nomal,
    Follow,
}