using UnityEngine;

public class MAEAttackState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    Vector3 _basePosition;
    Vector3 _dir;
    float _playerSearchDistance;
    float _distance;
    float _speed;
    float _timer;

    public MAEAttackState(MeleeAttackEnemy enemy, PlayerControl player, float playerSearchDistance, float distance, float speed)
    {
        _enemy = enemy;
        _player = player;
        _playerSearchDistance = playerSearchDistance;
        _distance = distance;
        _speed = speed;

        _basePosition = _enemy.transform.position;
    }
    public async void Enter()
    {
        _enemy.Rb.velocity = Vector3.zero;
        var dir = (_player.transform.position - _enemy.transform.position).normalized;
        _enemy.transform.forward = dir;
        _enemy.Rb.AddForce(_enemy.transform.forward * 5, ForceMode.Impulse);
        _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        Debug.Log("MAEAttack:Enter");
    }

    public void Exit()
    {
        Debug.Log("MAEAttack:Exit");
    }

    public void Update()
    {

    }
}
