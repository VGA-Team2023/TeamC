using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShortChantingMagicBase
{
    [Header("攻撃の回数")]
    [SerializeField] private int _attackMaxNum = 3;

    [Header("攻撃力_貯める前")]
    [SerializeField] private float _powerShortChanting = 1;

    [Header("攻撃力_貯めた")]
    [SerializeField] private float _powerLongChanting = 3;

    [Header("攻撃の位置(魔法陣を使わない場合")]
    [SerializeField] private Transform _attackInstanciatePos;

    [Header("魔法陣")]
    [SerializeField] private List<GameObject> _magick = new List<GameObject>();

    [Header("ための時の魔法陣")]
    [SerializeField] private List<GameObject> _magickTame = new List<GameObject>();



    [Header("パーティクル")]
    [SerializeField] private List<ParticleSystem> _particleSystem = new List<ParticleSystem>();

    [Header("魔法陣から魔法を出したときのエフェクト1")]
    [SerializeField] private List<ParticleSystem> _magickEffect1 = new List<ParticleSystem>();

    [Header("魔法陣から魔法を出したときのエフェクト1")]
    [SerializeField] private List<ParticleSystem> _magickEffect2 = new List<ParticleSystem>();

    [Header("魔法陣から魔法を出したときのエフェクト1")]
    [SerializeField] private List<ParticleSystem> _magickEffect3 = new List<ParticleSystem>();
    [Header("魔法陣から魔法を出したときのエフェクト1")]
    [SerializeField] private List<ParticleSystem> _magickEffect4 = new List<ParticleSystem>();
    [Header("魔法陣から魔法を出したときのエフェクト1")]
    [SerializeField] private List<ParticleSystem> _magickEffect5 = new List<ParticleSystem>();


    [Header("飛ばす魔法のプレハブ_短い詠唱")]
    [SerializeField] private GameObject _prefab;

    [Header("飛ばす魔法のプレハブ_短い詠唱")]
    [SerializeField] private GameObject _prefabLong;

    [Header("止めるまでの時間")]
    [SerializeField] private float _time = 1;

    private float _countTime = 0;

    private bool _isStop = false;

    private PlayerControl _playerControl;

    public int AttackMaxNum => _attackMaxNum;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;

        if (_particleSystem.Count < _attackMaxNum)
        {
            _attackMaxNum = _particleSystem.Count;
        }
    }

    /// <summary>
    /// 魔法陣を準備する
    /// </summary>
    public void SetUpMagick()
    {
        _isStop = false;

        if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;

        if (_magick.Count != 0)
        {
            _magick?.ForEach(i => i.SetActive(true));
        }


        foreach (var a in _particleSystem)
        {
            a.time = 0;
        }

    }


    public void ShowTameMagic(int num, bool isOn)
    {
        //魔法陣無しの場合、テスト
        if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;

        if (_magickTame.Count <= num) return;

        _magickTame[num].SetActive(isOn);
    }

    /// <summary>
    /// 魔法陣を消す
    /// </summary>
    /// <param name="num"></param>
    public void UseMagick(int num, Transform[] enemys, AttackType attackType, bool isAllAttack)
    {
        if (num > _magick.Count) return;

        _magick[num].SetActive(false);

        if (_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack)
        {
            if (num == 0)
            {
                foreach (var a in _magickEffect1)
                {
                    a.Play();
                }
            }
            else if (num == 1)
            {
                foreach (var a in _magickEffect2)
                {
                    a.Play();
                }
            }
            else if (num == 2)
            {
                foreach (var a in _magickEffect3)
                {
                    a.Play();
                }
            }
            else if (num == 3)
            {
                foreach (var a in _magickEffect4)
                {
                    a.Play();
                }
            }
            else if (num == 4)
            {
                foreach (var a in _magickEffect5)
                {
                    a.Play();
                }
            }





        }



        GameObject prefab = _prefab;

        if (isAllAttack)
        {
            prefab = _prefabLong;
        }

        if (enemys.Length == 0)
        {
            var go = UnityEngine.GameObject.Instantiate(prefab);
            go.transform.forward = _playerControl.PlayerT.forward;


            if (_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack)
            {
                go.transform.position = _magick[num].transform.position;
            }
            else
            {
                go.transform.position = _attackInstanciatePos.position;
            }  //魔法陣無しの場合、テスト


            go.TryGetComponent<IMagicble>(out IMagicble magicble);
            magicble.SetAttack(null, _playerControl.PlayerT.forward, attackType, _powerShortChanting);
        }
        else
        {
            foreach (var e in enemys)
            {
                var go = UnityEngine.GameObject.Instantiate(prefab);


                if (_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack)
                {
                    go.transform.forward = e.transform.position - _magick[num].transform.position;
                    go.transform.position = _magick[num].transform.position;
                }
                else
                {
                    go.transform.forward = e.transform.position - _attackInstanciatePos.position;
                    go.transform.position = _attackInstanciatePos.position;
                }  //魔法陣無しの場合、テスト



                go.TryGetComponent<IMagicble>(out IMagicble magicble);
                magicble.SetAttack(e, _playerControl.PlayerT.forward, attackType, _powerLongChanting);
            }
        }
    }


    public void UnSetMagick()
    {
        //魔法陣無しの場合、テスト
        if (!_playerControl.IsNewAttack)
        {
            if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;
            _magick.ForEach(i => i.SetActive(false));
            _magickTame.ForEach(i => i.SetActive(false));
        }
    }


    public void ParticleStopUpdate()
    {
        //魔法陣無しの場合、テスト
        if (!_playerControl.Attack.ShortChantingMagicAttack.IsMahouzinAttack) return;

        if (_isStop) return;

        _countTime += Time.deltaTime;

        if (_countTime > _time && !_isStop)
        {
            _isStop = true;
            _countTime = 0;
            foreach (var a in _particleSystem)
            {
                a.Pause();
            }
        }
    }
}
