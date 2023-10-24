using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class SetUpCamera
{
    [Header("通常のFOV")]
    [SerializeField] private float _defaltFOV = 60;

    [Header("最大のFOV")]
    [SerializeField] private float _maxFOV = 60;

    [Header("最小のFOV")]
    [SerializeField] private float _minFOV = 50;

    [Header("FOV")]
    [SerializeField] private float _changeFOVSpeed = 3;

    private CameraControl _cameraControl;

    private CinemachinePOV _pov;

    public void Init(CameraControl cameraControl)
    {
        _cameraControl = cameraControl;
        _pov = _cameraControl.SetUpCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void ResetCamera()
    {
        _cameraControl.SetUpCamera.m_Lens.FieldOfView = _defaltFOV;
        _cameraControl.DedultCamera.m_Lens.FieldOfView = _defaltFOV;
    }

    /// <summary>
    /// 構え移行の際に、カメラを遠巻きにする
    /// </summary>
    public void SetFOV()
    {
     
    }

}
