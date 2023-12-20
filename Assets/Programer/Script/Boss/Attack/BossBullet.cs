using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAudio;

public class BossBullet : MonoBehaviour, IPause, ISlow, ISpecialMovingPause
{
    [Header("属性")]
    [SerializeField] private MagickType _magickType;

    [Header("弾速")]
    [SerializeField] private float _moveSpeed = 10f;

    [Header("LifeTime")]
    [SerializeField] private float _liftTime = 7f;

    [SerializeField] private Rigidbody _rb;

    private float _attackPower;

    private float _countLifeTime = 0;

    private bool _isDestroy = false;

    public void Init(float attackPower)
    {
        _attackPower = attackPower;
        _setMoveSpeed = _moveSpeed;
        _rb.velocity = transform.forward * _moveSpeed;
    }

    private void Awake()
    {
        //音源の更新
        AudioSet(PlayMagicAudioType.Play);
    }

    private void Update()
    {
        //音源の更新
        if (!_isDestroy)
        {
            AudioSet(PlayMagicAudioType.Updata);
        }

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
            _isDestroy = true;
            //音源の更新
            AudioSet(PlayMagicAudioType.Stop);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isDestroy = true;

            other.gameObject.TryGetComponent<IPlayerDamageble>(out IPlayerDamageble player);
            player?.Damage(_attackPower);
            //音源の更新
            AudioSet(PlayMagicAudioType.Stop);
            Destroy(gameObject);
        }
    }

    /// <summary>音を流す</summary>
    /// <param name="isPlay"></param>
    private void AudioSet(PlayMagicAudioType audioType)
    {
        SEState state = default;

        //属性に応じて鳴らす音を分ける
        if (_magickType == MagickType.Ice)
        {
            state = SEState.EnemyBossTrailIce;
        }
        else
        {
            state = SEState.EnemyBossTrailGrass;
        }

        //音の再生方法に応じて分ける
        if (audioType == PlayMagicAudioType.Play)
        {
            AudioController.Instance.SE.Play3D(state, transform.position);
        }
        else if (audioType == PlayMagicAudioType.Stop)
        {
            AudioController.Instance.SE.Stop(state);
        }
        else if (audioType == PlayMagicAudioType.Updata)
        {
            AudioController.Instance.SE.Update3DPos(state, transform.position);
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
