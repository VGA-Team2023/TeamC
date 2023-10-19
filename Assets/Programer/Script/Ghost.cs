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
                (Mathf.Sin(Random.Range(0, 361)) * Mathf.Cos(0) * _moveRange,
                0,
                Mathf.Cos(Random.Range(0, 361) * _moveRange));
        Instantiate(_destination, _dir, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        float baseDis = Vector3.Distance(transform.position, _basePos);
        float destinationDis = Vector3.Distance(transform.position, _dir);
        
        if (baseDis < _distance && !_isArrived)
        {
            _rb.velocity = Vector3.zero;
            _dir = new Vector3
                (Mathf.Sin(Random.Range(0, 361)) * Mathf.Cos(Random.Range(0, 361)) * _moveRange,
                0,
                Mathf.Cos(Random.Range(0, 361)) * _moveRange);
            Instantiate(_destination, _dir, Quaternion.identity);
            _isArrived = true;
        }
        else if(destinationDis < _distance && _isArrived)
        {
            _rb.velocity = Vector3.zero;
            _dir = (_basePos - transform.position).normalized;
            _isArrived = false;
        }
        transform.forward = _dir + new Vector3(0, transform.position.y, 0);
        _rb.velocity = transform.forward * base.Speed;
    }
}
