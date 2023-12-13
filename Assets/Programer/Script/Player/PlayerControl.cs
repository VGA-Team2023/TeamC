using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IPlayerDamageble, IPause, ISlow, ISpecialMovingPause
{
    [Header("マウスで操作するかどうか")]
    public bool IsMousePlay = false;

    [Header("@Hp設定")]
    [SerializeField] private PlayerHp _hp;

    [Header("@ダメージ処理設定")]
    [SerializeField] private PlayerDamage _damage;

    [Header("@移動設定")]
    [SerializeField] private PlayerMove _playerMove;

    [Header("@回避")]
    [SerializeField] private PlayerAvoid _avoid;
     
    [Header("@ロックオン")]
    [SerializeField] private PlayerLockOn _lockOn;

    [Header("@攻撃")]
    [SerializeField] private Attack2 _attack2;

    [Header("@とどめ")]
    [SerializeField] private FinishingAttack _finishingAttack;
     
    [Header("設置判定")]
    [SerializeField] private GroundCheck _groundCheck;

    [Header("アニメーション設定")]
    [SerializeField] private PlayerAnimControl _playerAnimControl;

    [Header("属性変更")]
    [SerializeField] private PlayerChangeAttribute _playerChangeAttribute;


    [SerializeField] private ControllerVibrationManager _controllerVibrationManager;
    [Header("カメラ設定")]
    [SerializeField] private CameraControl _cameraControl;
    [Header("プレイヤー自身")]
    [SerializeField] private Transform _playerT;
    [Header("モデル")]
    [SerializeField] private Transform _playerModelT;
    [Header("RigidBody")]
    [SerializeField] private Rigidbody _rigidbody;
    [Header("Animator")]
    [SerializeField] private Animator _anim;
    [Header("Input")]
    [SerializeField] private InputManager _inputManager;
    [Header("音")]
    [SerializeField] private PlayerAudio _audio;
    [Header("HitStop設定")]
    [SerializeField] private HitStopCall _hitStopCall;
    [SerializeField] private HitStopConrol _hitStopConrol;
    [SerializeField] private PlayerStateMachine _stateMachine = default;
    [SerializeField] private ColliderCheck _colliderCheck;
    [SerializeField] private Attack _attack;


    private bool _isNewAttack = true;

    private bool _isPause = false;

    public PlayerChangeAttribute PlayerAttributeControl => _playerChangeAttribute;

    private Vector3 _savePauseVelocity = default;

    public Transform PlayerModelT => _playerModelT;
    public bool IsNewAttack => _isNewAttack;
    public PlayerLockOn LockOn => _lockOn;
    public Attack2 Attack2 => _attack2;
    public HitStopCall HitStopCall => _hitStopCall;
    public PlayerAudio PlayerAudio => _audio;

    public HitStopConrol HitStopConrol => _hitStopConrol;
    public PlayerDamage PlayerDamage => _damage;
    public PlayerHp PlayerHp => _hp;
    public ControllerVibrationManager ControllerVibrationManager => _controllerVibrationManager;
    public CameraControl CameraControl => _cameraControl;
    public PlayerAnimControl PlayerAnimControl => _playerAnimControl;
    public Animator Animator => _anim;
    public Rigidbody Rb => _rigidbody;
    public Transform PlayerT => _playerT;
    public InputManager InputManager => _inputManager;
    public PlayerMove Move => _playerMove;
    public GroundCheck GroundCheck => _groundCheck;
    public Attack Attack => _attack;
    public FinishingAttack FinishingAttack => _finishingAttack;
    public PlayerAvoid Avoid => _avoid;
    public ColliderCheck ColliderCheck => _colliderCheck;
    private void Awake()
    {
        _playerChangeAttribute.Init(this);
        _stateMachine.Init(this);
        _groundCheck.Init(this);
        _playerMove.Init(this);
        _playerAnimControl.Init(this);
        _attack.Init(this);
        _attack2.Init(this);
        _finishingAttack.Init(this);
        _colliderCheck.Init(this);
        _avoid.Init(this);
        _hp.Init(this);
        _damage.Init(this);
        _lockOn.Init(this);
    }

    void Start()
    {

    }

    void Update()
    {
        if (_inputManager.IsPause)
        {
            _isPause = !_isPause;
            GameManager.Instance.PauseManager.PauseResume(_isPause);
        }

        if (GameManager.Instance.PauseManager.IsPause || GameManager.Instance.SpecialMovingPauseManager.IsPaused) return;


        _stateMachine.Update();

        _damage.CountWaitTime();



    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.PauseManager.IsPause || GameManager.Instance.SpecialMovingPauseManager.IsPaused) return;
        _stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.PauseManager.IsPause || GameManager.Instance.SpecialMovingPauseManager.IsPaused) return;
        _stateMachine.LateUpdate();
        _playerAnimControl.AnimSet();
    }


    private void OnDrawGizmosSelected()
    {
        _groundCheck.OnDrawGizmos(PlayerT);
        _attack.ShortChantingMagicAttack.OnDrwowGizmo(PlayerT);
        _attack2.AttackMagic.OnDrwowGizmo(PlayerT);
        _finishingAttack.OnDrwowGizmo(PlayerT);
    }

    public void Damage(float damage)
    {
        _damage.Damage(damage,false,MagickType.Ice);
    }
   public void BossDamage(float damage, MagickType magickType)
    {
        _damage.Damage(damage, true, magickType);
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


    public void Pause()
    {
        _anim.speed = 0;

        _savePauseVelocity = _rigidbody.velocity;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
    }

    public void Resume()
    {
        _anim.speed = 1;

        _rigidbody.isKinematic = false;
        _rigidbody.velocity = _savePauseVelocity;
    }

    public void OnSlow(float slowSpeedRate)
    {
        if (_anim == null) return;
        _anim.speed = slowSpeedRate;
    }

    public void OffSlow()
    {
        if (_anim == null) return;
        _anim.speed = 1;
    }

    void ISpecialMovingPause.Pause()
    {
        _anim.speed = 0;

        _savePauseVelocity = _rigidbody.velocity;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
    }

    void ISpecialMovingPause.Resume()
    {
        _anim.speed = 1;

        _rigidbody.isKinematic = false;
        _rigidbody.velocity = _savePauseVelocity;
    }

 
}