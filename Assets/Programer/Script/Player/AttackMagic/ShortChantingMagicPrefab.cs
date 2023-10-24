using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortChantingMagicPrefab : MonoBehaviour, IMagicble
{
    [SerializeField] private float _lifeTime = 7;

    [SerializeField] private float _speed = 10;

    [SerializeField] private Rigidbody _rb;

    private Transform _enemy;

    private Vector3 _foward;

    public void SetEnemy(Transform enemy, Vector3 foward)
    {
        _enemy = enemy;
        _foward = foward;
        Destroy(gameObject, _lifeTime);
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
        CheckDistance();
    }

    public void CheckDistance()
    {
        if (_enemy == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, _enemy.position) < 0.2f)
        {
            Destroy(gameObject);
        }
    }

    public void Move()
    {
        Vector3 dir = _foward;
        if (_enemy != null)
        {
            dir = _enemy.position - transform.position;
        }
        _rb.velocity = dir.normalized * _speed;
    }


}
