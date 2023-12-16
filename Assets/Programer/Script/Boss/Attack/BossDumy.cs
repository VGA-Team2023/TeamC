using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDumy : MonoBehaviour, IEnemyDamageble, IPause, ISlow, ISpecialMovingPause
{
    [Header("体力")]
    [SerializeField] private float _hp;

    [Header("破壊されてから消えるまでの時間")]
    [SerializeField] private float _destroyTime = 3;

    [Header("ライフタイム")]
    [SerializeField] private float _liftTime = 10f;

    private float _countLifeTime = 0;

    [Header("回転速度")]
    [SerializeField] private float _rotateSpeed = 100f;

    [Header("倒された後のレイヤー")]
    [SerializeField] private int _deathLayer = 0;

    [Header("消える時に出すエフェクト")]
    [SerializeField] private GameObject _deathEffect;

    [Header("トラップ_")]
    [SerializeField] private GameObject _trapBulletIce;
    [Header("トラップのOffset_")]
    [SerializeField] private Vector3 _trapOffsetIce;

    [Header("ダメージエフェクト_氷")]
    [SerializeField] private List<ParticleSystem> _damageEffectIce = new List<ParticleSystem>();

    [Header("ダメージエフェクト_草")]
    [SerializeField] private List<ParticleSystem> _damageEffectGrass = new List<ParticleSystem>();

    [SerializeField] private Animator _anim;

    private bool _isDead = false;

    private float _countTime = 0;

    /// <summary>向くべき回転方向</summary>
    Quaternion _targetRotation;

    private MagickType _magickType;

    private Transform _playerT;


    private void Awake()
    {
        _playerT = GameObject.FindObjectOfType<PlayerControl>().gameObject.transform;
    }


    void Update()
    {
        if (_isPause || _isMoviePause) return;

        if (!_isDead)
        {
            _countLifeTime += Time.deltaTime;

            if (_countLifeTime > _liftTime)
            {
                _isDead = true;
                gameObject.layer = _deathLayer;
                var deathEffect = Instantiate(_deathEffect);
                deathEffect.transform.position = transform.position;
            }
        }

        if (_isDead)
        {
            _countTime += Time.deltaTime;

            if (_countTime > _destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }


    private void FixedUpdate()
    {
        if (_isPause || _isMoviePause) return;

        if (_isDead) return;
        RotateSet();
    }

    /// <summary>回転設定</summary>
    public void RotateSet()
    {
        Vector3 dir = _playerT.position - transform.position;
        dir.y = 0;
        _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotateSpeed);
    }
    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        _hp -= damage;

        if (attackHitTyp == MagickType.Ice)
        {
            _damageEffectIce.ForEach(i => i.Play());
        }
        else
        {
            _damageEffectGrass.ForEach(i => i.Play());
        }

        if (_hp <= 0)
        {
            _isDead = true;

            var go = Instantiate(_trapBulletIce);
            go.transform.position = transform.position+ _trapOffsetIce;

            var deathEffect = Instantiate(_deathEffect);
            deathEffect.transform.position = transform.position;

            gameObject.layer = _deathLayer;
        }
    }


    private bool _isPause = false;
    private bool _isMoviePause = false;
    private float _saveAnimSpeedPause = 0;
    private float _saveAnimSpeedMoviePause = 0;


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



    public void OffSlow()
    {
        _anim.speed = 1;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _anim.speed = slowSpeedRate;
    }

    public void Pause()
    {
        _isPause = true;
        _saveAnimSpeedPause = _anim.speed;
    }

    public void Resume()
    {
        _isPause = false;
        _anim.speed = _saveAnimSpeedPause;
    }

    void ISpecialMovingPause.Pause()
    {
        _isMoviePause = true;
        _saveAnimSpeedMoviePause = _anim.speed;
    }

    void ISpecialMovingPause.Resume()
    {
        _isMoviePause = false;
        _anim.speed = _saveAnimSpeedMoviePause;
    }


}
