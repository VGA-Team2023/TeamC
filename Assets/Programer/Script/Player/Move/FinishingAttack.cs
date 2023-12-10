using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FinishingAttack
{
    [Header("---@詳細設定---")]
    [SerializeField] private FinishingAttackShort _finishingAttackShort;

    [Header("---@移動設定---")]
    [SerializeField] private FinishingAttackMove _finishingAttackMove;

    [Header("---UIの設定---")]
    [SerializeField] private FinishingAttackUI _finishingAttackUI;

    [Header("レイヤー")]
    [SerializeField] private LayerMask _targetLayer;

    /// <summary>トドメをさせるかどうか</summary>
    private bool _isCanFinishing = false;

    /// <summary>トドメのアニメーションを終えたかどうか</summary>
    private bool _isEndFinishAnim = false;

    /// <summary>トドメの時間まで出来たかどうか</summary>
    private bool _isCompletedFinishTime = false;

    private float _setFinishTime = 0;

    private float _countFinishTime = 0;

    private PlayerAttribute _startAttribute = PlayerAttribute.Ice;

    private PlayerControl _playerControl;

    private Collider[] _nowFinishEnemy;

    public bool IsEndFinishAnim { get => _isEndFinishAnim; set => _isEndFinishAnim = value; }

    public bool IsCanFinishing => _isCanFinishing;
    public FinishingAttackShort FinishingAttackShort => _finishingAttackShort;

    public FinishingAttackMove FinishingAttackMove => _finishingAttackMove;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;

        _finishingAttackUI.Init(playerControl, _finishingAttackShort.FinishTime);
        _finishingAttackShort.Init(playerControl);
        _finishingAttackMove.Init(playerControl);
    }



    public void StartFinishingAttack()
    {
        _startAttribute = _playerControl.PlayerAttributeControl.PlayerAttribute;
        _playerControl.PlayerAudio.FinishCharge(_playerControl.PlayerAttributeControl.PlayerAttribute, true);

        _isEndFinishAnim = false;

        _isCompletedFinishTime = false;

        _countFinishTime = 0;

        //トドメの時間を設定
        _setFinishTime = _finishingAttackShort.FinishTime;

        //コントローラーの振動
        _playerControl.ControllerVibrationManager.StartVibration();

        //短い詠唱の魔法陣を消す
        //魔法陣無しの場合、テスト
        if (!_playerControl.IsNewAttack)
        {
            _playerControl.Attack.ShortChantingMagicAttack.UnSetMagic();
        }

        //エフェクトを設定
        _finishingAttackShort.FinishAttackNearMagic.SetEffect();

        //アニメーション再生
        _playerControl.PlayerAnimControl.StartFinishAttack(AttackType.LongChantingMagick);



        //敵を索敵
        _nowFinishEnemy = CheckFinishingEnemy();

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.StartFinishing();
        }

        //トドメ用のカメラを使う
        _playerControl.CameraControl.UseFinishCamera();


        //移動視点
        _finishingAttackMove.SetEnemy(_nowFinishEnemy[0].transform);
        //カメラを敵の方向に向ける
        _playerControl.CameraControl.FinishAttackCamera.SetCameraFOVStartFinish(_nowFinishEnemy[0].transform.position);

        for (int i = 0; i < _nowFinishEnemy.Length; i++)
        {
            if (_nowFinishEnemy[i].gameObject == _playerControl.LockOn.NowLockOnEnemy)
            {
                //移動視点
                _finishingAttackMove.SetEnemy(_playerControl.LockOn.NowLockOnEnemy.transform);
                //カメラを敵の方向に向ける
                _playerControl.CameraControl.FinishAttackCamera.SetCameraFOVStartFinish(_playerControl.LockOn.NowLockOnEnemy.transform.position);
                break;
            }
        }






        //UIを出す
        _finishingAttackUI.SetFinishUI(_setFinishTime, _nowFinishEnemy.Length);
    }

    public void SetUI()
    {
        if (!_isCompletedFinishTime)
        {
            for (int i = 0; i < _nowFinishEnemy.Length; i++)
            {
                _finishingAttackUI.UpdateFinishingUIPosition(_nowFinishEnemy[i].transform, i);
            }
        }
    }

    /// <summary>
    /// トドメをさしている時間を計測、入力を観測
    /// </summary>
    /// <returns></returns>
    public bool DoFinishing()
    {
        if (_playerControl.InputManager.IsFinishAttack && !_isCompletedFinishTime)
        {
            _countFinishTime += Time.deltaTime;

            _finishingAttackUI.ChangeValue(Time.deltaTime);

            if (_countFinishTime >= _setFinishTime)
            {
                CompleteAttack();
            }
            return true;
        }
        else if (!_playerControl.InputManager.IsFinishAttack && !_isCompletedFinishTime)
        {
            StopFinishingAttack();
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>トドメをし終えた時の処理</summary>
    private void CompleteAttack()
    {
        _playerControl.PlayerAudio.FinishCharge(_playerControl.PlayerAttributeControl.PlayerAttribute, false);
        _playerControl.PlayerAudio.Finish(_playerControl.PlayerAttributeControl.PlayerAttribute);

        _isCompletedFinishTime = true;

        _finishingAttackShort.FinishAttackNearMagic.SetFinishEffect();

        //カメラ終わり
        _playerControl.CameraControl.FinishAttackCamera.EndFinish();

        //通常のカメラに戻す
        _playerControl.CameraControl.UseDefultCamera(false);

        //カメラの振動
        _playerControl.CameraControl.ShakeCamra(CameraType.Defult, CameraShakeType.EndFinishAttack);
        _playerControl.CameraControl.ShakeCamra(CameraType.FinishCamera, CameraShakeType.EndFinishAttack);

        //コントローラーの振動を停止
        _playerControl.ControllerVibrationManager.StopVibration();

        //スライダーUIを非表示にする
        _finishingAttackUI.UnSetFinishUI();

        //トドメ完了のUIを表示
        _finishingAttackUI.ShowCompleteFinishUI(true);

        //エフェクトを設定
        _finishingAttackShort.FinishAttackNearMagic.Stop();

        //時間を遅くする
        _playerControl.HitStopConrol.StartHitStop(HitStopKind.FinishAttack);


        // LineSetting();


        //アニメーション再生
        _playerControl.PlayerAnimControl.EndFinishAttack(AttackType.LongChantingMagick);

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);

            if (_playerControl.PlayerAttributeControl.PlayerAttribute == PlayerAttribute.Ice)
            {
                damgeble?.EndFinishing(MagickType.Ice);
            }
            else
            {
                damgeble?.EndFinishing(MagickType.Grass);
            }


        }
    }

    private void StopFinishingAttack()
    {
        _playerControl.PlayerAudio.FinishCharge(_playerControl.PlayerAttributeControl.PlayerAttribute, false);

        _playerControl.FinishingAttack.FinishingAttackShort.FinishAttackNearMagic.Stop(_startAttribute);

        //スライダーUIを非表示にする
        _finishingAttackUI.UnSetFinishUI();

        //通常のカメラに戻す
        _playerControl.CameraControl.UseDefultCamera(false);

        _playerControl.CameraControl.FinishAttackCamera.EndFinish();

        //エフェクトを設定
        _finishingAttackShort.FinishAttackNearMagic.Stop();

        foreach (var e in _nowFinishEnemy)
        {
            e.TryGetComponent<IFinishingDamgeble>(out IFinishingDamgeble damgeble);
            damgeble?.StopFinishing();
        }

        _playerControl.PlayerAnimControl.StopFinishAttack();

        //コントローラーの振動
        _playerControl.ControllerVibrationManager.StopVibration();
    }


    /// <summary>トドメのアニメーションが終わった。
    /// アニメーションイベントから呼ぶ。トドメのアニメーションが終わった</summary>
    public void EndFinishAnim()
    {
        _isEndFinishAnim = true;

        _nowFinishEnemy = null;

        //トドメ完了のUIを非表示
        _finishingAttackUI.ShowCompleteFinishUI(false);
    }

    /// <summary>トドメを終えた際、エフェクトを消すかどうかを判断する</summary>
    public void FinishEffectCheck()
    {
        if (!_isCompletedFinishTime)
        {

        }
    }

    /// <summary>
    /// トドメをさせる敵を探し、Uiを表示する
    /// </summary>
    public void SearchFinishingEnemy()
    {
        var enemys = CheckFinishingEnemy();

        if (enemys.Length <= 0)
        {
            _isCanFinishing = false;
            _finishingAttackUI.ShowCanFinishingUI(false);
            return;
        }

        _isCanFinishing = true;

        _finishingAttackUI.ShowCanFinishingUI(true);

        //Transform[] d = new Transform[enemys.Length];

        //for (int i = 0; i < enemys.Length; i++)
        //{
        //    d[i] = enemys[i].transform;
        //}
        _finishingAttackUI.ShowUI(enemys);
    }


    /// <summary>
    /// 範囲内にあるコライダーを取得する
    /// </summary>
    /// <returns> 移動方向 :正の値, 負の値 </returns>
    public Collider[] CheckFinishingEnemy()
    {
        Vector3 setOffset = _finishingAttackShort.Offset;
        Vector3 setSize = _finishingAttackShort.BoxSize;

        var posX = _playerControl.PlayerT.position.x + setOffset.x;
        var posY = _playerControl.PlayerT.position.y + setOffset.y;
        var posz = _playerControl.PlayerT.position.z + setOffset.z;

        Quaternion r = _playerControl.PlayerT.rotation;
        r.x = 0;
        r.z = 0;

        return Physics.OverlapBox(new Vector3(posX, posY, posz), setSize, r, _targetLayer);
    }


    public void OnDrwowGizmo(Transform origin)
    {
        _finishingAttackShort.OnDrwowGizmo(origin);
    }


}
