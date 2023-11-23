using UnityEngine;

public class LAEAttackState : IStateMachine
{
    LongAttackEnemy _enemy;
    PlayerControl _player;
    float _timer;
    public LAEAttackState(LongAttackEnemy enemy, PlayerControl player)
    {
        _enemy = enemy;
        _player = player;
    }
    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Update()
    {
        _enemy.transform.forward = (_player.transform.position - _enemy.transform.position).normalized;
        _timer += Time.deltaTime;
        if(_timer > _enemy.AttackInterval)
        {
            _enemy.Attack(_enemy.transform.forward);
            _timer = 0;
            Debug.Log("UŒ‚");
        }
        float distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(distance > _enemy.SearchRange) 
        {
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        }
    }
}
