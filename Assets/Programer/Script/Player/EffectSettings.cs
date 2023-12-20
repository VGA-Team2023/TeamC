using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CriWare.CriAtomExMic;

public class EffectSettings : MonoBehaviour, IPause, ISlow, ISpecialMovingPause
{
    [Header("一定時間経過で削除するかどうか")]
    [SerializeField] private bool _isDestroy = false;

    [Header("削除するまでの時間")]
    [SerializeField] private float _destroyTime = 1;

    [Header("パーティクル")]
    [SerializeField] private List<SettingEffect> _effects = new List<SettingEffect>();

    /// <summary>パーティクルが再生しているかどうか</summary>
    private List<List<bool>> _isPlays = new List<List<bool>>();

    private List<List<bool>> _isMoviePlays = new List<List<bool>>();

    private List<List<float>> _isPlaySpeeds = new List<List<float>>();

    private int _indexPause = 0;
    private int _indexMoviePause = 0;
    private int _indexSlow = 0;

    private float _countDestroyTime = 0;

    private bool _isPause = false;
    private bool _isMoviePause = false;

    private List<float> _defultSpeed = new List<float>();

    private void OnEnable()
    {
        GameManager.Instance.PauseManager.Add(this);
        GameManager.Instance.SlowManager.Add(this);
        GameManager.Instance.SpecialMovingPauseManager.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.PauseManager.Remove(this);
        GameManager.Instance.SlowManager.Remove(this);
        GameManager.Instance.SpecialMovingPauseManager.Resume(this);
    }

    private void Awake()
    {
        _indexPause = _effects.Count;

        foreach (var effect in _effects)
        {
            List<float> speed = new List<float>();

            foreach (var effect2 in effect.Effects)
            {
                speed.Add(effect2.main.simulationSpeed);
            }
            _isPlaySpeeds.Add(speed);
        }
    }

    void ISpecialMovingPause.Pause()
    {
        _isMoviePause = true;

        foreach (var effect in _effects)
        {
            List<bool> list = new List<bool>();
            foreach (var effect2 in effect.Effects)
            {
                list.Add(effect2.isPlaying);
                effect2.Pause();
            }
            _isMoviePlays.Add(list);
        }
    }

    void ISpecialMovingPause.Resume()
    {
        _isMoviePause = false;
        _indexMoviePause = 0;
        foreach (var e in _effects)
        {
            for (int i = 0; i < e.Effects.Count; i++)
            {
                if (_isMoviePlays[_indexMoviePause][i])
                {
                    e.Effects[i].Play();
                }
            }
            _indexMoviePause++;
        }
        _isMoviePlays.Clear();
    }


    public void OffSlow()
    {
        _indexSlow = 0;

        foreach (var effect in _effects)
        {
            for (int i = 0; i < effect.Effects.Count; i++)
            {
                var main = effect.Effects[i].main;
                main.simulationSpeed = _isPlaySpeeds[_indexSlow][i];
            }
            _indexSlow++;
        }
    }

    public void OnSlow(float slowSpeedRate)
    {
        foreach (var effect in _effects)
        {
            foreach (var effect2 in effect.Effects)
            {
                effect2.playbackSpeed = slowSpeedRate;
            }
        }
    }

    public void Pause()
    {
        _isPause = true;

        foreach (var effect in _effects)
        {
            List<bool> list = new List<bool>();
            foreach (var effect2 in effect.Effects)
            {
                list.Add(effect2.isPlaying);
                effect2.Pause();
            }
            _isPlays.Add(list);
        }
    }

    public void Resume()
    {
        _isPause = false;

        _indexPause = 0;

        foreach (var e in _effects)
        {
            for (int i = 0; i < e.Effects.Count; i++)
            {
                if (_isPlays[_indexPause][i] == true)
                {
                    e.Effects[i].Play();
                }
            }

            _indexPause++;
        }
        _isPlays.Clear();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!_isDestroy) return;

        if (_isPause || _isMoviePause) return;

        _countDestroyTime += Time.deltaTime;


        if (_destroyTime < _countDestroyTime)
        {
            Destroy(gameObject);
        }
    }



}

[System.Serializable]
public class SettingEffect
{
    [SerializeField] private string _name;

    [Header("パーティクル")]
    [SerializeField] private List<ParticleSystem> _effects = new List<ParticleSystem>();

    public List<ParticleSystem> Effects => _effects;

}