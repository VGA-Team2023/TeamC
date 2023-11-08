using UnityEngine;

public class LAEAttackState : IStateMachine
{
    LongAttackEnemy _enemy;
    PlayerControl _player;
    float _playerDis;
    float _interval;
    float _timer;
    public LAEAttackState(LongAttackEnemy enemy, PlayerControl player, float playerDis, float interval)
    {
        _enemy = enemy;
        _player = player;
        _playerDis = playerDis;
    }
    public void Enter()
    {
        Debug.Log("LAEAttack:Enter");
    }

    public void Exit()
    {
        Debug.Log("LAEAttack:Exit");
    }

    public void Update()
    {
        _enemy.transform.forward = (_player.transform.position - _enemy.transform.position).normalized;
        _timer += Time.deltaTime;
        if(_timer > _interval)
        {
            _enemy.Attack(_enemy.transform.forward);
            _timer = 0;
            Debug.Log("�U��");
        }
        float distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(distance > _playerDis) 
        {
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        }
    }
}