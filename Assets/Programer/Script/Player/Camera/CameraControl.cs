using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.Rendering;

public class CameraControl : MonoBehaviour, IPause, ISlow, ISpecialMovingPause
{
    [Header("=====構えのカメラの設定=====")]
    [SerializeField] private DefaultCamera _setUpCameraSetting;

    [Header("===トドメの時のカメラの動き===")]
    [SerializeField] private FinishAttackCamera _finishAttackCamera;

    [Header("==攻撃時のカメラ設定==")]
    [SerializeField] private AttackCamera _attackCamera;

    [Header("カメラ感度設定")]
    [SerializeField] private CameraSpeed _cameraSpeed;

    [Header("通常時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _defultCamera;

    [Header("攻撃溜めのカメラ")]
    [SerializeField] private CinemachineVirtualCamera _attackChargeCamera;

    [Header("トドメの時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _finishCamera;

    [Header("トドメ入りたてのカメラ")]
    [SerializeField] private GameObject _finishFirstCamera;

    [Header("通常時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _defultCameraImpulsSource;

    [Header("攻撃時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _attckChargeCameraImpulsSource;

    [Header("構え時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _setUpCameraImpulsSource;

    [SerializeField] private PlayerControl _playerControl;

    private float _count = 0;

    private bool _isFinish = false;

    private Animator _anim;

    public CinemachineVirtualCamera DedultCamera => _defultCamera;
    public CinemachineVirtualCamera FinishCamera => _finishCamera;

    public AttackCamera AttackCamera => _attackCamera; 
    public DefaultCamera SetUpCameraSetting => _setUpCameraSetting;
    public PlayerControl PlayerControl => _playerControl;

    public FinishAttackCamera FinishAttackCamera => _finishAttackCamera;


    private void Awake()
    {
        _cameraSpeed.Init(this, _defultCamera, _attackChargeCamera, _finishCamera);
        _setUpCameraSetting.Init(this, _defultCamera);
        _finishAttackCamera.Init(this, _finishCamera, _defultCamera);
        _attackCamera.Init(this, _attackChargeCamera);
        SetCameraSpeed(OptionValueRecorder.Instance.CameraSensitivity);
        _anim = _finishFirstCamera.GetComponent<Animator>();
    }

    /// <summary>カメラ感度を設定する</summary>
    public void SetCameraSpeed(float speed)
    {
        _cameraSpeed.ChangeCameraSpeed(speed);
    }

    public void UseAttackChargeCamera()
    {
        _attackCamera.UseCamera();

        _attackChargeCamera.Priority = 10;
        _defultCamera.Priority = 0;
        _finishCamera.Priority = 0;
    }

    public void UseAvoidCamera()
    {

    }



    public void UseDefultCamera(bool isReset)
    {
        if (isReset)
        {
            _finishAttackCamera.ResetCamera();
        }
        _isFinish = false;
        _defultCamera.transform.eulerAngles = _finishCamera.transform.eulerAngles;
        _defultCamera.Priority = 10;
        _attackChargeCamera.Priority = 0;
        _finishFirstCamera.SetActive(false);
        _finishCamera.Priority = 0;
        //ShakeCamra(CameraType.Defult, CameraShakeType.ChangeWeapon);
    }

    public void UseFinishCamera()
    {
        _finishAttackCamera.ResetCamera();

        // カメラAの向いている方向を取得
        Vector3 direction = _defultCamera.State.FinalOrientation * Vector3.forward;

        // カメラBのAimターゲットをカメラAの向いている方向に設定
        _finishCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = direction.x;
        _finishCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = direction.y;

        _isFinish = true;
        _count = 0;
        _defultCamera.Priority = 0;
        _finishFirstCamera.SetActive(true);
        // _finishCamera.Priority = 10;


        //ShakeCamra(CameraType.SetUp, CameraShakeType.ChangeWeapon);
    }

    public void UPP()
    {
        if (_isFinish)
        {
            _count += Time.deltaTime;

            if (_count > 0.6f)
            {
                _count = 0;
                _isFinish = false;
                _finishFirstCamera.SetActive(false);
                _finishCamera.Priority = 11;
            }
        }
    }


    public void LockOnCamera()
    {

    }

    public void ShakeCamra(CameraType cameraType, CameraShakeType cameraShakeType)
    {
        CinemachineImpulseSource source = default;
        if (cameraType == CameraType.Defult)
        {
            source = _defultCameraImpulsSource;
        }
        else if (cameraType == CameraType.FinishCamera)
        {
            source = _setUpCameraImpulsSource;
        }
        else
        {
            source = _attckChargeCameraImpulsSource;
        }

        source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.2f;
        source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.2f;

        float setPowerX = 0;
        float setPowerY = 0;
        float setPowerZ = 0;


        if (cameraShakeType == CameraShakeType.ChangeWeapon)
        {
            setPowerY = 2f;
        }
        else if (cameraShakeType == CameraShakeType.AttackNomal)
        {
            setPowerY = 1f;
        }
        else if (cameraShakeType == CameraShakeType.Damage)
        {
            setPowerY = 4f;
            source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 1;
            source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 1f;
        }
        else if (cameraShakeType == CameraShakeType.EndFinishAttack)
        {
            setPowerX = 5f;
            setPowerY = 5f;
            setPowerZ = 0f;
            source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 1;
            source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 1f;
        }
        else
        {
            setPowerY = 1f;
        }

        source.GenerateImpulse(new Vector3(setPowerX, setPowerY, setPowerZ));
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
        _cameraSpeed.Pause();
        _anim.speed = 0;
    }

    public void Resume()
    {
        _cameraSpeed.Resume();
        _anim.speed = 1;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _cameraSpeed.OnSlow(slowSpeedRate);
    }

    public void OffSlow()
    {
        _cameraSpeed.OffSlow();
    }

    void ISpecialMovingPause.Pause()
    {
        _cameraSpeed.ISpecialMovingPausePause();
        _anim.speed = 0;
    }

    void ISpecialMovingPause.Resume()
    {
        _cameraSpeed.ISpecialMovingPauseResume();
        _anim.speed = 1;
    }

}

public enum CameraType
{
    Defult,
    AttackCharge,
    FinishCamera,
    All,
}

public enum CameraShakeType
{
    ChangeWeapon,
    AttackNomal,
    Kill,
    EndFinishAttack,
    Damage,
}