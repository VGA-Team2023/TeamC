using UnityEngine;

/// <summary>
/// プレイヤーを発見したらプレイヤーのに向かって移動するステート
/// </summary>
public class MAEChaseState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;

    public MAEChaseState(MeleeAttackEnemy enemy, PlayerControl player)
    {
        _enemy = enemy;
        _player = player;
    }
    public void Enter()
    {
        ///throw new System.NotImplementedException();
    }

    public void Exit()
    {
       // throw new System.NotImplementedException();
    }

    public void Update()
    {
        if (_enemy.IsDemo) return;
        //プレイヤーとの距離を算出
        float distance = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if(distance < _enemy.ChaseDistance)
        {
            //アタック可能な距離まで近づいたらアタックステートに移行
            _enemy.StateChange(EnemyBase.MoveState.Attack);
        }
        if(distance > _enemy.SearchRange)
        {
            //一定距離離れるとフリームーブステートに移行
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        }
        //プレイヤーがいる方向を算出してプレイヤーに近づく
        var dir = (_player.transform.position - _enemy.transform.position).normalized;
        _enemy.transform.forward = new Vector3(dir.x, 0, dir.z);
        _enemy.Rb.velocity = (_enemy.transform.forward * _enemy.Speed) + new Vector3(0, dir.y , 0);
    }

    public void WallHit()
    {
        throw new System.NotImplementedException();
    }
}
