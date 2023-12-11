using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AttackMagicBase
{
    [Header("@魔法の属性")]
    [SerializeField] private MagickType _magicType = MagickType.Ice;

    [Header("@魔法の位置と、時間設定")]
    [SerializeField] private List<Magic> _magickData = new List<Magic>();

    [Header("攻撃力")]
    [SerializeField] private float _powerShortChanting = 1;

    [Header("飛ばす魔法の弾プレハブ")]
    [SerializeField] private GameObject _prefab;

    [Header("連撃の実行時間")]
    [SerializeField] private List<float> _attackContinueTimes = new List<float>();

    /// <summary>連撃の実行時間を計測</summary>
    private float _countAttackContinueTime = 0;

    private float _countOneAttackTime = 0;

    private float _countCoolTime = 0;

    private bool _isCoolTime = true;

    private bool _isAttackNow = false;

    public bool IsAttackNow { get => _isAttackNow; set => _isAttackNow = value; }
    private float _useMagicTime = 0.1f;

    /// <summary>魔法を放っている時間を計測 </summary>
    private float _countUsemagicTime = 0;

    /// <summary>現在連撃目なのか</summary>
    private int _nowContunueNumber = 0;

    /// <summary>なん連撃まで可能か </summary>
    private int _maxContunueNumber = 1;

    /// <summary>全連撃内での、魔法陣を使った回数</summary>
    private int _useMagicCount = 0;

    private bool _isFireNow = false;

    private int _attackCount = 0;
    public int AttackCount { get => _attackCount; set => _attackCount = value; }
    private int _attackMaxNum = 3;

    private Transform[] _enemys = new Transform[1];
    public Transform[] Enemys { get => _enemys; set => _enemys = value; }
    /// <summary>ための時間を計測</summary>
    private float _countChargeTime = 2f;

    private int _setUpMagicCount = 0;

    /// <summary>設定されている全ての魔法陣を出したかどうか</summary>
    private bool _isChantingAllMagic = false;

    private PlayerControl _playerControl;

    public int AttackMaxNum => _attackMaxNum;
    public List<Magic> MagickData => _magickData;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;

        _attackMaxNum = _magickData.Count;

        foreach (var d in _magickData)
        {
            foreach (var f in d.MagickData)
            {
                if (f.AttackContinuousSetNumber == 0)
                {
                    f.AttackContinuousSetNumber = 1;
                }
            }
        }
    }

    /// <summary>
    /// 魔法陣を準備する。攻撃始めに呼ぶ
    /// </summary>
    public void SetUpMagick()
    {
        _isChantingAllMagic = false;
        _setUpMagicCount = 0;
        _countChargeTime = 0;


        _countAttackContinueTime = 0;
        _isAttackNow = false;
        _useMagicCount = 0;
        _nowContunueNumber = 0;
        _maxContunueNumber = 1;
        _isFireNow = false;

        _countCoolTime = 0;
    }

    /// <summary>
    /// 長押し時間に応じて魔法陣を出していく
    /// </summary>
    public void SetUpChargeMagic(int attackCount)
    {
        //魔法陣を全て出していたらこれ以上何もしない
        if (_isChantingAllMagic || attackCount > _magickData.Count) return;

        //ボタンの押し込み時間を計測
        _countChargeTime += Time.deltaTime;

        //魔法陣を1つ出す
        if (_countChargeTime > _magickData[attackCount - 1].MagickData[_setUpMagicCount].ChargeTime)
        {
            //魔法陣を出現させる
            _magickData[attackCount - 1].MagickData[_setUpMagicCount].MagicCircle.SetActive(true);

            //連撃数の更新
            if (_magickData[attackCount - 1].MagickData[_setUpMagicCount].AttackContinuousSetNumber > _maxContunueNumber)
            {
                _maxContunueNumber = _magickData[attackCount - 1].MagickData[_setUpMagicCount].AttackContinuousSetNumber;
                Debug.Log("MAX:" + _maxContunueNumber);
            }

            _setUpMagicCount++;

            _countChargeTime = 0;

            if (_magickData[attackCount - 1].MagickData.Count == _setUpMagicCount)
            {
                _isChantingAllMagic = true;
            }
        }
    }

    public void UseMagics()
    {
        if (!_isAttackNow) return;

        //連撃の実行時間を計測
        _countAttackContinueTime += Time.deltaTime;


        if (_magickData[_attackCount - 1].AttackContinueTimes[_nowContunueNumber] <= _countAttackContinueTime && !_isFireNow)
        {
            _isFireNow = true;
            _countAttackContinueTime = 0;

            _nowContunueNumber++;

            _playerControl.Animator.Play("Attack" + _nowContunueNumber);


            //カメラの振動
            _playerControl.CameraControl.ShakeCamra(CameraType.AttackCharge, CameraShakeType.AttackNomal);

            //コントローラーの振動
            _playerControl.ControllerVibrationManager.OneVibration(0.2f, 0.5f, 0.5f);

            //魔法を使う
            Magic();


            //連撃が終了　== 攻撃を終了
            if (_nowContunueNumber == _maxContunueNumber)
            {
                //カメラ変更
                _playerControl.CameraControl.UseDefultCamera(true);
                _isCoolTime = false;
                _isAttackNow = false;
            }

        }
    }

    public void CountCoolTime()
    {
        if (_isCoolTime) return;
        _countCoolTime += Time.deltaTime;

        if (_countCoolTime > 0.5f)
        {
            _countCoolTime = 0;
            _isCoolTime = true;




            _useMagicCount = 0;
            _nowContunueNumber = 0;

            _playerControl.Attack2.CheckEnd();
        }
    }

    public void Magic()
    {
        //連撃番号と同じ魔法陣を使う
        foreach (var m in _magickData[_attackCount - 1].MagickData)
        {
            if (_nowContunueNumber == m.AttackContinuousSetNumber)
            {
                m.MagicCircle.SetActive(false);
                m.UseMagicparticle.ForEach(i => i.Play());


                AttackType attackType = AttackType.ShortChantingMagick;
                if (_isChantingAllMagic) attackType = AttackType.LongChantingMagick; 


                //魔法のプレハブを出す
                var go = UnityEngine.GameObject.Instantiate(_prefab);
                go.transform.position = m.MagicCircle.transform.position;

                if (_playerControl.LockOn.IsLockOn)
                {
                    if (_playerControl.LockOn.NowLockOnEnemy != null)
                    {
                        go.transform.forward = _playerControl.LockOn.NowLockOnEnemy.transform.position - go.transform.position;
                        go.TryGetComponent<IMagicble>(out IMagicble magicble);
                        magicble.SetAttack(_playerControl.LockOn.NowLockOnEnemy.transform, _playerControl.PlayerT.forward, attackType, _powerShortChanting);
                    }
                }
                else
                {
                    if (_enemys.Length == 0)
                    {
                        go.transform.forward = _playerControl.PlayerT.forward;
                        go.TryGetComponent<IMagicble>(out IMagicble magicble);
                        magicble.SetAttack(null, _playerControl.PlayerT.forward, attackType, _powerShortChanting);
                    }
                    else
                    {
                        if (_enemys[_useMagicCount % _enemys.Length] == null)
                        {
                            go.transform.forward = _playerControl.PlayerT.forward;
                            go.TryGetComponent<IMagicble>(out IMagicble magicble);
                            magicble.SetAttack(null, _playerControl.PlayerT.forward, attackType, _powerShortChanting);
                        }
                        else
                        {
                            go.transform.forward = _enemys[_useMagicCount % _enemys.Length].transform.position - go.transform.position;
                            go.TryGetComponent<IMagicble>(out IMagicble magicble);
                            magicble.SetAttack(_enemys[_useMagicCount % _enemys.Length], _playerControl.PlayerT.forward, attackType, _powerShortChanting);
                        }
                    }
                }

                //サウンドを再生
                if (_playerControl.PlayerAttributeControl.PlayerAttribute == PlayerAttribute.Ice)
                {
                    _playerControl.PlayerAudio.Fire(1, true);
                }
                else
                {
                    _playerControl.PlayerAudio.Fire(1, false);
                }
              //  AudioManager.Instance.PlayerSEPlay(PlayerAttackSEState.Shoot);
                //AudioManager.Instance.PlayerSEPlay(PlayerAttackSEState.Trail);
            }
            _isFireNow = false;
            _useMagicCount++;


        }
    }



    /// <summary>現在出している魔法を中断させる</summary>
    public void StopMagic(int attackCount)
    {
        for (int i = 0; i < _setUpMagicCount; i++)
        {
            //魔法陣を消す
            _magickData[attackCount - 1].MagickData[i].MagicCircle.SetActive(false);
        }
        _isAttackNow = false;
        _playerControl.Attack2.IsCanNextAttack = true;
        _nowContunueNumber = 0;
        _maxContunueNumber = 1;
        _isFireNow = false;
    }
}

