using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AttackMagic
{
    [Header("---@魔法の設定---")]
    [SerializeField] private List<AttackMagicBase> _magicSetting = new List<AttackMagicBase>();

    [Header("---@移動設定---")]
    [SerializeField] private ShortChantingMagicAttackMove _shortChantingMagicAttackMove;

    [Header("@敵の探索範囲の_Offset")]
    [SerializeField] private Vector3 _offset;

    [Header("@敵の探索範囲の_Size")]
    [SerializeField] private Vector3 _size;

    [Header("@Gizmoを表示するかどうか")]
    [SerializeField] private bool _isDrawGizmo = true;

    [Header("敵のレイヤー")]
    [SerializeField] private LayerMask _targetLayer;


    private SearchType _searchType = SearchType.AllEnemy;



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
        _magicSetting[0].Init(playerControl);
        _magicSetting[1].Init(playerControl);
    }

    public void SetMagicBase(PlayerAttribute playerAttribute)
    {
        if (playerAttribute == PlayerAttribute.Ice)
        {
            _attackBase = _magicSetting[0];
        }
        else
        {
            _attackBase = _magicSetting[1];
        }
    }

    /// <summary>
    /// 範囲内にあるコライダーを取得する
    /// </summary>
    /// <returns> 移動方向 :正の値, 負の値 </returns>
    public void Attack(int attackCount)
    {
        if (attackCount == _magicSetting.Count)
        {
            _isCanAttack = false;
        }

        //敵を索敵
        Transform[] t = _playerControl.ColliderCheck.EnemySearch(_searchType, _offset, _size, 128);

        List<Transform> inCameraEnemys = new List<Transform>();

        //画面内に映っている敵を選別
        foreach (var a in t)
        {
            Vector3 viewportPoint = Camera.main.WorldToViewportPoint(a.transform.position);

            // zが正であることを確認し、xとyが0〜1の範囲内に収まっているかどうかを確認
            if (viewportPoint.z > 0 && viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1)
            {
                inCameraEnemys.Add(a);
            }
        }

        // プレイヤーとの距離が近い順に並べ替える
        inCameraEnemys.Sort((a, b) =>
                  Vector3.Distance(_playerControl.PlayerT.position, a.position).CompareTo(Vector3.Distance(_playerControl.PlayerT.position, b.position)));


        _attackBase.Enemys = inCameraEnemys.ToArray();

        _attackBase.IsAttackNow = true;
        _attackBase.AttackCount = attackCount;

        if (t.Length == 0)
        {
            //魔法の攻撃処理
            // _attackBase.UseMagick(t, attackCount);
            _shortChantingMagicAttackMove.SetEnemy(null);
        }
        else
        {
            //魔法の攻撃処理
            // _attackBase.UseMagick(t, attackCount);
            _shortChantingMagicAttackMove.SetEnemy(t[0]);
        }   //タメが遅いとき
    }

    public void StopMagic(int attackCount)
    {
        _attackBase.StopMagic(attackCount);
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
