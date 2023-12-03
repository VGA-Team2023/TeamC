using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAvoidMove
{
    [Header("回避速度")]
    [SerializeField] private float _avoidSpeed = 4f;

    [Header("回避最大距離")]
    [SerializeField] private float _maxAvoidDistance = 3f;

    [Header("回避後の追加速度")]
    [SerializeField] private float _endAddSpeed = 3f;

    [Header("回避開始時に移動速度を0にするかどうか")]
    [SerializeField] private bool _isZero = false;

    /// <summary>回避の方向 </summary>
    private Vector3 _avoidDir = default;

    private Vector3 _startPos;

    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void SetAvoidDir(Vector3 dir)
    {
        _avoidDir = dir;
    }

    public void StartAvoid(Vector3 startPos)
    {
        _startPos = startPos;

      //  _playerControl.PlayerModelT.rotation = Quaternion.Euler(0, 0, 0);

        if (_isZero)
        {
            _playerControl.Rb.velocity = Vector3.zero;
        }

    }


    public bool Move()
    {
        if (_playerControl.Avoid.IsStartAvoid)
        {
            _playerControl.Rb.velocity = _avoidDir * _avoidSpeed;

            if (Vector3.Distance(_playerControl.PlayerT.position, _startPos) > _maxAvoidDistance)
            {
                return true;
            }
        }
        return false;
    }

    public void MoveEnd()
    {
        _playerControl.Rb.velocity = Vector3.zero;
        _playerControl.Rb.AddForce(_avoidDir * _endAddSpeed, ForceMode.Impulse);
    }

}