using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class PlayerDamage
{
    [Header("@ダメージを受ける時間(無敵時間)")]
    [SerializeField] private float _damageTime = 1f;

    [Header("Playerを無敵にするか_Debug用設定")]
    [SerializeField] private bool _isMuteki = false;

    [Header("蘇生のTimeLine")]
    [SerializeField] private PlayableDirector _reviveMovie;

    [Header("ムービーを流すまでの待機時間")]
    [SerializeField] private float _waitTime = 0;

    [Header("攻撃を受けた時のエフェクト")]
    [SerializeField] private List<ParticleSystem> _damageEffects = new List<ParticleSystem>();

    [Header("ボスの氷属性のダメージエフェクト")]
    [SerializeField] private List<ParticleSystem> _damageEffectsBossIce = new List<ParticleSystem>();

    [Header("ボスの草属性のダメージエフェクト")]
    [SerializeField] private List<ParticleSystem> _damageEffectsBossGrass = new List<ParticleSystem>();

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
        //アニメーション
        _playerControl.PlayerAnimControl.PlayDamage();
        //カメラ変更
        _playerControl.CameraControl.UseDefultCamera(true);
        //カメラの振動
        _playerControl.CameraControl.ShakeCamra(CameraType.AttackCharge, CameraShakeType.Damage);
    }

    public void CountDamageTime()
    {
        if (!_isDamage) return;

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

            //先生の声
            AudioController.Instance.Voice.Play(VoiceState.InstructorGameHeal);

            //蘇生のムービーを流す
            _reviveMovie.Play();
            GameManager.Instance.SpecialMovingPauseManager.PauseResume(true);
        }
    }

    /// <summary>ダメージを与える。</summary>
    /// <param name="damage">体力が0になったかどうか</param>
    public void Damage(float damage, bool isBossDamage, MagickType magickType)
    {
        //無敵時間中はダメージを受けない
        if (_isDamage || _isDead || _isMuteki || _playerControl.Avoid.isAvoid) return;

        if (_playerControl.PlayerHp.AddDamage(damage))
        {
            _isDead = true;

            //プレイヤーのダウン回数を保存
            GameManager.Instance.ScoreManager.PlayerDownNum++;

            //ダメージボイス
            AudioController.Instance.Voice.Play(VoiceState.PlayerDown);

            //アニメーション設定
            _playerControl.PlayerAnimControl.PlayDead();
            _playerControl.PlayerAnimControl.IsDead(true);

            //時間を遅くする
            _playerControl.HitStopConrol.StartHitStop(HitStopKind.FinishAttack);
        }
        else
        {
            _isDamage = true;

            //ダメージボイス
            AudioController.Instance.Voice.Play(VoiceState.PlayerDamage);
            //アニメーション設定
            _playerControl.PlayerAnimControl.IsDamage(true);
        }



        if (isBossDamage)
        {
            //属性別
            if (magickType == MagickType.Ice)
            {
                //音を鳴らす
                AudioController.Instance.SE.Play(SEState.PlayerBossEnemyHitIce);

                //エフェクトを再生
                _damageEffectsBossIce.ForEach(i => i.Play());
            }
            else
            {
                //音を鳴らす
                AudioController.Instance.SE.Play(SEState.PlayerBossEnemyHitGrass);

                //エフェクトを再生
                _damageEffectsBossGrass.ForEach(i => i.Play());
            }
        }
        else
        {
            //音を鳴らす
            AudioController.Instance.SE.Play(SEState.PlayerLongAttackEnemyDamage);
            //エフェクトを再生
            _damageEffects.ForEach(i => i.Play());
        }
    }


}
