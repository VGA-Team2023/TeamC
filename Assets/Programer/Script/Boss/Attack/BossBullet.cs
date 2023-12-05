using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _p = new List<ParticleSystem>();

    [Header("’e‘¬")]
    [SerializeField] private float _moveSpeed = 10f;

    [Header("LifeTime")]
    [SerializeField] private float _liftTime = 7f;

    [SerializeField] private Rigidbody _rb;

    private float _attackPower;

    private float _countLifeTime = 0;


    public void Init(float attackPower)
    {
        _attackPower = attackPower;
        _rb.velocity = transform.forward * _moveSpeed;
    }

    private void Awake()
    {

    }

    private void Update()
    {
        CountLifeTime();
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * _moveSpeed;
    }

    public void CountLifeTime()
    {
        _liftTime += Time.deltaTime;

        if (_liftTime < _countLifeTime)
        {
            Destroy(gameObject);
        }

    }

}
