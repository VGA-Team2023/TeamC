using Unity.VisualScripting;
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
        if (distance < 1f && !_isHit)
        {
            //_player.Damage(_enemy.Attack);
            _enemy.Rb.velocity = Vector3.zero;
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    Debug.Log("ƒ^ƒbƒNƒ‹UŒ‚");
                    _enemy.Rb.AddForce(-_dir * 3f + Vector3.up * 3f, ForceMode.Impulse);
                    break;
                case 1:
                    Ray ray = new Ray(_enemy.transform.position, _enemy.transform.forward * 1f);
                    if(Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (_enemy.TryGet(out PlayerControl getObject, hit.collider.gameObject))
                        {
                            Debug.Log("‚Ð‚Á‚©‚«UŒ‚");
                            //getObject.Damage(10);
                        }
                    }
                    break;
            }
            _isHit = true;
        }
        if (_isHit)
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
