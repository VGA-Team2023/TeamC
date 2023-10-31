using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAttack : MonoBehaviour,IEnemyDamageble
{
    [Header("çUåÇóÕ")]
    [SerializeField] private float _damage;

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IPlayerDamageble>(out IPlayerDamageble damageble);
        damageble?.Damage(_damage);
    }

}
