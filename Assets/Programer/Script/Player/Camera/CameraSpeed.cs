using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraSpeed
{
    private CameraControl _cameraControl;

    private CinemachineVirtualCamera _defultCamera;

    //現在使用中の速度
    private Vector2 _setSpeedDefultCamera;
    private Vector2 _setSpeedAttackCamera;
    private Vector2 _setSpeedFinishAttackCamera;

    //基本の速度
    private Vector2 _defultSpeedDefultCamera;
    private Vector2 _defultSpeedAttackCamera;
    private Vector2 _defultSpeedFinishAttackCamera;

    //感度設定を考慮した速度
    private Vector2 _toSpeedDefultCamera;
    private Vector2 _toSpeedAttackCamera;
    private Vector2 _toSpeedFinishAttackCamera;


    private Vector2 _pauseSpeedDefultCamera;
    private Vector2 _slowSpeedAttackCamera;
    private Vector2 _slowSpeedFinishAttackCamera;

    private CinemachinePOV _defultCameraPOV;
    private CinemachinePOV _attackCameraPOV;
    private CinemachinePOV _finishAttackCameraPOV;

    private bool _isSlow = false;

    public void Init(CameraControl cameraControl, CinemachineVirtualCamera defultCamera, CinemachineVirtualCamera attackCamera, CinemachineVirtualCamera finishCamera)
    {
        _cameraControl = cameraControl;
        _defultCamera = defultCamera;
        _defultCameraPOV = defultCamera.GetCinemachineComponent<CinemachinePOV>();
        _attackCameraPOV = attackCamera.GetCinemachineComponent<CinemachinePOV>();
        _finishAttackCameraPOV = finishCamera.GetCinemachineComponent<CinemachinePOV>();

        _defultSpeedDefultCamera = new Vector2(_defultCameraPOV.m_HorizontalAxis.m_MaxSpeed, _defultCameraPOV.m_VerticalAxis.m_MaxSpeed);
        _defultSpeedAttackCamera = new Vector2(_attackCameraPOV.m_HorizontalAxis.m_MaxSpeed, _attackCameraPOV.m_VerticalAxis.m_MaxSpeed);
        _defultSpeedFinishAttackCamera = new Vector2(_finishAttackCameraPOV.m_HorizontalAxis.m_MaxSpeed, _finishAttackCameraPOV.m_VerticalAxis.m_MaxSpeed);


        _toSpeedDefultCamera = new Vector2(_defultSpeedDefultCamera.x, _defultSpeedDefultCamera.y);
        _toSpeedAttackCamera = new Vector2(_defultSpeedAttackCamera.x, _defultSpeedAttackCamera.y);
        _toSpeedFinishAttackCamera = new Vector2(_defultSpeedFinishAttackCamera.x, _defultSpeedFinishAttackCamera.y);

        _setSpeedDefultCamera = new Vector2(_toSpeedDefultCamera.x, _toSpeedDefultCamera.y);
        _setSpeedFinishAttackCamera = new Vector2(_toSpeedFinishAttackCamera.x, _toSpeedFinishAttackCamera.y);
        _setSpeedAttackCamera = new Vector2(_toSpeedAttackCamera.x, _toSpeedAttackCamera.y);
    }

    public void ChangeCameraSpeed(float speed)
    {
        _toSpeedDefultCamera = new Vector2(_defultSpeedDefultCamera.x, _defultSpeedDefultCamera.y);
        _toSpeedAttackCamera = new Vector2(_defultSpeedAttackCamera.x, _defultSpeedAttackCamera.y);
        _toSpeedFinishAttackCamera = new Vector2(_defultSpeedFinishAttackCamera.x, _defultSpeedFinishAttackCamera.y);
    }

    public void Pause()
    {
        //カメラの視点変更を不可にする
        _defultCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        _defultCameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
        _attackCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        _attackCameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
        _finishAttackCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        _finishAttackCameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
    }

    public void Resume()
    {
        _defultCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedDefultCamera.x;
        _defultCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedDefultCamera.y;
        _attackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedAttackCamera.x;
        _attackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedAttackCamera.y;
        _finishAttackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.x;
        _finishAttackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.y;
    }

    public void OnSlow(float slowSpeedRate)
    {
        _isSlow = true;

        _setSpeedDefultCamera = new Vector2(_toSpeedDefultCamera.x * slowSpeedRate, _toSpeedDefultCamera.y * slowSpeedRate);
        _setSpeedFinishAttackCamera = new Vector2(_toSpeedFinishAttackCamera.x * slowSpeedRate, _toSpeedFinishAttackCamera.y * slowSpeedRate);
        _setSpeedAttackCamera = new Vector2(_toSpeedAttackCamera.x * slowSpeedRate, _toSpeedAttackCamera.y * slowSpeedRate);

        _defultCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedDefultCamera.x;
        _defultCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedDefultCamera.y;
        _attackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedAttackCamera.x;
        _attackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedAttackCamera.y;
        _finishAttackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.x;
        _finishAttackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.y;
    }

    public void OffSlow()
    {
        _isSlow = false;

        _setSpeedDefultCamera = new Vector2(_toSpeedDefultCamera.x, _toSpeedDefultCamera.y);
        _setSpeedFinishAttackCamera = new Vector2(_toSpeedFinishAttackCamera.x, _toSpeedFinishAttackCamera.y);
        _setSpeedAttackCamera = new Vector2(_toSpeedAttackCamera.x, _toSpeedAttackCamera.y);

        _defultCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedDefultCamera.x;
        _defultCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedDefultCamera.y;
        _attackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedAttackCamera.x;
        _attackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedAttackCamera.y;
        _finishAttackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.x;
        _finishAttackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.y;
    }

    public void ISpecialMovingPausePause()
    {
        _defultCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        _defultCameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
        _attackCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        _attackCameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
        _finishAttackCameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        _finishAttackCameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
    }

    public void ISpecialMovingPauseResume()
    {
        _defultCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedDefultCamera.x;
        _defultCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedDefultCamera.y;
        _attackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedAttackCamera.x;
        _attackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedAttackCamera.y;
        _finishAttackCameraPOV.m_HorizontalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.x;
        _finishAttackCameraPOV.m_VerticalAxis.m_MaxSpeed = _setSpeedFinishAttackCamera.y;
    }




}
