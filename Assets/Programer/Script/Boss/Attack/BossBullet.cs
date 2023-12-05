using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour, IPause, ISlow, ISpecialMovingPause
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
        _setMoveSpeed = _moveSpeed;
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
        if (_isPause || _isMoviePause) return;
        _rb.velocity = transform.forward * _moveSpeed;
    }

    public void CountLifeTime()
    {
        if (_isPause || _isMoviePause) return;

        _liftTime += Time.deltaTime;

        if (_liftTime < _countLifeTime)
        {
            Destroy(gameObject);
        }

    }

    private bool _isPause = false;
    private bool _isMoviePause = false;
    private float _setMoveSpeed = 10f;
    private Vector3 _savePauseVelocity = default;
    private Vector3 _saveMoviePauseVelocity = default;

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        GameManager.Instance.SpecialMovingPauseManager.Resume(this);
    }


    public void Pause()
    {
        _isPause = true;

        _savePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    public void Resume()
    {
        _isPause = false;

        _rb.isKinematic = false;
        _rb.velocity = _savePauseVelocity;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _setMoveSpeed = _moveSpeed * slowSpeedRate;
    }

    public void OffSlow()
    {
        _setMoveSpeed = _moveSpeed;
    }

    void ISpecialMovingPause.Pause()
    {
        _isMoviePause = true;

        _saveMoviePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    void ISpecialMovingPause.Resume()
    {
        _isMoviePause = false;

        _rb.isKinematic = false;
        _rb.velocity = _saveMoviePauseVelocity;
    }
}
