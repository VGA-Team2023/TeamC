using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [Header("=====構えのカメラの設定=====")]
    [SerializeField] private DefaultCamera _setUpCameraSetting;

    [Header("===トドメの時のカメラの動き===")]
    [SerializeField] private FinishAttackCamera _finishAttackCamera;

    [Header("通常時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _defultCamera;

    [Header("攻撃溜めのカメラ")]
    [SerializeField] private CinemachineVirtualCamera _attackChargeCamera;

    [Header("トドメの時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _finishCamera;

    [Header("通常時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _defultCameraImpulsSource;

    [Header("攻撃時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _attckChargeCameraImpulsSource;

    [Header("構え時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _setUpCameraImpulsSource;

    [SerializeField] private PlayerControl _playerControl;

    public CinemachineVirtualCamera DedultCamera => _defultCamera;
    public CinemachineVirtualCamera FinishCamera => _finishCamera;

    public DefaultCamera SetUpCameraSetting => _setUpCameraSetting;

    public PlayerControl PlayerControl => _playerControl;

    public FinishAttackCamera FinishAttackCamera => _finishAttackCamera;


    private void Awake()
    {
        _setUpCameraSetting.Init(this, _defultCamera);
        _finishAttackCamera.Init(this, _finishCamera, _defultCamera);
    }

    public void UseAttackChargeCamera()
    {
        _attackChargeCamera.Priority = 10;
        _defultCamera.Priority = 0;
        _finishCamera.Priority = 0;

    }

    public void UseDefultCamera(bool isReset)
    {
        if (isReset)
        {
            _finishAttackCamera.ResetCamera();
        }

        _defultCamera.transform.eulerAngles = _finishCamera.transform.eulerAngles;
        _defultCamera.Priority = 10;
        _finishCamera.Priority = 0;
        //ShakeCamra(CameraType.Defult, CameraShakeType.ChangeWeapon);
    }

    public void UseFinishCamera()
    {
        _finishAttackCamera.ResetCamera();
        _finishCamera.transform.eulerAngles = _defultCamera.transform.eulerAngles;
        _defultCamera.Priority = 0;
        _finishCamera.Priority = 10;
        //ShakeCamra(CameraType.SetUp, CameraShakeType.ChangeWeapon);
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
}