using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMagicPrefab : MonoBehaviour, IMagicble, IPause, ISlow, ISpecialMovingPause
{
    [Header("魔法の属性")]
    [SerializeField] MagickType _magicType = MagickType.Ice;

    [Header("弾のTransform")]
    [SerializeField] private Transform _bullet;

    [Header("弾の回転速度")]
    [SerializeField] private float _bulletRotateSpeed = 200;

    [Header("消えるまでの時間")]
    [SerializeField] private float _lifeTime = 7;

    [Header("球の速度")]
    [SerializeField] private float _moveSpeed = 10;

    [Header("ホーミング時の弾の速度")]
    [SerializeField] private float _hormingSpeed = 70f;

    [Header("放物線の最大高さ")]
    [SerializeField] private float height = 5f;

    [Header("目的地修正に入る距離")]
    [SerializeField] private float _hormingDis = 10f;

    [Header("引き寄せを開始する距離")]
    [SerializeField] private float attractionDistance = 10f;

    [Header("引き寄せの強さ")]
    [SerializeField] private float attractionStrength = 2f;

    [Header("爆発のエフェクト")]
    [SerializeField] private GameObject _effect;


    /// <summary>初回の進行速度</summary>
    private Vector3 _initialDirection;

    /// <summary>半分の距離 </summary>
    private float _disHarf;

    /// <summary>引き寄せられる</summary>
    private bool _isAttracted = false;
    /// <summary>引き寄せを中断 </summary>
    private bool _isAttractedEnd = false;
    /// <summary>初回から引き寄せ</summary>
    private bool _isHormig = false;

    private bool _isNear = false;

    private Vector3 _saveDir = Vector3.zero;

    [SerializeField] private Rigidbody _rb;

    /// <summary>攻撃力</summary>
    private float _attackPower = 1;

    /// <summary>魔法のタイプ</summary>
    private AttackType _attackType = AttackType.ShortChantingMagick;

    private float _countLifeTime = 0;

    private Vector3 _moveDir;

    private Transform _enemy;

    private Vector3 _foward;

    private bool _isEnter = false;

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

        if (attackType == AttackType.Horming)
        {
            _initialDirection = transform.forward;
            _moveSpeed = _hormingSpeed;

            // オブジェクトBとの距離を計算
            float distanceToB = Vector3.Distance(transform.position, _enemy.position);

            // 一定距離以内で引き寄せられる
            if (distanceToB < attractionDistance)
            {
                transform.forward = _enemy.position - transform.position;
                _isHormig = true;
            }
            else
            {
                Vector3 v1 = transform.position;
                Vector3 v2 = _enemy.position;
                v1.y = 0;
                v2.y = 0;

                _disHarf = Vector3.Distance(_enemy.position, v1) / 2;
            }
        }

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
            _rb.velocity = _moveDir.normalized * _moveSpeed;
        }
        else if (_attackType == AttackType.Horming)
        {
            // オブジェクトBとの距離を計算
            Vector3 v1 = transform.position;
            Vector3 v2 = _enemy.position;

            //過激範囲に入ったら破裂
            if (Vector3.Distance(v1, v2) < 3f)
            {
                var go = Instantiate(_effect);
                go.transform.position = transform.position;
                Hit();
            }
            else if (!_isNear && Vector3.Distance(v1, v2) < 10f)
            {
                _isNear = true;
            }
            else if (_isNear && Vector3.Distance(v1, v2) > 10f)
            {
                var go = Instantiate(_effect);
                go.transform.position = transform.position;
                Hit();
            }

            v1.y = 0;
            v2.y = 0;
            float distanceToB = Vector3.Distance(v1, v2);



            // 一定距離内に入ったら、引き寄せを始める
            if (distanceToB < _disHarf)
            {
                _isAttracted = true;
            }


            // 直線移動または引き寄せ移動を処理
            if (_isHormig)
            {
                // 直線移動
                _rb.velocity = transform.forward * _hormingSpeed;
            }
            else if (_isAttractedEnd)
            {
                // 直線移動
                _rb.velocity = _saveDir;
            }
            else if (_isAttracted)
            {
                // オブジェクトBの方向へのベクトルを計算
                Vector3 directionToB = (_enemy.position - transform.position).normalized;

                // 直線移動の方向と引き寄せ方向を合成
                Vector3 combinedDirection = (_initialDirection + directionToB * attractionStrength).normalized;

                //回転
                Quaternion r = Quaternion.RotateTowards(_bullet.rotation, Quaternion.Euler(directionToB), Time.deltaTime * _bulletRotateSpeed);
                _bullet.rotation = r;

                // オブジェクトAを移動
                _rb.velocity = combinedDirection.normalized * _hormingSpeed;

                _saveDir = _rb.velocity;
            }
            else
            {
                // 直線移動
                _rb.velocity = transform.forward * _hormingSpeed;
            }
        }
        else
        {
            _rb.velocity = transform.forward * _moveSpeed;
        }
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

    void Hit()
    {
        AudioSet(PlayMagicAudioType.Stop);
        _isEnter = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isEnter) return;
        other.gameObject.TryGetComponent<IEnemyDamageble>(out IEnemyDamageble damageble);
        damageble?.Damage(_attackType, _magicType, _attackPower);
        Hit();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isEnter) return;
        other.gameObject.TryGetComponent<IEnemyDamageble>(out IEnemyDamageble damageble);
        damageble?.Damage(_attackType, _magicType, _attackPower);
        AudioSet(PlayMagicAudioType.Stop);
        _isEnter = true;
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
