using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離攻撃の通常行動ステート
/// </summary>
public class LAEFreeMoveState : IStateMachine
{
    LongAttackEnemy _enemy;
    PlayerControl _player;
    List<Vector3> _patrolPoint = new List<Vector3>();
    int _index;

    public LAEFreeMoveState(LongAttackEnemy enemy, PlayerControl player, List<Vector3> patrolPoint)
    { 
        _enemy = enemy;
        _player = player;
        _patrolPoint = patrolPoint;
    }
    public void Enter()
    {
        Debug.Log("LAEFreeMove:Enter");
    }

    public void Exit()
    {
        Debug.Log("LAEFreeMove:Exit");
    }

    public void Update()
    {
        if (_enemy.IsDemo) return;
        //敵がサーチ範囲に入ったら攻撃を始める(遠距離攻撃)
        float playerDistance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(playerDistance < _enemy.SearchRange)
        {
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.Attack);
        }
        //基本は決められた地点を周回する
        float distance = Vector3.Distance(_enemy.transform.position, _patrolPoint[_index % _patrolPoint.Count]);
        if (distance < _enemy.ChangeDistance)
        {
            _index++;
        }
        var nextPoint = (_patrolPoint[_index % _patrolPoint.Count] - _enemy.transform.position).normalized;
        _enemy.transform.forward = new Vector3(nextPoint.x, 0, nextPoint.z);
        _enemy.Rb.velocity = _enemy.transform.forward * _enemy.Speed;
    }
}
