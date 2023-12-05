using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowAttackBullet : MonoBehaviour
{
    [Header("攻撃の柱のエフェクト")]
    [SerializeField] private List<ParticleSystem> _attackP = new List<ParticleSystem>();

    [Header("魔法陣ののエフェクト")]
    [SerializeField] private List<ParticleSystem> _magicCircles = new List<ParticleSystem>();

    [Header("攻撃力")]
    [SerializeField] private float _attackPower = 1;

    [Header("攻撃のエフェクト発生までのクールタイム")]
    [SerializeField] private float _waitTime = 1f;

    [Header("エフェクト再生から攻撃発生までの時間")]
    [SerializeField] private float _attackWaitTime = 0.3f;


    [Header("削除までの時間")]
    [SerializeField] private float _destroyTime = 5f;

    [Header("攻撃範囲_offset")]
    [SerializeField] private Vector3 _offset;

    [Header("攻撃範囲_size")]
    [SerializeField] private Vector3 _size;

    [Header("プレイヤーのレイヤー")]
    [SerializeField] private LayerMask _layer;

    [Header("Gizmoを表示するかどうか")]
    [SerializeField] private bool _isDrowGizmo = false;

    private float _countWaitTime = 0;

    private float _countAttackWaitTime = 0;

    private float _countDestroyTime = 0;

    private bool _isPlayEffect = false;

    private bool _isAttack = false;


    void Start()
    {

    }

    void Update()
    {
        if (!_isPlayEffect)
        {
            _countWaitTime += Time.deltaTime;

            if (_countWaitTime > _waitTime)
            {
                foreach (var e in _attackP)
                {
                    e.Play();
                }
                _isPlayEffect = true;
            }   //時間経過で攻撃のエフェクトを再生
        }
        else if (_isPlayEffect && !_isAttack)
        {
            _countAttackWaitTime += Time.deltaTime;

            if (_countAttackWaitTime > _attackWaitTime)
            {
                CheckAttack();
                _isAttack = true;
            }
        }
        else
        {
            _countDestroyTime += Time.deltaTime;

            if (_countDestroyTime > _destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void CheckAttack()
    {
        var g = Physics.OverlapBox(transform.position + _offset, _size, Quaternion.identity, _layer);

        foreach (var e in g)
        {
            e.gameObject.TryGetComponent<IPlayerDamageble>(out IPlayerDamageble player);
            player?.Damage(_attackPower);
            return;
        }
    }


    private void OnDrawGizmos()
    {
        if (!_isDrowGizmo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + _offset, _size / 2);
    }

}
