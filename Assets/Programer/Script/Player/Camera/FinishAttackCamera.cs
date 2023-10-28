using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class FinishAttackCamera
{
    [Header("í èÌÇÃFOV")]
    [SerializeField] private float _defaltFOV = 60;

    [Header("ç≈ëÂÇÃFOV")]
    [SerializeField] private float _maxFOV = 60;

    [Header("ç≈è¨ÇÃFOV")]
    [SerializeField] private float _minFOV = 50;

    [Header("FOV")]
    [SerializeField] private float _changeFOVSpeed = 3;

    [SerializeField] private float _cganngeTime = 0.3f;

    private float _count = 0;
    private float _first;
    private bool _isSet = false;

    private CinemachineVirtualCamera _finishCamera;

    private CameraControl _cameraControl;
    private CinemachineVirtualCamera _defultCamera;

    private CinemachinePOV _finishCameraPov;



    float _set = 0;

    public void Init(CameraControl cameraControl, CinemachineVirtualCamera finishCamera, CinemachineVirtualCamera camera)
    {
        _cameraControl = cameraControl;
        _finishCamera = finishCamera;
        _defultCamera = camera;
        _finishCameraPov = finishCamera.GetCinemachineComponent<CinemachinePOV>();
    }


    public void SetCamera(Vector3 enemyPos)
    {
        _isSet = false;
        _count = 0;
        _first = _finishCameraPov.m_HorizontalAxis.Value;

        Vector3 dir = enemyPos - _cameraControl.PlayerControl.PlayerT.position;
        Quaternion _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        _set = NormalizeAngle(_targetRotation.eulerAngles.y);
    }

    // äpìxÇ-180ìxÇ©ÇÁ180ìxÇ…ê≥ãKâªÇ∑ÇÈä÷êî
    float NormalizeAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }

        while (angle < -180f)
        {
            angle += 360f;
        }

        return angle;
    }


    public void ResetCamera()
    {
        _finishCamera.m_Lens.FieldOfView = _defaltFOV;
    }

    public void DoFinishCamera()
    {
        if (_finishCamera.m_Lens.FieldOfView > _minFOV)
        {
            _finishCamera.m_Lens.FieldOfView -= Time.deltaTime * _changeFOVSpeed;
        }


        if (!_isSet)
        {
            _count += Time.deltaTime;
            _finishCameraPov.m_HorizontalAxis.Value = Mathf.Lerp(_first, _set, _count / _cganngeTime);
            _finishCameraPov.m_VerticalAxis.Value = 0;

            if (_finishCameraPov.m_HorizontalAxis.Value == _set)
            {
                _isSet = true;
            }
        }
    }

    public void EndFinish()
    {
        _finishCamera.m_Lens.FieldOfView = _defaltFOV;
        _defultCamera.m_Lens.FieldOfView = _maxFOV;
        _isSet = false;
    }


}
