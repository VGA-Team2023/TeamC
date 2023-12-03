using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class PlayerLockOn
{
    [Header("Ui設定")]
    [SerializeField] private PlayerLockOnUI _lockOnUI;

    [Header("当たり判定_Offset")]
    [SerializeField] private Vector3 _offset;

    [Header("当たり判定_Size")]
    [SerializeField] private Vector3 _size;

    [Header("敵のレイヤー")]
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField]
    private bool _isDrawGizmo = true;

    private bool _isLockOn = false;

    public bool IsLockOn => _isLockOn;

    private GameObject _nowLockonEnemy = null;

    private PlayerControl _playerControl;

    public GameObject NowLockOnEnemy => _nowLockonEnemy;
    public PlayerLockOnUI PlayerLockOnUI => _lockOnUI;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _lockOnUI.Init(playerControl);
    }


    public void CheckLockOn()
    {
        CheckEnmeyIsDestroy();
        LockOn();
        ChengeEnemy();
    }

    public void LockOn()
    {
        if (_playerControl.InputManager.IsLockOn && !_isLockOn)
        {
            Debug.Log("LockOn開始");
            var c = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, 128);

            //敵がいなかったら何もしない
            if (c.Length == 0)
            {
                Debug.Log("LockOn_敵無し");
                return;
            }

            _isLockOn = true;


            GameObject lockOn = c[0].gameObject;
            float angle = Mathf.Abs(Vector3.Angle(_playerControl.PlayerT.forward, c[0].transform.position - _playerControl.PlayerT.position));

            for (int i = 1; i < c.Length; i++)
            {
                float a = Mathf.Abs(Vector3.Angle(_playerControl.PlayerT.forward, c[i].transform.position - _playerControl.PlayerT.position));

                if (angle > a)
                {
                    angle = a;
                    lockOn = c[i].gameObject;
                }
            }

            _nowLockonEnemy = lockOn;

            _lockOnUI.LockOn(true);
        }
        else if (_playerControl.InputManager.IsLockOn && _isLockOn)
        {
            Debug.Log("LockOn終了");
            _isLockOn = false;
            _nowLockonEnemy = null;
        }
    }

    /// <summary>ロックオンした敵がいなくなったっていないかどうかを確認</summary>
    public void CheckEnmeyIsDestroy()
    {
        if (_nowLockonEnemy == null && _isLockOn)
        {
            _isLockOn = false;
            _lockOnUI.LockOn(false);
        }
    }

    public void ChengeEnemy()
    {
        if (!_isLockOn || _playerControl.InputManager.IsChangeLockOnEney == 0) return;

        Debug.Log("LockOn変更");

        bool isRight = false;

        if (_playerControl.InputManager.IsChangeLockOnEney > 0)
        {
            isRight = true;
        }

        var c = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, 128);

        //敵がいなかったら何もしない
        if (c.Length == 0) return;


        GameObject lockOn = c[0].gameObject;
        float angle = Vector3.Angle(_playerControl.PlayerT.forward, c[0].transform.position - _playerControl.PlayerT.position);

        for (int i = 1; i < c.Length; i++)
        {
            if (c[i].gameObject == _nowLockonEnemy) continue;

            float a = Vector3.Angle(_playerControl.PlayerT.forward, c[i].transform.position - _playerControl.PlayerT.position);

            if (isRight)
            {
                if (angle < a)
                {
                    angle = a;
                    lockOn = c[i].gameObject;
                }
            }
            else
            {
                if (angle > a)
                {
                    angle = a;
                    lockOn = c[i].gameObject;
                }
            }

        }

        _nowLockonEnemy = lockOn;
    }


    public void OnDrwowGizmo(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.cyan;

            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            Gizmos.DrawWireCube(_offset, _size / 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }
}
