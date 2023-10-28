using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    [Header("=====構えのカメラの設定=====")]
    [SerializeField] private SetUpCamera _setUpCameraSetting;

    [Header("===トドメの時のカメラの動き===")]
    [SerializeField] private FinishAttackCamera _finishAttackCamera;
     
    [Header("通常時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _defultCamera;

    [Header("構えの時のカメラ")]
    [SerializeField] private CinemachineVirtualCamera _setUpCamera;

    [Header("通常時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _defultCameraImpulsSource;

    [Header("構え時のカメラ_振動")]
    [SerializeField] private CinemachineImpulseSource _setUpCameraImpulsSource;

    [SerializeField] private PlayerControl _playerControl;

    public CinemachineVirtualCamera DedultCamera => _defultCamera;
    public CinemachineVirtualCamera SetUpCamera => _setUpCamera;

    public SetUpCamera SetUpCameraSetting => _setUpCameraSetting;

    public PlayerControl PlayerControl => _playerControl;




    private void Awake()
    {
        _setUpCameraSetting.Init(this);
        _finishAttackCamera.Init(this, _defultCamera);
    }

    public void UseDefultCamera()
    {
        _setUpCameraSetting.ResetCamera();
        _defultCamera.transform.eulerAngles = _setUpCamera.transform.eulerAngles;
        _defultCamera.Priority = 10;
        _setUpCamera.Priority = 0;
        ShakeCamra(CameraType.Defult, CameraShakeType.ChangeWeapon);
    }

    public void UseSetUpCamera()
    {
        _setUpCameraSetting.ResetCamera();
        _setUpCamera.transform.eulerAngles = _defultCamera.transform.eulerAngles;
        _defultCamera.Priority = 0;
        _setUpCamera.Priority = 10;
        ShakeCamra(CameraType.SetUp, CameraShakeType.ChangeWeapon);
    }

    public void ShakeCamra(CameraType cameraType, CameraShakeType cameraShakeType)
    {


        CinemachineImpulseSource source = default;
        if (cameraType == CameraType.Defult)
        {
            source = _defultCameraImpulsSource;
        }
        else if (cameraType == CameraType.SetUp)
        {
            source = _setUpCameraImpulsSource;
        }
        else
        {
            source = _defultCameraImpulsSource;
        }

        source.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0.2f;
        source.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = 0.2f;

        float setPower = 0;
        if (cameraShakeType == CameraShakeType.ChangeWeapon)
        {
            setPower = 2f;
        }
        else if (cameraShakeType == CameraShakeType.AttackNomal)
        {
            setPower = 1f;
        }
        else
        {
            setPower = 1f;
        }



        source.GenerateImpulse(new Vector3(0, setPower, 0));
    }

}

public enum CameraType
{
    Defult,
    SetUp,
    All,
}

public enum CameraShakeType
{
    ChangeWeapon,
    AttackNomal,
    Kill,
}