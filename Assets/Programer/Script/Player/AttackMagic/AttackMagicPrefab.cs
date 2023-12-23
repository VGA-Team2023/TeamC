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

    private float _countLifeTime = 0;

    private Vector3 _moveDir;

    private Transform _enemy;

    private Vector3 _foward;

    enum PlayMagicAudioType
    {
        Play,
        Stop,
        Updata,
    }

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



        AudioSet(PlayMagicAudioType.Play);
    }



    void Update()
    {
        _countLifeTime += Time.deltaTime;

        if (_countLifeTime > _lifeTime)
        {
            AudioSet(PlayMagicAudioType.Stop);
            Destroy(gameObject);
            return;
        }

        //音源の更新
        AudioSet(PlayMagicAudioType.Updata);
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
            AudioSet(PlayMagicAudioType.Stop);
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

    /// <summary>音を流す</summary>
    /// <param name="isPlay"></param>
    private void AudioSet(PlayMagicAudioType audioType)
    {
        SEState state = default;

        //属性に応じて鳴らす音を分ける
        if (_magicType == MagickType.Ice)
        {
            if (_attackType == AttackType.ShortChantingMagick)
            {
                state = SEState.PlayerTrailIcePatternA;
            }
            else
            {
                state = SEState.PlayerTrailIcePatternB;
            }
        }
        else
        {
            if (_attackType == AttackType.ShortChantingMagick)
            {
                state = SEState.PlayerTrailGrassPatternA;
            }
            else
            {
                state = SEState.PlayerTrailGrassPatternB;
            }
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


    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IEnemyDamageble>(out IEnemyDamageble damageble);
        damageble?.Damage(_attackType, _magicType, _attackPower);
        AudioSet(PlayMagicAudioType.Stop);
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
        GameManager.Instance.SlowManager.Remove(this);
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
        if (_saveSpeed == 0)
        {
            _moveSpeed = 40;
            return;
        }
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
