using System;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("エネミーの体力")]
    int _hp;
    public int HP 
    {
        get => _hp; 
        set
        {
            _hp = value;
            if(_hp <= 0)
            {
                OnEnemyDestroy();
            }
        }
    }
    [SerializeField, Tooltip("エネミーの攻撃力")]
    int _attack;
    public int Attack => _attack;
    [SerializeField, Tooltip("エネミーの移動速度")]
    float _speed;
    public float Speed => _speed;
    [SerializeField, Tooltip("攻撃の間隔")]
    float _attackInterval;
    public float AttackInterval => _attackInterval;
    [SerializeField, Tooltip("プレイヤーを検出する範囲(赤い円)"), Range(0, 10)]
    float _searchRange;
    public float SearchRange => _searchRange;
    [SerializeField, Tooltip("通常のレイヤー")]
    LayerMask _defaultLayer;
    public LayerMask DefaultLayer => _defaultLayer;
    [SerializeField, Tooltip("とどめが可能なレイヤー")]
    LayerMask _finishLayer;
    public LayerMask FinishLayer => _finishLayer;
    [SerializeField, Tooltip("コアのオブジェクト")]
    GameObject _core;
    public GameObject Core => _core;

    //enemyが破壊された時の
    public event Action OnEnemyDestroy;

    public enum MoveState
    {
        FreeMove,
        TargetMove,
        Attack,
        Finish,
        Chase,
    }
}
