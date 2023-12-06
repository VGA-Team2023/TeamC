using CriWare;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>シングルトン化</summary>
    public static AudioManager _instance;
    [Header("設定")]
    [SerializeField, Tooltip("BGMのボリューム"), Range(0, 1)] int _bgmVolume = 1;
    [SerializeField, Tooltip("SEのボリューム"), Range(0, 1)] int _seVolume = 1;
    [SerializeField, Tooltip("PlayerおよびEnemyのSEに3D機能をつけるかどうか")] bool _is3DPositioning;
    [SerializeField, Tooltip("現在のシーン(BGM)")] BGMState _sceneBGMState;
    [Space]
    [Space]
    [Header("値固定(以下の値は固定のままで)")]
    [Header("BGM")]
    [SerializeField, Tooltip("各シーン用のBGMデータ")] BGM[] bgms;
    [Header("Player")]
    [SerializeField, Tooltip("PlayerのSEデータ")] PlayerSE[] playerSEDates;
    [Header("Enemy")]
    [SerializeField, Tooltip("EnemyのSEデータ")] EnemyActionSE[] enemyActionSEDates;
    [SerializeField, Tooltip("EnemyのSEデータ")] EnemyHitSE[] enemyHitSEDates;
    [Header("System")]
    [SerializeField, Tooltip("ボタン音のSEデータ")] ButtonPushSEProperty[] buttonPushSE;
    /// <summary>BGM専用</summary>
    CriAtomSource _bgmSource;
    /// <summary>システム専用</summary>
    CriAtomSource _systemSeSource;
    /// <summary>Player専用</summary>
    CriAtomSource _playerSeSource;
    /// <summary>Playerの属性</summary>
    PlayerAttribute _playerAttribute;
    public static AudioManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (!_instance)
                {
                    Debug.LogError("Scene内に" + typeof(AudioManager).Name + "をアタッチしているGameObjectがありません");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            PlayerAttributeSet();
            //BGM再生
            BGMPlay(this._sceneBGMState);
            DontDestroyOnLoad(this);
        }

        if(_instance != this)
        {
            PlayerAttributeSet();
            //シーン遷移した後のBGM切り替え
            _instance.BGMPlay(this._sceneBGMState);
            Destroy(this);
        }
    }
    /// <summary>Playerの属性をGameManagerから参照</summary>
    public void PlayerAttributeSet()
    {
        _instance._playerAttribute = _playerAttribute = GameManager.Instance.PlayerAttribute;
    }

    /// <summary>PlayerのSEを再生するもの</summary>
    /// <param name="se">再生するSE</param>
    public void PlayerSEPlay(PlayerAttackSEState se)
    {
        if (_playerSeSource == null)
        {
            _playerSeSource = transform.GetChild(2).GetComponent<CriAtomSource>();
        }
        int index = (int)se;
        string cueName = "";
        switch (_playerAttribute)
        {
            case PlayerAttribute.Ice:
                cueName = playerSEDates[index].AttributeSE.IceSoundCueName;
                break;
            case PlayerAttribute.Grass:
                cueName = playerSEDates[index].AttributeSE.GrassSoundCueName;
                break;
        }
        _playerSeSource.volume = _seVolume;
        _playerSeSource.Play(cueName);
    }
    /// <summary>再生中のPlayerのSEを停止するもの</summary>
    public void PlayerSEStop()
    {
        if (_playerSeSource == null)
        {
            _playerSeSource = transform.GetChild(2).GetComponent<CriAtomSource>();
        }
        _playerSeSource.Stop();
    }
    /// <summary>EnemyのActionのSEを再生するもの</summary>
    /// <param name="se">再生するSE</param>
    public void EnemyActionSEPlay(GameObject enemy, EnemyActionSEState se)
    {
        CriAtomSource _seSource = GameObjectCriAtomSourceSet(enemy);
        int index = (int)se;
        _seSource.Play(enemyActionSEDates[index].SoundCueName);
    }
    /// <summary>Playerの攻撃がEnemyに当たった時のSEを再生するもの</summary>
    /// <param name="se">再生するSE</param>
    /// <param name="enemy">自分のGameObject</param>
    public void EnemyHitSEPlay(GameObject enemy, EnemyHitSEState se)
    {
        CriAtomSource _seSource = GameObjectCriAtomSourceSet(enemy);
        int index = (int)se;
        string cueName = "";
        switch(_playerAttribute)
        {
            case PlayerAttribute.Ice:
                cueName = enemyHitSEDates[index].AttributeSE.IceSoundCueName;
                break;
            case PlayerAttribute.Grass:
                cueName = enemyHitSEDates[index].AttributeSE.GrassSoundCueName;
                break;
        }
        _seSource.Play(cueName);
    }

    /// <summary>CriAtomSourceコンポーネントの所持確認とセット</summary>
    /// <param name="target">対象のGameObject</param>
    /// <returns>対象のCriAtomSourceコンポーネントのデータ</returns>
    CriAtomSource GameObjectCriAtomSourceSet(GameObject target)
    {
        CriAtomSource _seSource = target.GetComponent<CriAtomSource>();
        if (_seSource == null)
        {
            _seSource = target.AddComponent<CriAtomSource>();
        }
        if (_is3DPositioning)
        {
            _seSource.use3dPositioning = true;
        }
        _seSource.volume = _seVolume;
        return _seSource;
    }

    /// <summary>BGMの切り替えと再生を行うメソッド</summary>
    /// <param name="bgm">BGMを流す時のシーンの状態</param>
    public void BGMPlay(BGMState bgm)
    {
        if (_bgmSource == null)
        {
            _bgmSource = transform.GetChild(0).GetComponent<CriAtomSource>();
        }
        _bgmSource.Stop();
        _sceneBGMState = bgm;
        int index = (int)bgm;
        _bgmSource.Play(bgms[index].SoundCueName);
    }

    /// <summary>ボタン音を再生するメソッド</summary>
    /// <param name="applyButton">True時は決定時の音・False時はキャンセル時の音</param>
    public void ButtonSEPlay(bool applyButton)
    {
        ButtonPushSE seState = applyButton ? ButtonPushSE.Apply : ButtonPushSE.Cancel;
        int index = (int)seState;
        if (_systemSeSource == null)
        {
            _systemSeSource = transform.GetChild(1).GetComponent<CriAtomSource>();
        }
        _systemSeSource.volume = _seVolume;
        _systemSeSource.Play(buttonPushSE[index].SoundCueName);
    }

    public void BGMVolumeChange(int volume)
    {
        _bgmVolume = volume;
        _bgmSource.volume = _bgmVolume;
    }

    public void SEVolumeChnage(int volume)
    {
        _seVolume = volume;
    }
}
