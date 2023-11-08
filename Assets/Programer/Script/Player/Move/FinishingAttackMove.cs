using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishingAttackMove
{
    [Header("ˆÚ“®‘¬“x")]
    [SerializeField] private float _moveSpeed = 3f;

    [Header("‰ñ“]‘¬“x")]
    [SerializeField] private float _rotateSepeed = 200f;

    private PlayerControl _playerControl;


    private Transform _enemy;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetEnemy(Transform enemy)
    {
        _enemy = enemy;
    }

    public void Move()
    {
        //ˆÚ“®“ü—Í‚ðŽó‚¯Žæ‚é
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        Vector3 velo = horizontalRotation * new Vector3(h, 0, v).normalized;

        //‘¬“x‚ð‰Á‚¦‚é
        _playerControl.Rb.velocity = velo * _moveSpeed;
    }

    public void Rotation()
    {
        Vector3 dirP = default;

        if (_enemy != null)
        {
            dirP = _enemy.transform.position - _playerControl.PlayerT.position;
            dirP.y = 0;
        }

        Quaternion _targetRotation = Quaternion.LookRotation(dirP, Vector3.up);

        Quaternion setR = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * _rotateSepeed);
        setR.x = 0;
        setR.z = 0;
        _playerControl.PlayerT.rotation = setR;
    }

}
