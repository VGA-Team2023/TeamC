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
    int _index = 0;

    /// <summary>Playerが攻撃判定内にいる時間を計算 </summary>
    private float _playerNearTime = 0;

    public LAEFreeMoveState(LongAttackEnemy enemy, PlayerControl player, List<Vector3> patrolPoint)
    {
        _enemy = enemy;
        _player = player;
        _patrolPoint = patrolPoint;
        _index = 0;
    }
    public void Enter()
    {
        _enemy.VoiceAudio(VoiceState.EnemyLongSaerch, EnemyBase.CRIType.Play);
        _index = 0;
        _playerNearTime = 0;
    }

    public void Exit()
    {
        Debug.Log("LAEFreeMove:Exit");
    }

    public void Update()
    {
        _enemy.VoiceAudio(VoiceState.EnemyLongSaerch, EnemyBase.CRIType.Update);
        if (_enemy.IsDemo) return;
        //敵がサーチ範囲に入ったら攻撃を始める(遠距離攻撃)
        float playerDistance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if (playerDistance < _enemy.SearchRange)
        {
            _playerNearTime += Time.deltaTime;

            if (_playerNearTime > 0.3f)
            {
                Exit();
                _enemy.StateChange(EnemyBase.MoveState.Attack);
            }
        }
        else
        {
            _playerNearTime = 0;
        }
        //基本は決められた地点を周回する
        float distance = Vector3.Distance(_enemy.transform.position, _patrolPoint[_index % _patrolPoint.Count]);
        Debug.Log($"{_enemy.name}Index:{_index}");
        Debug.Log($"{_enemy.name}NextPoint:{_patrolPoint[_index % _patrolPoint.Count]}");
        if (distance < _enemy.ChangeDistance)
        {
            _index++;
            Debug.Log($"{_enemy.name}Index:{_index}");
            Debug.Log($"{_enemy.name}NextPoint:{_patrolPoint[_index % _patrolPoint.Count]}");
        }
        var nextPoint = (_patrolPoint[_index % _patrolPoint.Count] - _enemy.transform.position).normalized;
        _enemy.transform.forward = new Vector3(nextPoint.x, 0, nextPoint.z);
        _enemy.Rb.velocity = _enemy.transform.forward * _enemy.Speed;
    }

    public void WallHit()
    {
    }
}
