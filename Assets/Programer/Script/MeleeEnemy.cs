using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeleeEnemy : EnemyBase
{
    [SerializeField, Tooltip("どれくらいベース位置に近づいたら次の目標に向かうか")]
    float _distance;
    [SerializeField, Tooltip("移動の範囲")]
    float _moveRange;
    [SerializeField]
    GameObject _destination;
    [SerializeField]
    GameObject _base;

    Vector3 _basePos;
    Rigidbody _rb;
    Vector3 _dir;
    bool _isArrived = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _basePos = transform.position;
        //Instantiate(_base, _basePos, Quaternion.identity);
        _dir = GetMovePoint();
        transform.forward = (_dir - transform.position).normalized;
    }

    void Update()
    {
        float baseDis = Vector3.Distance(transform.position, _basePos);
        float destinationDis = Vector3.Distance(transform.position, _dir);
        if (baseDis < _distance && !_isArrived)
        {
            _dir = GetMovePoint();
            transform.forward = (_dir - transform.position).normalized;
            _isArrived = true;
        }
        else if(destinationDis < _distance && _isArrived)
        {
            _dir = _basePos - transform.position;
            transform.forward = _dir.normalized;
            _isArrived = false;
        }
        _rb.velocity = transform.forward * base.Speed;
        transform.position = new Vector3(transform.position.x, _basePos.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _dir = _basePos - transform.position;
        transform.forward = _dir.normalized;
        _isArrived = false;
    }

    Vector3 GetMovePoint()
    {
        float random = Random.Range(0, 361);
        var dir =  new Vector3(Mathf.Sin(random) * _moveRange + transform.position.x, _basePos.y, Mathf.Cos(random) * _moveRange + transform.position.z);
        //Instantiate(_destination, new Vector3(dir.x, _basePos.y, dir.z), Quaternion.identity);
        return dir;
    }
}
