using UnityEngine;
using Utils;

public class MoveRangeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var enemy = GetComponent<MeleeAttackEnemy>();
        if (enemy)
        {
            Gizmos.color = Color.blue;
            GizmosExtensions.DrawWireCircle(transform.position, enemy.MoveRange);
        }
    }
}
