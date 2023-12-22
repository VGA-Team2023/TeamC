using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ウェーブを管理するクラス
/// </summary>
public class WaveManager : MonoBehaviour
{
    [SerializeField, Tooltip("WaveのPrefab")]
    List<WaveSetting> _waveSettings = new List<WaveSetting>();
    IEnumerator _waveCoroutine;
    int _waveCount;
    int _destroyCount = 0;
    public int DestroyCount
    {
        get => _destroyCount;
        set
        {
            //敵が消えた時に呼ばれる
            _destroyCount = value;
            //Waveに設定した敵が全部消えたら
            if (_destroyCount <= 0)
            {
                //設定してるWaveが全て呼ばれたらシーン遷移
                _waveCount++;
                _waveCoroutine.MoveNext();
                Debug.Log("カウント:" + _waveCount);
                if (_waveCount == _waveSettings.Count)
                {
                    //リザルト状態に変更
                    var _gameManager = FindObjectOfType<GameManager>();
                    _gameManager.GameEndWaitCall();
                }
            }
        }
    }

    void Start()
    {
        //スタート時に一度呼ぶ
        _waveCoroutine = NextWave();
        _waveCoroutine.MoveNext();
    }

    //MoveNextを呼ばれるごとに呼び出すWaveの配列が変わる
    IEnumerator NextWave()
    {
        DestroyCount = _waveSettings[0].EnemyCount;
        _waveSettings[0].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[0].Enemys)
        {
            //Observerで敵がやられたか監視する
            if(summon.TryGetComponent(out LongAttackEnemy longEnemy))
            {
                summon.OnEnemyFinish += LongEnemyDestroy;
            }
            if (summon.TryGetComponent(out MeleeAttackEnemy meleeEnemy))
            {
                summon.OnEnemyFinish += MeleeEnemyDestroy;
            }
        }
        yield return 1;
        DestroyCount = _waveSettings[1].EnemyCount;
        _waveSettings[1].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[1].Enemys)
        {
            //Observerで敵がやられたか監視する
            if (summon.TryGetComponent(out LongAttackEnemy longEnemy))
            {
                summon.OnEnemyFinish += LongEnemyDestroy;
            }
            if (summon.TryGetComponent(out MeleeAttackEnemy meleeEnemy))
            {
                summon.OnEnemyFinish += MeleeEnemyDestroy;
            }
        }
        yield return 2;
        DestroyCount = _waveSettings[2].EnemyCount;
        Debug.Log(DestroyCount);
        _waveSettings[2].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[2].Enemys)
        {
            //Observerで敵がやられたか監視する
            if (summon.TryGetComponent(out LongAttackEnemy longEnemy))
            {
                summon.OnEnemyFinish += LongEnemyDestroy;
            }
            if (summon.TryGetComponent(out MeleeAttackEnemy meleeEnemy))
            {
                summon.OnEnemyFinish += MeleeEnemyDestroy;
            }
            if(summon.TryGetComponent(out BossControl boss))
            {
                summon.OnEnemyFinish += BossDestroy;
            }
        }
        yield return 3;
    }

    public void BossDestroy()
    {
        DestroyCount--;
    }

    //遠距離敵が消えた時に呼ばれる関数
    public void LongEnemyDestroy()
    {
        DestroyCount--;
        GameManager.Instance.ScoreManager.LongEnemyDefeatedNum++;
    }
    //近距離敵が消えた時に呼ばれる関数
    public void MeleeEnemyDestroy()
    {
        DestroyCount--;
        GameManager.Instance.ScoreManager.ShortEnemyDefeatedNum++;
    }
}

/// <summary>
/// 子オブジェクトに敵が格納されているWavePrefabを設定する
/// </summary>
[Serializable]
class WaveSetting
{
    [SerializeField, Tooltip("敵を配置している親オブジェクト")]
    GameObject _enemys;

    //設定されたWavePrefabから各情報を引き出す
    public GameObject Enemy => _enemys;
    public EnemyBase[] Enemys => _enemys.GetComponentsInChildren<EnemyBase>();
    public int EnemyCount => Enemys.Length;
}
