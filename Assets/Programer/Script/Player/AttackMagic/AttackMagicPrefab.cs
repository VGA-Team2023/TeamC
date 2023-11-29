using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMagicPrefab : MonoBehaviour, IMagicble
{
    [Header("魔法の属性")]
    [SerializeField] MagickType _magicType = MagickType.Ice;

    [Header("消えるまでの時間")]
    [SerializeField] private float _lifeTime = 7;

    [Header("球の速度")]
    [SerializeField] private float _speed = 10;

    [SerializeField] private Rigidbody _rb;

    /// <summary>攻撃力</summary>
    private float _attackPower = 1;

    /// <summary>魔法のタイプ</summary>
    private AttackType _attackType = AttackType.ShortChantingMagick;

    private Vector3 _moveDir;

    private Transform _enemy;

    private Vector3 _foward;

    public void SetAttack(Transform enemy, Vector3 foward, AttackType attackType, float attackPower)
    {
        _enemy = enemy;
        _foward = foward;
        _attackPower = attackPower;
        _attackType = attackType;

        if (_enemy != null)
        {
            _moveDir = _enemy.position - transform.position;
        }
        else
        {
            _moveDir = _foward;
        }

        var r = Random.Range(1, 1.5f);
        _speed = r * _speed;

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
        //敵がnullではない、溜め魔法の時は追尾する
        if (_attackType == AttackType.LongChantingMagick && _enemy != null)
        {
            _moveDir = _enemy.position - transform.position;
        }
        _rb.velocity = _moveDir.normalized * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IEnemyDamageble>(out IEnemyDamageble damageble);
        damageble?.Damage(_attackType, _magicType, _attackPower);
        Destroy(gameObject);
    }

}
