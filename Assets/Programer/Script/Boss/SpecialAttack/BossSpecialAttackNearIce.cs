using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossSpecialAttackNearIce
{
    [Header("攻撃用オブジェクト")]
    [SerializeField] private GameObject _attackObject;

    [Header("防御用魔法陣")]
    [SerializeField] private Transform _gard;

    [Header("魔法陣のパーティクル")]
    [SerializeField] private List<ParticleSystem> _chargeIce = new List<ParticleSystem>();

    [Header("魔法陣解除のパーティクル")]
    [SerializeField] private List<ParticleSystem> _chargeIceRemove = new List<ParticleSystem>();

    [Header("防御の回転速度")]
    [SerializeField] private float _rotateGardSpeed = 1;

    [Header("攻撃の位置")]
    [SerializeField] private Transform _movePos;

    [Header("移動速度")]
    [SerializeField] private float _moveSpeed = 10f;

    [Header("回転速度")]
    [SerializeField] private float _rotateSpeed = 10f;

    [Header("溜めの時間")]
    [SerializeField] private float _timeOfAttack = 2;

    [Header("攻撃の間隔")]
    [SerializeField] private float _timeOfAttackCoolTime = 2;

    [Header("攻撃回数")]
    [SerializeField] private int _attackNum = 0;

    [Header("攻撃個数")]
    [SerializeField] private int _attackObjectNum = 3;

    [Header("攻撃の位置")]
    [SerializeField] private List<Transform> _attackPos = new List<Transform>();


    /// <summary>攻撃中かどうか</summary>
    private bool _isAttack = false;

    /// <summary>攻撃のクールタイム中かどうか </summary>
    private bool _isAttackCool = false;

    /// <summary>移動が終わったかどうか</summary>
    private bool _isMoveEnd = false;

    /// <summary>攻撃の溜めの時間計測</summary>
    private float _countAttackTime = 0;

    /// <summary>攻撃の間隔の時間計測</summary>
    private float _countAttackCoolTime = 0;

    /// <summary>攻撃の回数カウント </summary>
    private int _attackCount = 0;

    private Transform _setMovePos;

    private BossControl _bossControl;

    public bool IsAttack => _isAttack;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    /// <summary>
    /// 攻撃開始時に呼ぶ。攻撃の初期設定
    /// </summary>
    public void AttackStart()
    {
        _isAttack = true;

        foreach (var a in _chargeIce)
        {
            a.Play();
        }
    }

    /// <summary>
    /// 攻撃終了時に呼ぶ
    /// </summary>
    public void AttackEnd()
    {
        _gard.gameObject.SetActive(false);

        //回転をやめる
        foreach (var a in _chargeIce)
        {
            a.Stop();
        }

        foreach (var a in _chargeIceRemove)
        {
            a.Play();
        }

    }

    /// <summary>
    /// 攻撃中断時に呼ぶ
    /// </summary>
    public void AttackStop()
    {

    }

    public void Updata()
    {
        if (_isMoveEnd && _isAttack)
        {
            Attack();
        }

        //防御魔法陣の回転
        Vector3 r = _gard.eulerAngles;
        r.y += _rotateGardSpeed;
        _gard.rotation = Quaternion.Euler(r);
    }

    public void Fixed()
    {
        //移動が完了していなかったら移動させる
        if (!_isMoveEnd)
        {
            Move();
        }
    }

    public void Attack()
    {
        if (_isAttackCool)
        {
            _timeOfAttackCoolTime += Time.deltaTime;

            if (_timeOfAttackCoolTime <= _countAttackCoolTime)
            {
                _timeOfAttackCoolTime = 0;
                _isAttackCool = false;

                if (_attackCount == _attackNum)
                {
                    _isAttack = false;
                    AttackEnd();
                }
            }
        }   //クールタイムの計算
        else
        {
            _countAttackTime += Time.deltaTime;

            if (_timeOfAttack <= _countAttackTime)
            {
                _attackCount++;
                _countAttackTime = 0;
                _isAttackCool = true;

                AttackOject();
            }
        }
    }

    /// <summary>
    /// 攻撃のオブジェクトを生成する
    /// </summary>
    void AttackOject()
    {
        List<Transform> poss = _attackPos;

        for (int i = 0; i < _attackObjectNum; i++)
        {
            var r = Random.Range(0, poss.Count);
            var go = GameObject.Instantiate(_attackObject);
            go.transform.position = poss[r].position;
            go.transform.rotation = Quaternion.LookRotation(_bossControl.PlayerT.position - go.transform.position);
            poss.RemoveAt(r);
        }
    }



    public void Move()
    {
        Vector3 dir = _movePos.position - _bossControl.transform.position;
        dir.y = 0;

        _bossControl.Rigidbody.velocity = dir.normalized * _moveSpeed;

        if (Vector3.Distance(_movePos.position, _bossControl.transform.position) < 1)
        {
            _isMoveEnd = true;
            _bossControl.Rigidbody.velocity = Vector3.zero;
        }
    }

    //void CheckPlayerDis()
    //{
    //    Transform setPos = default;
    //    float minDis = 10000;

    //    foreach (var a in _movePos)
    //    {
    //        float dis = Vector3.Distance(a.position, _bossControl.PlayerT.position);

    //        if (dis < minDis)
    //        {
    //            minDis = dis;
    //            setPos = a;
    //        }
    //    }

    //    _setMovePos = setPos;
    //}



}
