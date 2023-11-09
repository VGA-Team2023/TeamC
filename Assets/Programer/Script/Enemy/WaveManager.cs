using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField, Tooltip("WaveのPrefab")]
    List<WaveSetting> _waveSettings = new List<WaveSetting>();
    IEnumerator _waveCoroutine;
    int _destroyCount = 0;

    void Start()
    {
        _waveCoroutine = NextWave();
        _waveCoroutine.MoveNext();
    }

    void Update()
    {
        if(_destroyCount <= 0)
        {
            _waveCoroutine.MoveNext();
        }
    }

    IEnumerator NextWave()
    {
        Debug.Log("Wave1");
        _destroyCount = _waveSettings[0].EnemyCount;
        _waveSettings[0].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[0].Enemys)
        {
            summon.gameObject.SetActive(true);
            summon.OnEnemyDestroy += EnemyDestroy;
        }
        yield return 1;
        Debug.Log("Wave2");
        _destroyCount = _waveSettings[1].EnemyCount;
        _waveSettings[1].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[1].Enemys)
        {
            summon.gameObject.SetActive(true);
            summon.OnEnemyDestroy += EnemyDestroy;
        }
        yield return 2;
        Debug.Log("Wave3");
        _destroyCount = _waveSettings[2].EnemyCount;
        _waveSettings[2].Enemy.SetActive(true);
        foreach (var summon in _waveSettings[2].Enemys)
        {
            summon.OnEnemyDestroy += EnemyDestroy;
        }
        yield return 3;
    }

    public void EnemyDestroy()
    {
        _destroyCount--;
    }
}

[Serializable]
class WaveSetting
{
    [SerializeField, Tooltip("敵を配置している親オブジェクト")]
    GameObject _enemys;

    public GameObject Enemy => _enemys;
    public EnemyBase[] Enemys => _enemys.GetComponentsInChildren<EnemyBase>();
    public int EnemyCount => Enemys.Length;
}
