using System.Collections.Generic;
using UnityEngine;

public class LongAttackEnemy : MonoBehaviour
{
    [SerializeField, Tooltip("ˆÚ“®æ‚ÌêŠ")]
    List<Transform> _movePosition = new List<Transform>();
    [SerializeField, Tooltip("‚Ç‚ê‚­‚ç‚¢ˆÚ“®æ‚É‹ß‚Ã‚¢‚½‚çŸ‚Ì’n“_‚És‚­‚©")]
    float _changePointDistance = 0.5f;
    List<Vector3> _patrolPoint = new List<Vector3>();
    int _index = 0;

    void Start()
    {
        _patrolPoint.Add(transform.position);
        foreach(var point in _movePosition)
        {
            _patrolPoint.Add(point.position);
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _patrolPoint[_index % _patrolPoint.Count]);
        if(distance < _changePointDistance)
        {
            _index++;
        }
    }
}
