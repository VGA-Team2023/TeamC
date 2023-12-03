using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class PlayerDamage
{
    [Header("蘇生のTimeLine")]
    [SerializeField] private PlayableDirector _reviveMovie;

    [Header("ムービーを流すまでの待機時間")]
    [SerializeField] private float _waitTime = 0;

    [Header("ダメージを浮ける時間")]
    [SerializeField] private float _damageTime = 1f;

    private float _countDeadTime = 0;

    private float _countDamageTime = 0;

    private bool _isDead = false;

    private bool _isDamage = false;

    private PlayerControl _playerControl;

    public bool IsDamage => _isDamage;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void Damage()
    {
        //カメラ変更
        _playerControl.CameraControl.UseDefultCamera(true);
        //カメラの振動
        _playerControl.CameraControl.ShakeCamra(CameraType.AttackCharge, CameraShakeType.AttackNomal);
    }

    public void CountDamageTime()
    {
        _countDamageTime += Time.deltaTime;

        if (_countDamageTime > _damageTime)
        {
            _countDamageTime = 0;
            _isDamage = false;
            _playerControl.PlayerAnimControl.IsDamage(false);
        }
    }

    public void CountWaitTime()
    {
        if (!_isDead) return;

        _countDeadTime += Time.deltaTime;

        if (_waitTime < _countDeadTime)
        {
            _countDeadTime = 0;
            _isDead = false;

            //蘇生のムービーを流す
            _reviveMovie.Play();
        }
    }

    /// <summary>ダメージを与える。</summary>
    /// <param name="damage">体力が0になったかどうか</param>
    public void Damage(float damage)
    {
        if (_playerControl.PlayerHp.AddDamage(damage))
        {
            _isDead = true;
            _playerControl.PlayerAnimControl.PlayDead();
            _playerControl.PlayerAnimControl.IsDead(true);

            _playerControl.HitStopCall.HitStopCalld(_waitTime);
        }
        else
        {
            _isDamage = true;
            _playerControl.PlayerAnimControl.PlayDamage();
            _playerControl.PlayerAnimControl.IsDamage(true);
        }
    }


}
