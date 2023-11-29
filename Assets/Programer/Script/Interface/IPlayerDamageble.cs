using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playerにダメージを与えるインターフェイス
/// </summary>
public interface IPlayerDamageble
{

    /// <summary>ダメージを与える </summary>
    /// <param name="damage">攻撃力</param>
     void Damage(float damage);
}
