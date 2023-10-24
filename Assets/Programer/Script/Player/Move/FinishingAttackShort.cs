using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinishingAttackShort
{
    [Header("[-=====近い距離のトドメのエフェクト設定===-]")]
    [SerializeField] private FinishAttackNearMagic _finishAttackNearMagic;

    [Header("トドメをさす時間_銃")]
    [SerializeField] private float _finishTime = 1f;

    [Header("判定_Offset_遠距離")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, 3);

    [Header("判定_Size_遠距離")]
    [SerializeField] private Vector3 _boxSize = new Vector3(10, 10, 10);

    [Header("Gizmoを表示するかどうか")]
    [SerializeField] private bool _isDrawGizmo = true;


    public Vector3 Offset => _offset;
    public Vector3 BoxSize => _boxSize;

    private PlayerControl _playerControl;
    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public float FinishTime => _finishTime;

    public FinishAttackNearMagic FinishAttackNearMagic => _finishAttackNearMagic;



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
