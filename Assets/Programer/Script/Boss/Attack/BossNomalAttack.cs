using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossNomalAttack
{
    [Header("攻撃時間")]
    [SerializeField] private float _attackTime = 10f;

    [Header("魔法設定")]
    [SerializeField] private List<BossAttackMagicTypes> _magic = new List<BossAttackMagicTypes>();

    [Header("魔法陣を出し終えてから攻撃するまでの時間")]
    private float _waitFireTime = 1f;


    private float _countAttackTime = 0;


    /// <summary>出現させた魔法陣の数</summary>
    private int _chantingCount = 0;

    /// <summary>魔法陣を出し終えてから攻撃するまでの時間計測用</summary>
    private float _countChantingTime = 0;

    private float _countWaitFireTime = 0;

    private bool _isEndAttack = false;

    private bool _isEndAllChanting = false;

    private bool _isWaitFireTime = false;

    private float _countNeedFireTime = 0;

    private int _setMagicNumber = 0;

    private int _useMagicNum = 0;

    private BossAttackMagicTypes _setMagic;
    private BossControl _bossControl;


    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;

        foreach (var a in _magic)
        {
            if (a.PlayerAttribute == _bossControl.EnemyAttibute)
            {
                _setMagic = a;
                break;
            }
        }
    }

    public void AttackFirstSet()
    {
        _chantingCount = 0;
        _useMagicNum = 0;
        _countAttackTime = 0;
        _countNeedFireTime = 0;
        _countChantingTime = 0;
        _isEndAllChanting = false;
        _isWaitFireTime = false;
        _isEndAttack = false;
        _setMagicNumber = Random.Range(0, _setMagic.Magic.Count);
    }


    private void ResetValue()
    {
        _countNeedFireTime = 0;
        _chantingCount = 0;
        _useMagicNum = 0;
        _countChantingTime = 0;
        _isEndAllChanting = false;
        _isWaitFireTime = false;
        _setMagicNumber = Random.Range(0, _setMagic.Magic.Count);
    }


    /// <summary>攻撃中断処理</summary>
    public void StopAttack()
    {
        foreach (var a in _setMagic.Magic[_setMagicNumber].Magic)
        {
            a.MagicCircle.SetActive(false);
        }

        ResetValue();
    }

    public bool DoAttack()
    {

        //攻撃時間を計測する
        if (!_isEndAttack && _countAttackTime > _attackTime)
        {
            _isEndAttack = true;
        }
        else if (!_isEndAttack)
        {
            _countAttackTime += Time.deltaTime;
        }


        if (_isEndAllChanting)
        {
            _countWaitFireTime += Time.deltaTime;

            if (_countWaitFireTime > _waitFireTime)
            {
                _countWaitFireTime = 0;
                _isWaitFireTime = true;
                _isEndAllChanting = false;
            }
            return false;
        }
        else if (_isWaitFireTime)
        {
            _countNeedFireTime += Time.deltaTime;

            if (_countNeedFireTime > _setMagic.Magic[_setMagicNumber].NeedFireTime)
            {
                //魔法を出す
                UseMagic(_useMagicNum);

                _countNeedFireTime = 0;
                _useMagicNum++;

                Debug.Log("使う:" + (_useMagicNum + 1) + "/" + "全部:" + _chantingCount);
                if (_useMagicNum == _chantingCount)
                {
                    ResetValue();
                    if (_isEndAttack)
                    {

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }


            }
        }


        if (_chantingCount == _setMagic.Magic[_setMagicNumber].Magic.Count)
        {
            return false;
        }

        //魔法陣を出すのに必要な時間を計測
        _countChantingTime += Time.deltaTime;


        if (_countChantingTime > _setMagic.Magic[_setMagicNumber].Magic[_chantingCount].NeedTime)
        {
            //魔法陣を出現
            _setMagic.Magic[_setMagicNumber].Magic[_chantingCount].MagicCircle.SetActive(true);

            //計測時間をリセット
            _countChantingTime = 0;

            //魔法陣を出した回数をカウント
            _chantingCount++;
            if (_chantingCount == _setMagic.Magic[_setMagicNumber].Magic.Count)
            {
                _isEndAllChanting = true;
                return false;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    public void UseMagic(int i)
    {
        //  Debug.Log("使う:"+i+"/" + "全部:"+_chantingCount);

        foreach (var effect in _setMagic.Magic[_setMagicNumber].Magic[i].Particle)
        {
            effect.Play();
        }   //エフェクトを再生

        var go = GameObject.Instantiate(_setMagic.Bullet);
        go.transform.position = _setMagic.Magic[_setMagicNumber].Magic[i].MagicCircle.transform.position;
        go.transform.forward = _bossControl.PlayerT.position - go.transform.position;
        go.GetComponent<BossBullet>().Init(_setMagic.Magic[_setMagicNumber].AttackPower);
        _setMagic.Magic[_setMagicNumber].Magic[i].MagicCircle.SetActive(false);
    }


}

[System.Serializable]
public class BossAttackMagicTypes
{
    [SerializeField] private string _name;

    [Header("属性")]
    [SerializeField] private PlayerAttribute _attribute;

    [Header("魔法")]
    [SerializeField] private List<BossAttackmagic> _magic = new List<BossAttackmagic>();

    [Header("弾")]
    [SerializeField] private GameObject _bullet;

    public List<BossAttackmagic> Magic => _magic;
    public PlayerAttribute PlayerAttribute => _attribute;

    public GameObject Bullet => _bullet;
}



[System.Serializable]
public class BossAttackmagic
{
    [SerializeField] private string _name;

    [Header("魔法群")]
    [SerializeField] private List<BossAttackMagicBase> _magics = new List<BossAttackMagicBase>();

    [Header("攻撃力")]
    [SerializeField] private float _attackPower = 1f;

    [Header("魔法１つを打つのにかける時間")]
    [SerializeField] private float _needFireTime = 0.2f;

    public float AttackPower => _attackPower;
    public List<BossAttackMagicBase> Magic => _magics;
    public float NeedFireTime => _needFireTime;
}


[System.Serializable]
public class BossAttackMagicBase
{
    [SerializeField] private string _name;

    [Header("この魔法陣を出すのにかかる時間")]
    [SerializeField] private float _time;

    [Header("魔法陣")]
    [SerializeField] private GameObject _magicCirecle;

    [Header("魔法を出したときのエフェクト")]
    [SerializeField] private List<ParticleSystem> _p = new List<ParticleSystem>();

    public float NeedTime => _time;

    public GameObject MagicCircle => _magicCirecle;
    public List<ParticleSystem> Particle => _p;


}