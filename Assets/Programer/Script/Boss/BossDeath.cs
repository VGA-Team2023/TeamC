using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossDeath
{
    [Header("消すまでの時間")]
    [SerializeField] private float _destroyTime = 2;

    [Header("消える時のエフェクト")]
    [SerializeField] private GameObject _deathEffect;

    [Header("消えるエフェクトのOffset")]
    [SerializeField] private Vector3 _offset;

    private float _countDestroyTime = 0;



    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void FirstSetting()
    {
        _bossControl.BossAnimControl.DeathPlay();

        var go = GameObject.Instantiate(_deathEffect);
        go.transform.position = _bossControl.BossT.position + _offset;
    }

    public bool CountDestroyTime()
    {
        _countDestroyTime += Time.deltaTime;
        if (_countDestroyTime > _destroyTime)
        {
            Debug.Log("消す");
            return true;
        }
        return false;
    }

}
