using UnityEngine;

public class EnemyBase :  MonoBehaviour
{
    [SerializeField, Tooltip("エネミーの体力")]
    int _hp;
    public int HP => _hp;
    [SerializeField, Tooltip("エネミーの攻撃力")]
    int _attack;
    public int Attack => _attack;
    [SerializeField, Tooltip("エネミーの移動速度")]
    float _speed;
    public float Speed => _speed;
    [SerializeField, Tooltip("プレイヤーを検出する範囲(赤い円)"), Range(0, 10)]
    float _searchRange;
    public float SearchRange => _searchRange;

    public enum MoveState
    {
        FreeMove,
        TargetMove,
        Attack,
    }
}
