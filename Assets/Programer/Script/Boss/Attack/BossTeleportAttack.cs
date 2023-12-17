using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossTeleportAttack
{
    [Header("転移の位置")]
    [SerializeField] private List<Transform> _teleportPoss = new List<Transform>();

    [Header("クールタイム")]
    [SerializeField] private float _coolTime = 1f;

    [Header("転移のエフェクト_氷")]
    [SerializeField] private List<ParticleSystem> _teleportIce = new List<ParticleSystem>();
    [Header("転移のエフェクト_草")]
    [SerializeField] private List<ParticleSystem> _teleportGrass = new List<ParticleSystem>();

    [Header("ダミー_氷")]
    [SerializeField] private GameObject _dummyIce;

    [Header("ダミー_草")]
    [SerializeField] private GameObject _dummyGrass;

    [Header("転移の最大回数")]
    [SerializeField] private int _maxTeleportNum = 4;
    [Header("転移の最小回数")]
    [SerializeField] private int _minTeleportNum = 2;

    [Header("無敵のレイヤー")]
    [SerializeField] private int _layer = 7;
    [Header("通常のレイヤー")]
    [SerializeField] private int _defultLayer = 7;

    private BossControl _bossControl;

    private int _teleportNum = 0;

    private int _setTeleportNum = 0;

    /// <summary>転移した先を記録する用 </summary>
    private List<int> _doPos = new List<int>();

    private float _countCoolTime = 0f;

    private bool _isTeleport = true;

    private int _setPosition = 0;

    public List<ParticleSystem> TeleportIce => _teleportIce;
    public List<ParticleSystem> TeleportGrass => _teleportGrass;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void SetAttack()
    {
        _bossControl.gameObject.layer = _layer;
        _doPos.Clear();
        _teleportNum = 0;

        //転移の実行回数を決定
        _setTeleportNum = Random.Range(_minTeleportNum, _maxTeleportNum);

        _countCoolTime = _coolTime;
    }

    /// <summary>攻撃中断処理</summary>
    public void StopAttack()
    {
        _bossControl.gameObject.layer = _defultLayer;
    }


    public bool DoAttack()
    {
        _countCoolTime += Time.deltaTime;

        if (_countCoolTime >= _coolTime)
        {
            _countCoolTime = 0;

            _teleportNum++;

            //転移先を設定
            SetTeleport();

            //転移を実行
            Teleprt();

            if (_teleportNum == _setTeleportNum)
            {
                _bossControl.gameObject.layer = _defultLayer;
                return true;
            }
        }

        return false;
    }

    /// <summary>転移先を設定 </summary>
    void SetTeleport()
    {
        while (true)
        {
            int r = Random.Range(0, _teleportPoss.Count);

            if (!_doPos.Contains(r))
            {
                _doPos.Add(r);
                _setPosition = r;
                break;
            }
        }
    }

    public void Teleprt()
    {
        //ボスを転移
        _bossControl.gameObject.transform.position = _teleportPoss[_setPosition].position;

        //エフェクトを再生
        if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
        {
            _teleportIce.ForEach(i => i.Play());
        }
        else
        {
            _teleportGrass.ForEach(i => i.Play());
        }

        //ダミーを億
        if (_teleportNum != _setTeleportNum)
        {
            if (_bossControl.EnemyAttibute == PlayerAttribute.Ice)
            {
                var go = GameObject.Instantiate(_dummyIce);
                go.transform.position = _bossControl.gameObject.transform.position;
            }
            else
            {
                var go = GameObject.Instantiate(_dummyGrass);
                go.transform.position = _bossControl.gameObject.transform.position;
            }
        }



        _isTeleport = true;
    }

}
