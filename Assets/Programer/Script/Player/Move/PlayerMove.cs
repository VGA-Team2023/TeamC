using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMove
{
    [Header("@通常移動の加速速度")]
    [SerializeField] private float _walkAddSpeed = 4;
    [Header("走りの加速速度")]
    [SerializeField] private float _runAddSpeed = 8;

    [Header("@通常移動の限度速度")]
    [SerializeField] private float _walkSpeedLimit = 10;

    [Header("ダッシュの実行時間")]
    [SerializeField] private float _dashTime = 8;

    [Header("止まるまでの時間")]
    [SerializeField] private float _stopTime = 2;

    /// <summary>ダッシュの実行時間計測</summary>
    private float _countDashTime = 0;

    /// <summary>ダッシュ中、PlayerのRbに設定する速度の値 </summary>
    private float _dashSetSpeed = 3f;

    /// <summary>ダッシュ中かどうか </summary>
    private bool _isDash = false;

    /// <summary>移動から停止しるまでの時間の計測 </summary>
    private float _countStopTime = 0;

    /// <summary>移動から停止しるまでの、PlayerのRbに設定する速度の値  </summary>
    private Vector3 _nowStopSpeed = Vector3.zero;


    // [Header("空中での速度")]
    // [SerializeField]
    private float _airMoveSpeed = 4;

    // [Header("ジャンプパワー")]
    //[SerializeField]
    private float _jumpPower = 4;

    [Header("@移動時の回転速度")][SerializeField] private float _walkRotateSpeed = 100;

     [Header("走りの時の回転速度")]
    [SerializeField]
    private float _runRotateSpeed = 100;

    //  [Header("重力")]
    // [SerializeField] 
    private float _gravity = 0.9f;

    /// <summary>入力方向</summary>
    private Vector3 velo;

    /// <summary>向くべきプレイヤーの回転</summary>
    Quaternion _targetRotation;

    private PlayerControl _playerControl = null;

    public float WalkSpeedLimit => _walkSpeedLimit;

    /// <summary>StateMacineをセットする関数</summary>
    /// <param name="stateMachine"></param>
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public enum MoveType
    {
        Walk,
        Run,
    }

    /// <summary>ダッシュ開始時に呼ぶ</summary>
    public void StartDash()
    {
        //Vector3 velo = _playerControl.Rb.velocity.normalized;

        //_playerControl.Rb.AddForce(velo * _runAddSpeed);
        _isDash = true;
        _countDashTime = 0;
    }

    /// <summary>ダッシュ中のパラメータリセット</summary>
    public void ResetDashParamater()
    {
        _isDash = false;
    }


    /// <summary>ダッシュ中の減衰を計算する</summary>
    public void CountDashTime()
    {
        if (!_isDash) return;

        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        //入力が0の場合、すぐに速度を通常に戻す
        if(h==0 && v ==0)
        {
            _isDash = false;
            _dashSetSpeed = _walkSpeedLimit;
            return;
        }

        //ダッシュの実行時間を計測
        _countDashTime += Time.deltaTime;

        //ダッシュ→歩き　の限界速度に徐々に近づけていく
        _dashSetSpeed = Mathf.Lerp(_playerControl.Avoid.AvoidMove.AvoidSpeed, _walkSpeedLimit, _countDashTime / _dashTime);

        if (_countDashTime >= _dashTime)
        {
            _isDash = false;
        }
    }

    /// <summary>速度減衰開始時に呼ぶ</summary>
    public void StartStop()
    {
        if (_playerControl == null) return;
        _countStopTime = 0;
        _nowStopSpeed = _playerControl.Rb.velocity;
    }




    /// <summary>Idle状態までの速度の減衰 </summary>
    public void MoveStopTime()
    {
        if (_playerControl.Rb.velocity.magnitude == 0) return;

        //ダッシュの実行時間を計測
        _countStopTime += Time.deltaTime;

        //ダッシュ→歩き　の限界速度に徐々に近づけていく
        _nowStopSpeed.x = Mathf.Lerp(_playerControl.Rb.velocity.x, 0, _countStopTime / _stopTime);
        _nowStopSpeed.z = Mathf.Lerp(_playerControl.Rb.velocity.z, 0, _countStopTime / _stopTime);

        if (_countStopTime >= _stopTime)
        {
            _playerControl.Rb.velocity = Vector3.zero;
        }
    }

    /// <summary>速度を0にする</summary>
    public void SetSpeedDeletion()
    {
        _playerControl.Rb.velocity = new Vector3(_nowStopSpeed.x, 0, _nowStopSpeed.z);
    }


    public void Move(MoveType moveType)
    {
        //移動方向の転換速度
        float turnSpeed = 0;

        //移動速度
        float moveSpeed = 0;

        //走り方によって速度を変更
        if (moveType == MoveType.Walk)
        {
            turnSpeed = _walkRotateSpeed;
            moveSpeed = _walkAddSpeed;
        }
        else
        {
            turnSpeed = _runRotateSpeed;
            moveSpeed = _runAddSpeed;
        }

        if (_isDash)
        {
            turnSpeed = _runRotateSpeed;
        }
        else
        {
            turnSpeed = _walkRotateSpeed;
        }


        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;


        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velo = horizontalRotation * new Vector3(h, 0, v).normalized;
        var rotationSpeed = turnSpeed * Time.deltaTime;

        if (velo.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);
        }

        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);

        if (velo == Vector3.zero) return;

        //速度を加える
        if (_isDash)
        {
            _playerControl.Rb.velocity = _playerControl.PlayerT.forward * _dashSetSpeed;
        }
        else
        {
            _playerControl.Rb.AddForce(_playerControl.PlayerT.forward * moveSpeed);
            SpeedLimit();
        }

        Debug.Log(_playerControl.Rb.velocity);
        //重力を加える
        //_playerControl.Rb.AddForce(Vector3.down * _gravity);
    }

    public void SpeedLimit()
    {
        //移動入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;



        if (_playerControl.Rb.velocity.x > _walkSpeedLimit)
        {
            _playerControl.Rb.velocity = new Vector3(_walkSpeedLimit, _playerControl.Rb.velocity.y, _playerControl.Rb.velocity.z);
        }
        else if (_playerControl.Rb.velocity.x < -_walkSpeedLimit)
        {
            _playerControl.Rb.velocity = new Vector3(-_walkSpeedLimit, _playerControl.Rb.velocity.y, _playerControl.Rb.velocity.z);
        }

        if (_playerControl.Rb.velocity.z > _walkSpeedLimit)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, _playerControl.Rb.velocity.y, _walkSpeedLimit);
        }
        else if (_playerControl.Rb.velocity.z < -_walkSpeedLimit)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, _playerControl.Rb.velocity.y, -_walkSpeedLimit);
        }

    }


    /// <summary>空中での動き</summary>
    public void AirMove()
    {
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velo = horizontalRotation * new Vector3(h, 0, v).normalized;
        var rotationSpeed = 100 * Time.deltaTime;

        if (velo.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(velo, Vector3.up);
        }

        _playerControl.PlayerT.rotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, rotationSpeed);


        float speed = 0;

        speed = _airMoveSpeed;

        _playerControl.Rb.AddForce(velo * speed);
    }

    public void Jump()
    {
        Vector3 velo = new Vector3(_playerControl.Rb.velocity.x, _jumpPower, _playerControl.Rb.velocity.z);
        _playerControl.Rb.velocity = velo;
    }


}
