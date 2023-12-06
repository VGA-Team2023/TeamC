using UnityEngine;
using Utils;

public class PlayerSearchRangeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var enemy = GetComponent<EnemyBase>();
        if (enemy)
        {
            //プレイヤーを検出する範囲を描画
            Gizmos.color = Color.red;
            GizmosExtensions.DrawWireCircle(transform.position, enemy.SearchRange);
        }
    }
}
