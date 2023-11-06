using UnityEngine;
using Utils;

public class MoveRangeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var enemy = GetComponent<MeleeAttackEnemy>();
        if (enemy)
        {
            Gizmos.color = Color.yellow;
            GizmosExtensions.DrawWireCircle(transform.position, enemy.MoveRange);
        }
    }
}
