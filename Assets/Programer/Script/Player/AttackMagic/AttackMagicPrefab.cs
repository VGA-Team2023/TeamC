using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMagicPrefab : MonoBehaviour, IMagicble, IPause, ISlow, ISpecialMovingPause
{
    [Header("魔法の属性")]
    [SerializeField] MagickType _magicType = MagickType.Ice;

    [Header("消えるまでの時間")]
    [SerializeField] private float _lifeTime = 7;

    [Header("球の速度")]
    [SerializeField] private float _moveSpeed = 10;

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
        _moveSpeed = r * _moveSpeed;

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
        _rb.velocity = _moveDir.normalized * _moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IEnemyDamageble>(out IEnemyDamageble damageble);
        damageble?.Damage(_attackType, _magicType, _attackPower);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SpecialMovingPauseManager.Resume(this);
    }

    private Vector3 _savePauseVelocity = default;
    private Vector3 _saveMoviePauseVelocity = default;
    private float _saveSpeed = default;

    public void Pause()
    {
        _savePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    public void Resume()
    {
        _rb.isKinematic = false;
        _rb.velocity = _savePauseVelocity;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _saveSpeed = _moveSpeed;
        _moveSpeed = _saveSpeed * slowSpeedRate;
    }

    public void OffSlow()
    {
        _moveSpeed = _saveSpeed;
    }

    void ISpecialMovingPause.Pause()
    {
        _saveMoviePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    void ISpecialMovingPause.Resume()
    {
        _rb.isKinematic = false;
        _rb.velocity = _saveMoviePauseVelocity;
    }

}