[System.Serializable]
public class Magic
{
    [SerializeField] private string _name;

    [Header("連撃数")]
    [SerializeField] private int _attackContinuous;

    [Header("魔法群")]
    [SerializeField] private List<MagickData> _magickDatas = new List<MagickData>();

    [Header("連撃の実行時間")]
    [SerializeField] private List<float> _attackContinueTimes = new List<float>(5);

    public List<float> AttackContinueTimes => _attackContinueTimes;
    public int AttackContinuous => _attackContinuous;
    public List<MagickData> MagickData => _magickDatas;
}


[System.Serializable]
public class MagickData
{
    [SerializeField] private string _name;
    [Header("連撃の番号")]
    [SerializeField] private int _attackContinuous;

    [Header("@魔法陣1つ出すのにかける時間")]
    [SerializeField] private float _chargeTime = 0.3f;

    [Header("@魔法陣のオブジェクト")]
    [SerializeField] private GameObject _magickCircle;

    [Header("@魔法を出したときのエフェクト")]
    [SerializeField] private List<ParticleSystem> _particlesUseMagic = new List<ParticleSystem>();

    //[Header("魔法を出す位置()")]
    //  [SerializeField] 
    private Transform _muzzlePos;

    // [Header("魔法陣のパーティクル")]
    //  [SerializeField]
    private List<ParticleSystem> _particles = new List<ParticleSystem>();



    // [Header("魔法陣を出したときのエフェクトオブジェクト")]
    // [SerializeField]
    private GameObject _effect;

    public int AttackContinuousSetNumber { get => _attackContinuous; set => _attackContinuous = value; }
    public GameObject Effect => _effect;
    public List<ParticleSystem> UseMagicparticle => _particlesUseMagic;
    public Transform MuzzlePos => _muzzlePos;
    public List<ParticleSystem> ParticleSystem => _particles;
    public float ChargeTime => _chargeTime;
    public GameObject MagicCircle => _magickCircle;
}