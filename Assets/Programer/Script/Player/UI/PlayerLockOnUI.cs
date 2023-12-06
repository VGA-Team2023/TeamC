using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLockOnUI
{
    [Header("LockOnのUI")]
    [SerializeField] private GameObject _lockOnUI;

    [Header("LockOn中かどうかのUI")]
    [SerializeField] private GameObject _isLockOnUI;

    [Header("Canvas")]
    [SerializeField] private RectTransform _parentUI;


    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public void LockOn(bool isLockOn)
    {
        _lockOnUI.SetActive(isLockOn);
        _isLockOnUI.SetActive(isLockOn);
    }



    // UIの位置を更新する
    public void UpdateFinishingUIPosition()
    {
        if (_playerControl.LockOn.NowLockOnEnemy == null)
        {
            _lockOnUI.SetActive(false);
            _isLockOnUI.SetActive(false);
            return;
        }

        var cameraTransform = Camera.main.transform;

        // カメラの向きベクトル
        var cameraDir = cameraTransform.forward;

        // オブジェクトの位置
        var targetWorldPos = _playerControl.LockOn.NowLockOnEnemy.transform.position;

        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - _playerControl.PlayerT.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        _lockOnUI.gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = Camera.main.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        _lockOnUI.transform.localPosition = uiLocalPos;

        if (_lockOnUI.transform.localPosition == Vector3.zero) _lockOnUI.SetActive(false);

    }

}
