using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BossControl : EnemyBase, IEnemyDamageble, IFinishingDamgeble, IPause, ISlow, ISpecialMovingPause
{
    [Header("ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー")]

    [Header("反対属性でのみ攻撃が通るかどうか")]
    [SerializeField] private bool _isInDamageOtherAttribute = false;

    [Header("敵の属性")]
    [SerializeField] private PlayerAttribute _enemyAttribute;

    [Header("Hpの設定")]
    [SerializeField] private BossHp _hpControl;

    [Header("倒された時の設定")]
    [SerializeField] private BossDeath _death;

    [Header("移動設定")]
    [SerializeField] private BossMove _move;

    [Header("回転設定")]
    [SerializeField] private BossRotate _rotate;

    [Header("攻撃")]
    [SerializeField] private BossAttack _bossAttack;

    [SerializeField] private BossAnimControl _animControl;

    [Header("BossのTransform")]
    [SerializeField] private Transform _bossT;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private Animator _anim;

    [SerializeField] private GameObject _deathCamera;

    [SerializeField] private BossStateMachine _state;
    private Transform _player;

    private bool _isDeath = false;

    public BossDeath BossDeath => _death;
    public bool IsDeath => _isDeath;
    public Rigidbody Rigidbody => _rb;
    public BossAnimControl BossAnimControl => _animControl;
    public Animator Animator => _anim;
    public BossHp BossHp => _hpControl;
    public BossMove Move => _move;
    public BossRotate BossRotate => _rotate;
    public BossAttack BossAttack => _bossAttack;
    public PlayerAttribute EnemyAttibute => _enemyAttribute;
    public Transform PlayerT => _player;
    public Transform BossT => _bossT;

    private void Awake()
    {
        _player = GameObject.FindObjectOfType<PlayerControl>().gameObject.transform;
        _state.Init(this);
        _bossAttack.Init(this);
        _hpControl.Init(this);
        _move.Init(this);
        _rotate.Init(this);
        _animControl.Init(this);
        _death.Init(this);
    }
    void Start()
    {

    }

    private void Update()
    {
        if (_isPause || _isMoviePause) return;


        //ゲーム中でなかったら何もしない
        if (!GameManager.Instance.IsGameMove) return;

        _state.Update();

        if (_isDeath)
        {
            if (_death.CountDestroyTime())
            {
                Debug.Log("Destroys");
                _deathCamera.SetActive(false);
                EnemyFinish();
                Destroy(gameObject);
            }
        }

    }


    private void FixedUpdate()
    {
        if (_isPause || _isMoviePause) return;

        //ゲーム中でなかったら何もしない
        if (!GameManager.Instance.IsGameMove) return;
        _state.FixedUpdate();
    }


    private void LateUpdate()
    {
        if (_isPause || _isMoviePause) return;

        //ゲーム中でなかったら何もしない
        if (!GameManager.Instance.IsGameMove) return;

        _state.LateUpdate();
    }



    private void OnDrawGizmosSelected()
    {
        _move.OnDrwowGizmo(_bossT);
    }

    public void Damage(AttackType attackType, MagickType attackHitTyp, float damage)
    {
        if (_isInDamageOtherAttribute)
        {
            if (_enemyAttribute == PlayerAttribute.Ice)
            {
                if (attackHitTyp == MagickType.Ice)
                {
                    _hpControl.Damage(damage, attackHitTyp);
                }
                return;
            }
            else
            {
                if (attackHitTyp == MagickType.Ice)
                {
                    _hpControl.Damage(damage, attackHitTyp);
                }
                return;
            }
        }
        else
        {
            _hpControl.Damage(damage, attackHitTyp);
        }
    }

    public void StartFinishing()
    {
        _hpControl.StartFinishAttack();
    }

    public void StopFinishing()
    {
        _hpControl.StopFinishAttack();
    }

    public void EndFinishing(MagickType attackHitTyp)
    {
        _isDeath = _hpControl.CompleteFinishAttack(attackHitTyp);

        if(_isDeath)
        {
            _deathCamera.SetActive(true);
        }


        if (_enemyAttribute == PlayerAttribute.Ice)
        {
            _enemyAttribute = PlayerAttribute.Grass;
        }
        else
        {
            _enemyAttribute = PlayerAttribute.Ice;
        }

    }


    private bool _isPause = false;
    private bool _isMoviePause = false;
    private float _setMoveSpeed = 10f;
    private Vector3 _savePauseVelocity = default;
    private Vector3 _saveMoviePauseVelocity = default;
    private float _animSpeedPause = 1;
    private float _animSpeedMoviePause = 1;

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

        //アニメーション
        _animSpeedPause = _anim.speed;
        _anim.speed = 0;

        _savePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    public void Resume()
    {
        _isPause = false;

        //アニメーション
        _anim.speed = _animSpeedPause;

        _rb.isKinematic = false;
        _rb.velocity = _savePauseVelocity;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _anim.speed = slowSpeedRate;
    }

    public void OffSlow()
    {
        _anim.speed = 1;
    }

    void ISpecialMovingPause.Pause()
    {
        _isMoviePause = true;

        //アニメーション
        _animSpeedMoviePause = _anim.speed;
        _anim.speed = 0;

        _saveMoviePauseVelocity = _rb.velocity;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
    }

    void ISpecialMovingPause.Resume()
    {
        _isMoviePause = false;

        //アニメーション
        _anim.speed = _animSpeedMoviePause;

        _rb.isKinematic = false;
        _rb.velocity = _saveMoviePauseVelocity;
    }
}
