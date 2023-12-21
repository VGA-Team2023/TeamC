using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlay : MonoBehaviour
{
    [SerializeField]
    LongAttackEnemy _enemy;

    public void Attack()
    {
        _enemy.Attack();
    }
}
