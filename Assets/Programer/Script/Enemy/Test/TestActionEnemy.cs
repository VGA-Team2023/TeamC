using UnityEngine;

public class TestActionEnemy : MonoBehaviour
{
    [SerializeField]
    bool _testOn = false;

    [SerializeField]
    MeleeAttackEnemy _meleeEnemy;

    [SerializeField]
    LongAttackEnemy _longEnemy;

    [SerializeField]
    MagickType _magicType;

    [SerializeField]
    HitType _hitType;

    enum HitType
    {
        Hit,
        Finish,
    }

    public void TestAttack()
    {
        if (!_testOn) return;
        if(_meleeEnemy)
        {
            if(_magicType == MagickType.Ice)
            {
                if (_hitType == HitType.Hit)
                {
                    _meleeEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Ice, 3f);
                }
                else if(_hitType == HitType.Finish)
                {
                    _meleeEnemy.EndFinishing(MagickType.Ice);
                }
            }
            else if(_magicType == MagickType.Grass)
            {
                if (_hitType == HitType.Hit)
                {
                    _meleeEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Grass, 3f);
                }
                else if (_hitType == HitType.Finish)
                {
                    _meleeEnemy.EndFinishing(MagickType.Grass);
                }
            }
        }
        else if(_longEnemy)
        {
            if (_magicType == MagickType.Ice)
            {
                if (_hitType == HitType.Hit)
                {
                    _longEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Ice, 3f);
                }
                else if (_hitType == HitType.Finish)
                {
                    _longEnemy.EndFinishing(MagickType.Ice);
                }
            }
            else if (_magicType == MagickType.Grass)
            {
                if (_hitType == HitType.Hit)
                {
                    _longEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Grass, 3f);
                }
                else if (_hitType == HitType.Finish)
                {
                    _longEnemy.EndFinishing(MagickType.Grass);
                }
            }
        }
    }

    //void Update()
    //{
    //    if (_testOn)
    //    {
    //        if (_meleeEnemy)
    //        {
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                _meleeEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Ice, 3f);
    //            }
    //            if (gameObject.layer == 10 && Input.GetMouseButtonDown(1))
    //            {
    //                _meleeEnemy.EndFinishing(MagickType.Ice);
    //            }
    //        }
    //        if (_longEnemy)
    //        {
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                _longEnemy.Damage(AttackType.ShortChantingMagick, MagickType.Ice, 3f);
    //            }
    //            if (gameObject.layer == 10 && Input.GetMouseButtonDown(1))
    //            {
    //                _longEnemy.EndFinishing(MagickType.Ice);
    //            }
    //        }
    //    }
    //}
}
