using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMagicble
{

    void SetAttack(Transform enemy, Vector3 foward, AttackType attackType,float attackPower);

}
