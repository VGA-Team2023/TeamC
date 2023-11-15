using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MAEChaseState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    float _timer;
    float _random;

    public MAEChaseState(MeleeAttackEnemy enemy, PlayerControl player)
    {
        _enemy = enemy;
        _player = player;
    }
    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        float distance = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if(distance < _enemy.ChaseDistance)
        {
            _enemy.StateChange(EnemyBase.MoveState.Attack);
        }
        if(distance > _enemy.SearchRange)
        {
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        }
        var dir = (_player.transform.position - _enemy.transform.position).normalized;
        _enemy.transform.forward = new Vector3(dir.x, 0, dir.z);
        if (_timer > 1f)
        {
            _random = Random.Range(-2f, -3f);
            _timer = 0;
        }
        //Debug.Log(Random.Range(0f, _enemy.Random));
        _enemy.Rb.velocity = (_enemy.transform.forward * _enemy.Speed) + new Vector3(0, dir.y , 0);
        //Vector3.Slerp(_enemy.transform.position, new Vector3(_enemy.transform.position.x, dir.y + _random, _enemy.transform.position.z), 2);

    }
}
