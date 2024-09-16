﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class DefaultCamera
{
    [Header("通常のFOV")]
    [SerializeField] private float _defaltFOV = 60;

    [Header("最大のFOV")]
    [SerializeField] private float _maxFOV = 60;

    [Header("最小のFOV")]
    [SerializeField] private float _minFOV = 50;

    [Header("FOV")]
    [SerializeField] private float _changeFOVSpeed = 3;

    [Header("右Duch")]
    [SerializeField] private float _rightDutch = 5f;

    [Header("左Duch")]
    [SerializeField] private float _leftDutch = -5f;

    [Header("Dutchの変更速度")]
    [SerializeField] private float _changeDutchSpeedAvoid = 3;

    [SerializeField] private float _changeFOVSpeedAvoid = 3;

    [Header("fovValueの変更速度")]
    [SerializeField] private float _changeHAxisSpeed = 0.2f;

    [Header("回避中のfovValueの変更速度")]
    [SerializeField] private float _avoidChangeHAxisSpeed = 0.7f;

    private float _inputH = 0;

    /// <summary>画面効果を変更をしない、設定</summary>
    private bool _isNonAdditve = false;

    public bool IsNonAdditve => _isNonAdditve;

    private CameraControl _cameraControl;

    private CinemachineVirtualCamera _defultCamera;

    private CinemachinePOV _pov;

    public void Init(CameraControl cameraControl, CinemachineVirtualCamera defultCamera)
    {
        _cameraControl = cameraControl;
        _defultCamera = defultCamera;
        _pov = defultCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void ResetCamera()
    {
        _cameraControl.FinishCamera.m_Lens.FieldOfView = _defaltFOV;
        _cameraControl.DedultCamera.m_Lens.FieldOfView = _defaltFOV;
    }

    public void PlayerMoveAutoRotationSet(bool isAvoid)
    {
        if (_isNonAdditve) return;

        float h = _cameraControl.PlayerControl.InputManager.HorizontalInput;


        float speed = _changeHAxisSpeed;

        if (isAvoid)
        {
            speed = _avoidChangeHAxisSpeed;
        }

        if (h > 0.5)
        {
            _pov.m_HorizontalAxis.Value += speed;
        }
        else if (h < -0.5)
        {
            _pov.m_HorizontalAxis.Value -= speed;
        }

    }

    public void SetAvoidH(float value)
    {
        _inputH = value;
    }

    /// <summary>画面効果のオンオフ設定
    /// <returns></returns>
    public void IsNonCameraEffects(bool isActive)
    {
        _isNonAdditve = isActive;
    }

    public void AvoidFov()
    {
        if (_isNonAdditve) return;

        // _defultCamera.m_Lens.FieldOfView = _maxFOV;
        if (_defultCamera.m_Lens.FieldOfView < _maxFOV)
        {
            _defultCamera.m_Lens.FieldOfView += Time.deltaTime * _changeFOVSpeedAvoid;
            if (Mathf.Abs(_defultCamera.m_Lens.FieldOfView - _maxFOV) < 0.1f)
            {
                _defultCamera.m_Lens.FieldOfView = _maxFOV;
            }
        }

        if (_inputH > 0)
        {
            if (_defultCamera.m_Lens.Dutch != _rightDutch)
            {
                _defultCamera.m_Lens.Dutch += Time.deltaTime * _changeDutchSpeedAvoid;
                if (_defultCamera.m_Lens.Dutch > _rightDutch)
                {
                    _defultCamera.m_Lens.Dutch = _rightDutch;
                }
            }
        }
        else if (_inputH < 0)
        {
            if (_defultCamera.m_Lens.Dutch != _leftDutch)
            {
                _defultCamera.m_Lens.Dutch -= Time.deltaTime * _changeDutchSpeedAvoid;
                if (_defultCamera.m_Lens.Dutch < _leftDutch)
                {
                    _defultCamera.m_Lens.Dutch = _leftDutch;
                }
            }
        }
        else
        {

        }
    }

    /// <summary>
    /// 構え移行の際に、カメラを遠巻きにする
    /// </summary>
    public void SetDefaultFOV()
    {
        if (_defultCamera.m_Lens.FieldOfView > _defaltFOV)
        {
            _defultCamera.m_Lens.FieldOfView -= Time.deltaTime * _changeFOVSpeed;
            if (Mathf.Abs(_defultCamera.m_Lens.FieldOfView - _defaltFOV) < 0.2f)
            {
                _defultCamera.m_Lens.FieldOfView = _defaltFOV;
            }
        }
        else if (_defultCamera.m_Lens.FieldOfView < _defaltFOV)
        {
            _defultCamera.m_Lens.FieldOfView += Time.deltaTime * _changeFOVSpeed;
            if (Mathf.Abs(_defultCamera.m_Lens.FieldOfView - _defaltFOV) < 0.2f)
            {
                _defultCamera.m_Lens.FieldOfView = _defaltFOV;
            }
        }

        if (_defultCamera.m_Lens.Dutch > 0)
        {
            _defultCamera.m_Lens.Dutch -= Time.deltaTime * _changeDutchSpeedAvoid;
            if (_defultCamera.m_Lens.Dutch < 0.2f)
            {
                _defultCamera.m_Lens.Dutch = 0;
            }
        }
        else if (_defultCamera.m_Lens.Dutch < 0)
        {
            _defultCamera.m_Lens.Dutch += Time.deltaTime * _changeDutchSpeedAvoid;
            if (_defultCamera.m_Lens.Dutch > -0.2f)
            {
                _defultCamera.m_Lens.Dutch = 0;
            }
        }

    }

}