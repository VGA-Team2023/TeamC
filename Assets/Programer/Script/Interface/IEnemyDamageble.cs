using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵にダメージを与えるインターフェイス
/// </summary>
public interface IEnemyDamageble
{
    /// <summary>攻撃を加える</summary>
    /// <param name="attackType"></param>
    /// <param name="attackHitTyp"></param>
    void Damage(AttackType attackType, MagickType attackHitTyp, float damage);
}


