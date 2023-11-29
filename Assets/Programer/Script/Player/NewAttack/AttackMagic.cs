using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackMagic
{
    [Header("===魔法の設定===")]
    [SerializeField] private List<AttackMagicBase> _magicSetting = new List<AttackMagicBase>();

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

    private bool _isCanAttack = false;

    private PlayerControl _playerControl;

    private AttackMagicBase _attackBase;

    public bool IsCanAttack => _isCanAttack;
    public AttackMagicBase MagicBase => _attackBase;
    public ShortChantingMagicAttackMove ShortChantingMagicAttackMove => _shortChantingMagicAttackMove;
    public void Init(PlayerControl playerControl)
    {
        Debug.Log("レイヤー:" + _targetLayer);
        _playerControl = playerControl;
        _shortChantingMagicAttackMove.Init(playerControl);
        _attackBase = _magicSetting[0];
        _attackBase.Init(playerControl);
    }


    /// <summary>
    /// 範囲内にあるコライダーを取得する
    /// </summary>
    /// <returns> 移動方向 :正の値, 負の値 </returns>
    public void Attack(int attackCount)
    {
        //アニメーション再生
        _playerControl.PlayerAnimControl.SetAttackTrigger();

        //コントローラーの振動
        _playerControl.ControllerVibrationManager.OneVibration(0.2f, 0.5f, 0.5f);

        //カメラの振動
        _playerControl.CameraControl.ShakeCamra(CameraType.AttackCharge, CameraShakeType.AttackNomal);

        if (attackCount == _magicSetting.Count)
        {
            _isCanAttack = false;
        }

        //敵を索敵
        // Transform[] t = CheckFinishingEnemy();
        //敵を索敵
        Transform[] t = _playerControl.ColliderCheck.EnemySearch(_searchType, _offset, _size,128);
        if (t.Length == 0)
        {
            //魔法の攻撃処理
            _attackBase.UseMagick(t, attackCount);
            _shortChantingMagicAttackMove.SetEnemy(null);
        }
        else
        {
            //魔法の攻撃処理
            _attackBase.UseMagick(t, attackCount);
            _shortChantingMagicAttackMove.SetEnemy(t[0]);
        }   //タメが遅いときs
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
