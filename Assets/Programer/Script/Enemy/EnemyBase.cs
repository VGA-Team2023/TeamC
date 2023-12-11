using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("テスト用")]
    [SerializeField, Tooltip("仮でAudioを再生する")]
    bool _isTestAudio;
    public bool IsTestAudio => _isTestAudio;

    [Header("敵のステータスに関する数値")]
    [SerializeField, Tooltip("エネミーの体力")]
    int _hp;
    public int HP
    {
        get => _hp;
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                OnEnemyDestroy();
            }
        }
    }
    [SerializeField, Tooltip("エネミーの攻撃力")]
    int _attack;
    public int Attack => _attack;
    [Header("====================")]

    [Header("敵の挙動に関する数値")]
    [SerializeField, Tooltip("エネミーの移動速度")]
    float _speed;
    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }
    [SerializeField, Tooltip("攻撃の間隔")]
    float _attackInterval;
    public float AttackInterval => _attackInterval;
    [SerializeField, Tooltip("プレイヤーを検出する範囲(赤い円)"), Range(0, 30)]
    float _searchRange;
    public float SearchRange => _searchRange;
    [SerializeField]
    float _finishStopInterval;
    public float FinishStopInterval => _finishStopInterval;
    [Header("====================")]

    [Header("とどめの攻撃を出来るか判定するレイヤー")]
    [SerializeField, Tooltip("通常のレイヤー")]
    int _defaultLayer;
    public int DefaultLayer => _defaultLayer;
    [SerializeField, Tooltip("とどめが可能なレイヤー")]
    int _finishLayer;
    public int FinishLayer => _finishLayer;
    [SerializeField, Tooltip("倒された敵のレイヤー")]
    int _deadLayer;
    public int DeadLayer => _deadLayer;
    [Header("====================")]

    [Header("生成するオブジェクト")]
    [SerializeField, Tooltip("コアのオブジェクト")]
    GameObject _core;
    public GameObject Core => _core;

    //enemyのHPが0になった時に呼ばれる
    public event Action OnEnemyDestroy;
    //enemyが破壊された時に呼ばれる関数
    public event Action OnEnemyFinish;

    public void EnemyFinish()
    {
        OnEnemyFinish?.Invoke();
    }

    public enum MoveState
    {
        FreeMove,
        TargetMove,
        Attack,
        Finish,
        Chase,
    }
}
