using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class PlayerLockOn
{
    [Header("@敵の探知範囲_Offset")]
    [SerializeField] private Vector3 _offset;

    [Header("@敵の探知範囲_Size")]
    [SerializeField] private Vector3 _size;

    [Header("@Gizmoを表示するかどうか")]
    [SerializeField] private bool _isDrawGizmo = true;

    [Header("敵のレイヤー")]
    [SerializeField] private LayerMask _targetLayer;

    [Header("Ui設定")]
    [SerializeField] private PlayerLockOnUI _lockOnUI;

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
        if (!_isLockOn || (!_playerControl.InputManager.IsChangeLockOnEnemyLeft && !_playerControl.InputManager.IsChangeLockOnEnemyRight)) return;


        Debug.Log("LockOn変更");

        //1、敵を取得
        //2、カメラに移っている敵のみを選別
        //3、角度が低い順に並び変え
        //4、現在の敵の前後を選択する
        bool isRight = _playerControl.InputManager.IsChangeLockOnEnemyRight;


        //1、敵を取得
        var c = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, 128);

        //敵がいなかったら何もしない
        if (c.Length == 0) return;

        //2、カメラに移っている敵のみを選別
        List<GameObject> inCameraEnemys = new List<GameObject>();
        foreach (var e in c)
        {
            // オブジェクトの位置をスクリーン座標に変換
            Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(e.position);

            // カメラのビューポート内にオブジェクトがあるかどうかを判定
            if (objectScreenPos.x > 0 && objectScreenPos.x < Screen.width &&
                objectScreenPos.y > 0 && objectScreenPos.y < Screen.height && objectScreenPos.z > 0)
            {
                inCameraEnemys.Add(e.gameObject);
            }
        }

        //3並び変え

        // カメラの位置をスクリーン座標に変換
        Vector3 cameraScreenPos = Camera.main.WorldToScreenPoint(Camera.main.transform.position);

        // オブジェクトをカメラからの距離でソート
        inCameraEnemys = inCameraEnemys.OrderBy(obj =>
        {
            // オブジェクトのスクリーン座標を取得
            Vector3 objScreenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

            // カメラとオブジェクトのスクリーン座標の差を距離として計算
            return objScreenPos.x - cameraScreenPos.x;
        }).ToList();

        //ロックオン中の敵がいるかどうか
        bool isContainLockOnEnemy = inCameraEnemys.Contains(_nowLockonEnemy);

        if (isContainLockOnEnemy)
        {
            int i = inCameraEnemys.IndexOf(_nowLockonEnemy);

            if (isRight)
            {
                if (i == inCameraEnemys.Count - 1)
                {
                    _nowLockonEnemy = inCameraEnemys[0];
                }
                else
                {
                    _nowLockonEnemy = inCameraEnemys[i + 1];
                }
            }
            else
            {
                if (i == 0)
                {
                    _nowLockonEnemy = inCameraEnemys[inCameraEnemys.Count - 1];
                }
                else
                {
                    _nowLockonEnemy = inCameraEnemys[i - 1];
                }
            }
        }
        else
        {
            _nowLockonEnemy = inCameraEnemys[0];
        }
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
