using UnityEngine;
using Utils;

public class MoveRangeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var enemy = GetComponent<MeleeAttackEnemy>();
        if (enemy)
        {
            //移動範囲を描画
            Gizmos.color = Color.blue;
            GizmosExtensions.DrawWireCircle(transform.position, enemy.MoveRange);
        }
    }
}
