using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ghost : EnemyBase
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
        Instantiate(_base, _basePos, Quaternion.identity);
        _dir = new Vector3
                (Mathf.Sin(Random.Range(0, 361)) * _moveRange,
                _basePos.y,
                Mathf.Cos(Random.Range(0, 361) * _moveRange));
        Instantiate(_destination, new Vector3(_dir.x, _basePos.y, _dir.z), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        float baseDis = Vector3.Distance(transform.position, _basePos);
        float destinationDis = Vector3.Distance(transform.position, _dir);
        
        if (baseDis < _distance && !_isArrived)
        {
            float random = Random.Range(0, 361);
            _dir = new Vector3
                (Mathf.Sin(random) * _moveRange,
                _basePos.y,
                Mathf.Cos(random) * _moveRange);
            Instantiate(_destination, new Vector3(_dir.x, _basePos.y, _dir.z), Quaternion.identity);
            _isArrived = true;
        }
        else if(destinationDis < _distance && _isArrived)
        {
            _dir = (_basePos - transform.position).normalized;
            _isArrived = false;
        }
        transform.forward = _dir;
        _rb.velocity = transform.forward * base.Speed;
        transform.position = new Vector3(transform.position.x, _basePos.y, transform.position.z);
    }
}
