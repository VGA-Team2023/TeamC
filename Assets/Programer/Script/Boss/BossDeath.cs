using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossDeath
{
    [Header("消すまでの時間")]
    [SerializeField] private float _destroyTime = 6;

    [Header("消える時のエフェクト")]
    [SerializeField] private GameObject _deathEffect;

    [Header("消えるエフェクトのOffset")]
    [SerializeField] private Vector3 _offset;

    private float _countDestroyTime = 0;

    private bool _isDeathEeffect = false;

    private BossControl _bossControl;

    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
    }

    public void FirstSetting()
    {
        //アニメーション再生
        _bossControl.BossAnimControl.DeathPlay();

        _bossControl.Rigidbody.velocity = Vector3.zero;


    }

    public bool CountDestroyTime()
    {
        _countDestroyTime += Time.deltaTime;

        if (!_isDeathEeffect)
        {

            if (_countDestroyTime > 3f)
            {
                _isDeathEeffect = true;
                var go = GameObject.Instantiate(_deathEffect);
                go.transform.position = _bossControl.BossT.position + _offset;
            }
        }

        if (_countDestroyTime > _destroyTime)
        {
            return true;
        }
        return false;
    }

}
