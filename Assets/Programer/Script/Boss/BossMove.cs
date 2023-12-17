using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossMove
{
    [Header("移動速度")]
    [SerializeField] private float _moveSpeed = 4f;

    [SerializeField] private float _moveMaxTime = 7f;

    [SerializeField] private float _moveMinTime = 4f;

    [Header("転移の位置")]
    [SerializeField] private List<Transform> _teleportPoss = new List<Transform>();

    private float _setMoveTime = 0;

    private float _moveTimeCount = 0;

    [Header("Offset_右")]
    [SerializeField] private Vector3 _offsetRight;
    [Header("Size_右")]
    [SerializeField] private Vector3 _sizeRight;

    [Header("Offset_左")]
    [SerializeField] private Vector3 _offsetLeft;
    [Header("Size_左")]
    [SerializeField] private Vector3 _sizeLeft;

    [Header("Offset_前")]
    [SerializeField] private Vector3 _offsetFront;
    [Header("Size_前")]
    [SerializeField] private Vector3 _sizeFront;

    [Header("Offset_後")]
    [SerializeField] private Vector3 _offsetBack;
    [Header("Size_後")]
    [SerializeField] private Vector3 _sizeBack;

    [Header("障害物となるレイヤー")]
    [SerializeField] private LayerMask _layer;

    [Header("@Gizmoを表示するかどうか")]
    [SerializeField] private bool _isDrawGizmo = true;

    private BossControl _bossControl;

    private Vector3 _moveDir;


    private MoveDir _moveDirection = MoveDir.Right;


    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
        DirectionSetting();
        SetMoveTime();
    }

    public void StopMove()
    {
        _bossControl.BossAnimControl.SetMoveH(0);
        _bossControl.Rigidbody.velocity = Vector3.zero;
        ChangeDir();
    }

    public void ChangeDir()
    {
        if (_moveDirection == MoveDir.Back)
        {
            int r = Random.Range(0, 2);

            if (r == 0)
            {
                _moveDirection = MoveDir.Right;
            }
            else if (r == 1)
            {
                _moveDirection = MoveDir.Left;
            }
        }
        else if (_moveDirection == MoveDir.Left)
        {
            _moveDirection = MoveDir.Right;
        }
        else
        {
            _moveDirection = MoveDir.Left;
        }

        //アニメーション設定
        if (_moveDirection == MoveDir.Back)
        {
            _bossControl.BossAnimControl.SetMoveH(1);
        }
        else if (_moveDirection == MoveDir.Left)
        {
            _bossControl.BossAnimControl.SetMoveH(-1);
        }
        else
        {
            _bossControl.BossAnimControl.SetMoveH(1);
        }

        _moveTimeCount = 0;
    }

    public void DirectionSetting()
    {
        Vector3 foward = _bossControl.PlayerT.position - _bossControl.BossT.position;

        if (_moveDirection == MoveDir.Back)
        {
            _moveDir = Quaternion.Euler(0, 180, 0) * foward.normalized;
        }
        else if (_moveDirection == MoveDir.Left)
        {
            _moveDir = Quaternion.Euler(0, -90, 0) * foward.normalized;
        }
        else
        {
            _moveDir = Quaternion.Euler(0, 90, 0) * foward.normalized;
        }
        _moveDir.y = 0;
    }

    public void CheckPlayerDir()
    {
        float dis = Vector3.Distance(_bossControl.PlayerT.position, _bossControl.BossT.position);

        if (dis < 2)
        {
            Teleport();
        }
    }

    public void Teleport()
    {
        foreach (var t in _teleportPoss)
        {
            float d = Vector3.Distance(_bossControl.PlayerT.position, t.position);

            if (d > 4)
            {
                _bossControl.BossAttack.TeleportAttack.TeleportIce.ForEach(i => i.Play());

                //エフェクトを再生
                if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
                {
                    _bossControl.BossAttack.TeleportAttack.TeleportIce.ForEach(i => i.Play());
                }
                else
                {
                    _bossControl.BossAttack.TeleportAttack.TeleportGrass.ForEach(i => i.Play());
                }

                _bossControl.BossT.position = t.position;
                return;
            }
        }
    }

    public void Move()
    {
        //  DirectionSetting();
        _bossControl.Rigidbody.velocity = _moveDir * _moveSpeed;
    }

    public void SetMoveTime()
    {
        _setMoveTime = Random.Range(_moveMinTime, _moveMaxTime);
    }

    public void CountMoveTime()
    {
        _moveTimeCount += Time.deltaTime;

        if (_moveTimeCount > _setMoveTime)
        {
            SetMoveTime();
            Debug.Log("ChangeToTime");
            ChangeDir();
            DirectionSetting();
        }
    }

    public void CheckWall()
    {
        if (_moveDirection == MoveDir.Back)
        {
            if (Search(CheckSide.Back))
            {
                Teleport();
            }
        }
        else if (_moveDirection == MoveDir.Left)
        {
            if (Search(CheckSide.Left))
            {
                Teleport();
            }
        }
        else
        {
            if (Search(CheckSide.Left))
            {
                Teleport();
            }
        }
    }

    public bool Search(CheckSide checkSide)
    {
        Vector3 offSet;
        Vector3 size;

        if (checkSide == CheckSide.Front)
        {
            offSet = _offsetFront;
            size = _sizeFront;
        }
        else if (checkSide == CheckSide.Back)
        {
            offSet = _offsetBack;
            size = _sizeBack;
        }
        else if (checkSide == CheckSide.Right)
        {
            offSet = _offsetRight;
            size = _sizeRight;
        }
        else
        {
            offSet = _offsetLeft;
            size = _sizeLeft;
        }

        var posX = _bossControl.BossT.position.x + offSet.x;
        var posY = _bossControl.BossT.position.y + offSet.y;
        var posz = _bossControl.BossT.position.z + offSet.z;

        Quaternion r = Quaternion.Euler(0, _bossControl.BossT.eulerAngles.y, 0);

        var d = Physics.OverlapBox(new Vector3(posX, posY, posz), size, r, _layer);

        if (d.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDrwowGizmo(Transform origin)
    {
        if (_isDrawGizmo)
        {
            Gizmos.color = Color.cyan;
            Quaternion r = Quaternion.Euler(0, origin.eulerAngles.y, 0);
            Gizmos.matrix = Matrix4x4.TRS(origin.position, r, origin.localScale);
            Gizmos.DrawWireCube(_offsetRight, _sizeRight / 2);
            Gizmos.DrawWireCube(_offsetLeft, _sizeLeft / 2);
            Gizmos.DrawWireCube(_offsetFront, _sizeFront / 2);
            Gizmos.DrawWireCube(_offsetBack, _sizeBack / 2);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
    }

    public enum MoveDir
    {
        Back,
        Right,
        Left,
    }

    public enum CheckSide
    {
        Front,
        Back,
        Right,
        Left,
    }



}
