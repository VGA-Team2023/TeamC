using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopConrol : MonoBehaviour
{
    [Header("ヒットストップのデータ")]
    [SerializeField] private List<HitStopData> _hitStopData = new List<HitStopData>();

    private float _countTime = 0;

    private bool _isHitStop = false;

    private float _setHitStopTime = 0.3f;


    private ControllerVibrationManager _cMl;

    private void Awake()
    {
        _cMl = FindObjectOfType<ControllerVibrationManager>();
    }

    void Update()
    {
        if (GameManager.Instance.PauseManager.IsPause || GameManager.Instance.SpecialMovingPauseManager.IsPaused) return;
        CountHitStopTime();
    }


    /// <summary>ヒットストップの実行時間を計測</summary>
    private void CountHitStopTime()
    {

        if (!_isHitStop) return;

        _countTime += Time.deltaTime;

        if (_countTime > _setHitStopTime)
        {
            EndHitStop();
        }
    }

    public void StartHitStop(HitStopKind hitStopKind)
    {      
        //コントローラーの振動
        if (_cMl != null)
        {
            _cMl.OneVibration(0.5f, 1f, 1f);
        }

        _isHitStop = true;

        foreach (var data in _hitStopData)
        {
            if (data.HitStopKind == hitStopKind)
            {
                ResetHitStopTime(data.HitStopTime);
                GameManager.Instance.SlowManager.OnOffSlow(true);
                return;
            }
        }
    }

    private void EndHitStop()
    {
        GameManager.Instance.SlowManager.OnOffSlow(false);
        _isHitStop = false;
        _countTime = 0;
    }

    /// <summary>ヒットストップの時間を設定</summary>
    /// <param name="setTime"></param>
    public void ResetHitStopTime(float setTime)
    {
        _countTime = 0;
        _setHitStopTime = setTime;
    }

}

[System.Serializable]
public class HitStopData
{
    [SerializeField] private string _name;

    [Header("HitStopの種類")]
    [SerializeField] private HitStopKind _hitStopKind;

    [Header("Hitストップの実行時間")]
    [SerializeField] private float _hitStopTime = 0.3f;

    [Header("HitStopの再生速度")]
    [SerializeField] private float _hitstopScale = 0.3f;

    public HitStopKind HitStopKind => _hitStopKind;

    public float HitStopTime => _hitStopTime;

    public float HitStopScale => _hitstopScale;

}

public enum HitStopKind
{
    FinishAttack,
    Dead,
    BossDeath,

}