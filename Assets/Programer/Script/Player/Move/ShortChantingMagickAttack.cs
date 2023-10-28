using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShortChantingMagickAttack
{
    [Header("===魔法の設定===")]
    [SerializeField] private ShortChantingMagicData _shortChantingMagicData;

    [Header("ための時間")]
    [SerializeField] private float _time = 1;


    public float TameTime => _time;

    [Header("移動設定")]
    [SerializeField] private ShortChantingMagicAttackMove _shortChantingMagicAttackMove;

    [Header("攻撃の種類")]
    [SerializeField] private SearchType _searchType = SearchType.NearlestEnemy;

    [Header("当たり判定_Offset")]
    [SerializeField] private Vector3 _offset;

    [Header("当たり判定_Size")]
    [SerializeField] private Vector3 _size;

    [Header("敵のレイヤー")]
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField]
    private bool _isDrawGizmo = true;
    private int _attackCount = 0;

    private PlayerControl _playerControl;

    public ShortChantingMagicData ShortChantingMagicData => _shortChantingMagicData;

    public int AttackCount => _attackCount;
    public ShortChantingMagicAttackMove ShortChantingMagicAttackMove => _shortChantingMagicAttackMove;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _shortChantingMagicData.Init(playerControl);
        _shortChantingMagicAttackMove.Init(playerControl);
    }

    /// <summary>
    /// 範囲内にあるコライダーを取得する
    /// </summary>
    /// <returns> 移動方向 :正の値, 負の値 </returns>
    public void Attack(float time)
    {
        //アニメーション再生
        _playerControl.PlayerAnimControl.SetAttackTrigger();

        //コントローラーの振動
        _playerControl.ControllerVibrationManager.OneVibration(0.2f, 0.5f, 0.5f);

        //カメラの振動
        _playerControl.CameraControl.ShakeCamra(CameraType.SetUp, CameraShakeType.AttackNomal);

        _attackCount++;


        if (time < _time)
        {
            //敵を索敵
            Transform[] t = _playerControl.ColliderCheck.EnemySearch(_searchType, _offset, _size, _targetLayer);
            if (t.Length == 0)
            {
                //魔法の攻撃処理
                _shortChantingMagicData.AttackOneEnemy(t);
                _shortChantingMagicAttackMove.SetEnemy(null);
            }
            else
            {
                _shortChantingMagicAttackMove.SetEnemy(t[0]);

                //魔法の攻撃処理
                _shortChantingMagicData.AttackOneEnemy(t);
            }
        }   //タメが早い時
        else
        {
            //敵を索敵
            Transform[] t = _playerControl.ColliderCheck.EnemySearch(SearchType.AllEnemy, _offset, _size, _targetLayer);
            if (t.Length == 0)
            {
                //魔法の攻撃処理
                _shortChantingMagicData.AttackOneEnemy(t);
                _shortChantingMagicAttackMove.SetEnemy(null);
            }
            else
            {
                _shortChantingMagicAttackMove.SetEnemy(t[0]);

                //魔法の攻撃処理
                _shortChantingMagicData.AttackAllEnemy(t);
            }   //タメが遅いとき
        }
    }



    /// <summary>
    /// 武器変更時に呼ぶ
    /// </summary>
    public void UnSetMagic()
    {
        //魔法陣を消す
        _shortChantingMagicData.UnSetMagick();
        _attackCount = 0;
    }


    public void OnDrwowGizmo(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.red;

            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            Gizmos.DrawWireCube(_offset, _size / 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }

}


