using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class FinishAttackCamera
{
    [Header("通常のFOV")]
    [SerializeField] private float _defaltFOV = 60;

    [Header("最大のFOV")]
    [SerializeField] private float _maxFOV = 60;

    [Header("最小のFOV")]
    [SerializeField] private float _minFOV = 50;

    [Header("FOV")]
    [SerializeField] private float _changeFOVSpeed = 3;

    [Header("最大Dutch")]
    [SerializeField] private float _maxDutch = 20;

    [SerializeField] private float _changeTime = 0.3f;

    private float _count = 0;

    private float _countFinishTime = 0;

    private float _first;
    private bool _isSetFOV = false;
    private bool _isSetDutch = false;


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

    // 角度を-180度から180度に正規化する関数
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

    public void DoChangeDutch()
    {
        _countFinishTime += Time.deltaTime;

        if (!_isSetDutch)
        {
            _finishCamera.m_Lens.Dutch = Mathf.Lerp(0, _maxDutch, _countFinishTime / _cameraControl.PlayerControl.FinishingAttack.FinishingAttackShort.FinishTime);
            if (_finishCamera.m_Lens.Dutch == _maxDutch)
            {
                _isSetDutch = true;
            }
        }

    }


    /// <summary>トドメをさす際にカメラを敵に向ける。その初期設定</summary>
    /// <param name="enemyPos"></param>
    public void SetCameraFOVStartFinish(Vector3 enemyPos)
    {
        _isSetFOV = false;
        _isSetDutch = false;
        _count = 0;
        _countFinishTime = 0;
        _first = _finishCameraPov.m_HorizontalAxis.Value;


        Vector3 dir = enemyPos - _cameraControl.PlayerControl.PlayerT.position;
        Quaternion _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        _set = NormalizeAngle(_targetRotation.eulerAngles.y);
    }

    /// <summary>トドメをさす際にカメラを敵に向ける。その実行</summary>
    public void DoFinishCameraSettingFirst()
    {
        if (_finishCamera.m_Lens.FieldOfView > _minFOV)
        {
            _finishCamera.m_Lens.FieldOfView -= Time.deltaTime * _changeFOVSpeed;
        }   //カメラの範囲を広げる



        if (!_isSetFOV)
        {
            _count += Time.deltaTime;

            _finishCameraPov.m_HorizontalAxis.Value = Mathf.Lerp(_first, _set, _count / _changeTime);
            _finishCameraPov.m_VerticalAxis.Value = 0;

            if (_finishCameraPov.m_HorizontalAxis.Value == _set)
            {
                _isSetFOV = true;
            }
        }
    }


    public void ResetCamera()
    {
        _finishCamera.m_Lens.FieldOfView = _defaltFOV;
    }


    public void EndFinish()
    {
        _finishCamera.m_Lens.FieldOfView = _defaltFOV;
        _defultCamera.m_Lens.FieldOfView = _maxFOV;
        _finishCamera.m_Lens.Dutch = 0;
        _isSetFOV = false;
    }


}
