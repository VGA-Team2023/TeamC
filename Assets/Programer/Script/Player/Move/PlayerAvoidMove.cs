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
    }


    public bool Move()
    {
        _playerControl.Rb.velocity = _avoidDir * _avoidSpeed;

        if (Vector3.Distance(_playerControl.PlayerT.position, _startPos) > _maxAvoidDistance)
        {
            return true;
        }
        return false;
    }

}