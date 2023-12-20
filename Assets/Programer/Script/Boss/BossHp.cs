using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BossHp
{
    [Header("ボスの体力_各Wave")]
    [SerializeField] private List<float> _waveHp = new List<float>();

    [Header("トドメ可能なダウン時間")]
    [SerializeField] private float _knockDownTIme = 8f;

    [Header("トドメ可能のダウンエフェクト")]
    [SerializeField] private List<ParticleSystem> _knockDownEffect = new List<ParticleSystem>();

    [Header("氷属性のHitエフェクト")]
    [SerializeField] private List<ParticleSystem> _iceHitEffect = new List<ParticleSystem>();

    [Header("草属性のHitエフェクト")]
    [SerializeField] private List<ParticleSystem> _grassHitEffect = new List<ParticleSystem>();

    [Header("氷属性のトドメのエフェクト")]
    [SerializeField] private List<ParticleSystem> _iceFinishEffect = new List<ParticleSystem>();

    [Header("草属性のトドメのエフェクト")]
    [SerializeField] private List<ParticleSystem> _grassFinishEffect = new List<ParticleSystem>();

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
        _isKnockDown = false;
        _isFinishNow = false;

        //アニメーション設定
        _bossControl.BossAnimControl.IsDown(false);


        if (magickType == MagickType.Ice)
        {
            //音
            AudioController.Instance.SE.Play3D(SEState.EnemyFinichHitIce, _bossControl.BossT.position);
            foreach (var i in _iceFinishEffect)
            {
                i.Play();
            }
        }
        else
        {
            //音
            AudioController.Instance.SE.Play3D(SEState.EnemyFinishHitGrass, _bossControl.BossT.position);
            foreach (var i in _grassFinishEffect)
            {
                i.Play();
            }
        }

        foreach (var e in _knockDownEffect)
        {
            e.Stop();
        }   //ダウンエフェクトを停止

        if (_waveCount == _waveHp.Count)
        {
            Debug.Log("死亡");
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

    public void Damage(float damage, MagickType magickType)
    {
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
