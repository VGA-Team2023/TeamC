using UnityEngine;
using Utils;

public class PlayerSearchRangeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var enemy = GetComponent<EnemyBase>();
        if (enemy)
        {
            Gizmos.color = Color.red;
            GizmosExtensions.DrawWireCircle(transform.position, enemy.SearchRange);
        }
    }
}
