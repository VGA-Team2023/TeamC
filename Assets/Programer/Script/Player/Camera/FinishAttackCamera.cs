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


    private CameraControl _cameraControl;

    private CinemachinePOV _pov;

    public void Init(CameraControl cameraControl, CinemachineVirtualCamera camera)
    {
        _cameraControl = cameraControl;
        _pov = camera.GetCinemachineComponent<CinemachinePOV>();
    }


    public void DoFinishCamera()
    {

    }


}
