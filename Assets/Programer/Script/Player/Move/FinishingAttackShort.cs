using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishingAttackShort
{

    [Header("1体のみトドメをさすのに必要な時間")]
    [SerializeField] private float _finishTimePatarn1 = 1f;

    [Header("2体トドメをさすのに必要な時間")]
    [SerializeField] private float _finishTimePatarn2 = 2f;

    [Header("3体以上トドメをさすのに必要な時間")]
    [SerializeField] private float _finishTimePatarn3 = 3f;

    [Header("@探知判定_Offset")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, 3);

    [Header("@探知判定_Size")]
    [SerializeField] private Vector3 _boxSize = new Vector3(10, 10, 10);

    [Header("Gizmoを表示するかどうか")]
    [SerializeField] private bool _isDrawGizmo = true;

    [Header("Boss")]
    [SerializeField] private BossControl _boss;
 
    [Header("---エフェクト設定---")]
    [SerializeField] private FinishAttackNearMagic _finishAttackNearMagic;

    /// <summary>トドメのさし方の変化、敵の数</summary>
    private const int _patarn1EnemyNum = 1;
    private const int _patarn2EnemyNum = 2;
    private const int _patarn3EnemyNum = 3;

    public Vector3 Offset => _offset;
    public Vector3 BoxSize => _boxSize;

    private PlayerControl _playerControl;
    public float FinishTime1 => _finishTimePatarn1;
    public float FinishTime2 => _finishTimePatarn2;
    public float FinishTime3 => _finishTimePatarn3;


    public FinishAttackNearMagic FinishAttackNearMagic => _finishAttackNearMagic;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _finishAttackNearMagic.Init(playerControl);
    }


    public FinishAttackType SetFinishType(int enemyNum)
    {

        if (_boss != null)
        {
            if (_boss.gameObject.activeSelf)
            {
                if (_boss.BossHp.IsLastHp)
                {
                    return FinishAttackType.OverThree;
                }
            }
        }


        if (enemyNum >= _patarn3EnemyNum)
        {
            return FinishAttackType.OverThree;
        }
        else if (enemyNum == _patarn2EnemyNum)
        {
            return FinishAttackType.Two;
        }
        else
        {
            return FinishAttackType.One;
        }
    }





    public void OnDrwowGizmo(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.yellow;
            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            Gizmos.DrawWireCube(_offset, _boxSize / 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }


}

/// <summary>トドメのさし方の種類 </summary>
public enum FinishAttackType
{
    /// <summary>1体をトドメ </summary>
    One,
    /// <summary>2体をトドメ </summary>
    Two,
    /// <summary>3体以上うをトドメ</summary>
    OverThree,
}

