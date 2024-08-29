using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAvoidMove
{
    [Header("@回避速度")]
    [SerializeField] private float _avoidSpeed = 4f;

    [Header("@回避最大距離")]
    [SerializeField] private float _maxAvoidDistance = 3f;

    [Header("@回避後の追加速度")]
    [SerializeField] private float _endAddSpeed = 3f;

    [Header("@回避開始時に移動速度を0にするかどうか")]
    [SerializeField] private bool _isZero = false;

    [Header("回転速度")]
    [SerializeField] private float _rotateSpeed = 800;

    /// <summary>回避の方向 </summary>
    private Vector3 _avoidDir = default;

    private Vector3 _startPos;

    private Quaternion _targetRotation;

    /// <summary>回避時に入力があったかどうか</summary>
    private bool _isInput = false;


    private PlayerControl _playerControl;

    public float AvoidSpeed => _avoidSpeed;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetAvoidDir(Vector3 dir, bool isInpt)
    {
        _avoidDir = dir;
        _isInput = isInpt;
    }

    public void StartAvoid(Vector3 startPos)
    {
        _startPos = startPos;

        //  _playerControl.PlayerModelT.rotation = Quaternion.Euler(0, 0, 0);

        if (_isZero)
        {
            _playerControl.Rb.velocity = Vector3.zero;
        }
        _playerControl.Rb.velocity = _avoidDir * _avoidSpeed;
    }


    public bool Move()
    {
        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        Vector3 velo = horizontalRotation * new Vector3(h, 0, v).normalized;
        var rotationSpeed = _rotateSpeed * Time.deltaTime;

        if (velo.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);
        }

        if (!_isInput)
        {
            _targetRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            velo = horizontalRotation * new Vector3(0, 0, -1).normalized;
        }
        else
        {

        }

        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);

        if (_isInput)
        {
            if (velo != Vector3.zero)
            {
                _playerControl.Rb.velocity = _playerControl.PlayerT.forward * _avoidSpeed;
            }
        }
        else
        {
            _playerControl.Rb.velocity = _avoidDir * _avoidSpeed;
        }


        if (_isInput)
        {
            return false;
        }
        else
        {
            if (Vector3.Distance(_playerControl.PlayerT.position, _startPos) > _maxAvoidDistance)
            {
                return true;
            }
            return false;
        }


    }

    public void MoveEnd()
    {
        _playerControl.Rb.velocity = Vector3.zero;
        _playerControl.Rb.AddForce(_avoidDir * _endAddSpeed, ForceMode.Impulse);
    }

}