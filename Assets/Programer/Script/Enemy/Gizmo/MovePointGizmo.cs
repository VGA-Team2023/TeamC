using UnityEngine;
using Utils;

public class MovePointGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var enemy = GetComponent<LongAttackEnemy>();
        if (enemy)
        {
            Gizmos.color = Color.blue;
            var movePosition = enemy.SetMovePoint();
            for(int i = 0; i < movePosition.Count; i++)
            {
                if(i + 1 >= movePosition.Count)
                {
                    GizmosExtensions.DrawArrow(movePosition[i], movePosition[0]);
                }
                else
                {
                    GizmosExtensions.DrawArrow(movePosition[i], movePosition[i + 1]);
                }
            }
        }
    }
}
