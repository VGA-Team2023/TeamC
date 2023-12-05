using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSettings : MonoBehaviour, IPause, ISlow, ISpecialMovingPause
{
    [Header("一定時間経過で削除するかどうか")]
    [SerializeField] private bool _isDestroy = false;

    [Header("削除するまでの時間")]
    [SerializeField] private float _destroyTime = 1;

    [Header("パーティクル")]
    [SerializeField] private List<SettingEffect> _effects = new List<SettingEffect>();

    private List<bool> _isPlays = new List<bool>();
    private List<bool> _isPlaysMoviePause = new List<bool>();

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
        foreach (var effect in _effects)
        {
            foreach (var effect2 in effect.Effects)
            {
                _defultSpeed.Add(effect2.main.simulationSpeed);
            }
        }
    }

    void ISpecialMovingPause.Pause()
    {
        _isMoviePause = true;
        _isPlaysMoviePause.Clear();

        foreach (var effect in _effects)
        {
            foreach (var effect2 in effect.Effects)
            {
                _isPlaysMoviePause.Add(effect2.isPlaying);
                effect2.Pause();
            }
        }
    }

    void ISpecialMovingPause.Resume()
    {
        _isMoviePause = false;
        foreach (var e in _effects)
        {
            for (int i = 0; i < e.Effects.Count; i++)
            {
                if (_isPlaysMoviePause[i])
                {
                    e.Effects[i].Play();
                }
            }
        }
    }


    public void OffSlow()
    {
        foreach (var effect in _effects)
        {
            for (int i = 0; i < effect.Effects.Count; i++)
            {
                var main = effect.Effects[i].main;
                main.simulationSpeed = _defultSpeed[i];
            }
        }
    }

    public void OnSlow(float slowSpeedRate)
    {
        foreach (var effect in _effects)
        {
            foreach (var effect2 in effect.Effects)
            {
                var main = effect2.main;
                main.simulationSpeed = slowSpeedRate;
            }
        }
    }

    public void Pause()
    {
        _isPause = true;
        _isPlays.Clear();

        foreach (var effect in _effects)
        {
            foreach (var effect2 in effect.Effects)
            {
                _isPlays.Add(effect2.isPlaying);
                effect2.Pause();
            }
        }
    }

    public void Resume()
    {
        _isPause = false;
        foreach (var e in _effects)
        {
            for (int i = 0; i < e.Effects.Count; i++)
            {
                if (_isPlays[i])
                {
                    e.Effects[i].Play();
                }
            }
        }
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