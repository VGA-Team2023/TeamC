using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BossHp
{
    [Header("ボスの体力_各Wave")]
    [SerializeField] private List<float> _waveHp = new List<float>();

    [Header("トドメ後、硬直時間")]
    [SerializeField] private float _finishedWaitTime = 3f;

    private float _countFinishedTime = 0;

    private bool _isFinishComplete = false;

    private bool _isEndWaitTime = false;

    public bool IsFinishComplete => _isFinishComplete;
    public bool IsEndWaitTime => _isEndWaitTime;

    [Header("トドメ可能なダウン時間")]
    [SerializeField] private float _knockDownTIme = 8f;

    [Header("トドメ可能のダウンエフェクト")]
    [SerializeField] private List<ParticleSystem> _knockDownEffect = new List<ParticleSystem>();

    [Header("氷属性のHitエフェクト")]
    [SerializeField] private List<ParticleSystem> _iceHitEffect = new List<ParticleSystem>();

    [Header("草属性のHitエフェクト")]
    [SerializeField] private List<ParticleSystem> _grassHitEffect = new List<ParticleSystem>();

    [Header("氷属性のトドメのエフェクト")]
    [SerializeField] private GameObject _iceFinishEffect;
    [Header("氷属性のトドメのエフェクトのOffset")]
    [SerializeField] private Vector3 _offSetIceFinishEffect = new Vector3(0, -2, 0);

    [Header("草属性のトドメのエフェクト")]
    [SerializeField] private GameObject _grassFinishEffect;
    [Header("氷属性のトドメのエフェクトのOffset")]
    [SerializeField] private Vector3 _offSetGrassFinishEffect = new Vector3(0, -2, 0);


    [SerializeField] private List<ParticleSystem> _effectDark = new List<ParticleSystem>();

    [Header("トドメ可能なレイヤー")]
    [SerializeField] private int _canFinishLayer;

    [Header("トドメ後のレイヤー")]
    [SerializeField] private int _endFinishLayer;

    [Header("通常レイヤー")]
    [SerializeField] private int _enemyLayer;

    private float _countKnockDownTime = 0;

    private int _waveCount = 0;

    private float _nowHp = 0;

    /// <summary>トドメをさせられる状態にいるかどうか</summary>
    private bool _isKnockDown = false;

    /// <summary>一度、トドメをさす状態に持って行けたかどうか</summary>
    private bool _isKnockDowned = false;

    private bool _isFinishNow = false;

    public bool IsKnockDown => _isKnockDown;


    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
        SetNewHp();
    }

    public void CountKnockDownTime()
    {
        if (_isFinishNow) return;
        _countKnockDownTime += Time.deltaTime;
        if (_countKnockDownTime > _knockDownTIme)
        {
            _countKnockDownTime = 0;
            StopFinishAttack();
        }
    }

    /// <summary>新しい体力を設定</summary>
    public void SetNewHp()
    {
        _nowHp = _waveHp[_waveCount];

        _isKnockDown = false;
        _isKnockDowned = false;
    }


    public void StartFinishAttack()
    {
        _isFinishNow = true;
        _isFinishComplete = false;
        _isEndWaitTime = false;

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //ダウンエフェクトを停止
    }

    public void StopFinishAttack()
    {
        //アニメーション設定
        _bossControl.BossAnimControl.IsDown(true);

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //ダウンエフェクトを停止

        _isKnockDown = false;
        _isFinishNow = false;
        _nowHp = _waveHp[_waveCount] / 2;
        _bossControl.gameObject.layer = _enemyLayer;
    }

    /// <summary>トドメを刺された場合</summary>
    public bool CompleteFinishAttack(MagickType magickType)
    {
        if (_waveCount < 2)
        {
            _effectDark[_waveCount].Stop();
        }
        _waveCount++;

        _isFinishComplete = true;

        _isKnockDown = false;
        _isFinishNow = false;



        //アニメーション設定
        _bossControl.BossAnimControl.IsDown(false);


        if (magickType == MagickType.Ice)
        {
            //音
            AudioController.Instance.SE.Play3D(SEState.EnemyFinichHitIce, _bossControl.BossT.position);

            //エフェクト
            var go = GameObject.Instantiate(_iceFinishEffect);
            go.transform.position = _bossControl.BossT.position + _offSetIceFinishEffect;
        }
        else
        {
            //音
            AudioController.Instance.SE.Play3D(SEState.EnemyFinishHitGrass, _bossControl.BossT.position);
            //エフェクト
            var go = GameObject.Instantiate(_grassFinishEffect);
            go.transform.position = _bossControl.BossT.position + _offSetGrassFinishEffect;
        }

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //ダウンエフェクトを停止

        if (_waveCount == _waveHp.Count)
        {
            foreach (var e in _knockDownEffect)
            {
                e.gameObject.SetActive(false);
            }   //ダウンエフェクトを停止

            foreach (var e in _effectDark)
            {
                e.gameObject.SetActive(false);
            }   //もわもわエフェクトを停止


            _bossControl.gameObject.layer = _endFinishLayer;
            return true;
        }
        else
        {
            SetNewHp();
            _bossControl.gameObject.layer = _enemyLayer;
        }


        return false;

    }


    public void CountFinishedWaitTime()
    {
        if (!_isFinishComplete) return;
        _countFinishedTime += Time.deltaTime;

        if (_countFinishedTime > _finishedWaitTime)
        {
            _countFinishedTime = 0;
            _isFinishComplete = false;
            _isEndWaitTime = true;
        }
    }

    public void Damage(float damage, MagickType magickType)
    {
        if (_isFinishComplete) return;

        _nowHp -= damage;

        if (magickType == MagickType.Ice)
        {
            //音
            AudioController.Instance.SE.Play3D(SEState.EnemyHitIcePatternB, _bossControl.BossT.position);

            foreach (var i in _iceHitEffect)
            {
                i.Play();
            }
        }
        else
        {
            //音
            AudioController.Instance.SE.Play3D(SEState.EnemyHitGrassPatternB, _bossControl.BossT.position);

            foreach (var i in _grassHitEffect)
            {
                i.Play();
            }

        }

        if (_nowHp < 0)
        {
            foreach (var e in _knockDownEffect)
            {
                if (!e.isPlaying)
                {
                    e.Play();
                }
            }   //ダウンエフェクトを再生

            //アニメーション設定
            _bossControl.BossAnimControl.IsDown(true);

            _isKnockDown = true;
            _bossControl.gameObject.layer = _canFinishLayer;
        }
    }

}
