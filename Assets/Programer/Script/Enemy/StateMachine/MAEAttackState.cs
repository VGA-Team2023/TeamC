using UnityEngine;

public class MAEAttackState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    Vector3 _dir;
    bool _isHit = false;
    float _timer;

    public MAEAttackState(MeleeAttackEnemy enemy, PlayerControl player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Enter()
    {
        _isHit = false;
        _dir = (_player.transform.position - _enemy.transform.position).normalized;
        _enemy.transform.forward = new Vector3(_dir.x, 0, _dir.z);
        _enemy.Rb.AddForce(_enemy.transform.forward * _enemy.Speed * 10, ForceMode.Impulse);

    }

    public void Exit()
    {

    }

    public void Update()
    {
        float distance = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if(distance < 1f && !_isHit)
        {
            //_player.Damage(_enemy.Attack);
            _enemy.Rb.velocity = Vector3.zero;
            _enemy.Rb.AddForce(- _dir * 10f + Vector3.up * 10f, ForceMode.Impulse);
            _isHit = true;
        }
        if(_isHit)
        {
            _timer += Time.deltaTime;
            if (_timer > 3f)
            {
                _timer = 0;
                if (distance < _enemy.ChaseDistance)
                {
                    _enemy.StateChange(EnemyBase.MoveState.Chase);
                }
                else
                {
                    _enemy.StateChange(EnemyBase.MoveState.FreeMove);
                }
            }
        }
    }
}
