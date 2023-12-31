using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BossRotate
{
    [Header("回転速度")]
    [SerializeField] private float _rotateSpeed = 100f;

    /// <summary>向くべき回転方向</summary>
    Quaternion _targetRotation;


    private BossControl _bossControl;


    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void SetRotation()
    {
        Vector3 dir = _bossControl.PlayerT.position - _bossControl.BossT.position;
        dir.y = 0;
        _targetRotation = Quaternion.LookRotation(dir, Vector3.up);

        _bossControl.BossT.rotation = Quaternion.RotateTowards(_bossControl.BossT.rotation, _targetRotation, _rotateSpeed);
    }





}
