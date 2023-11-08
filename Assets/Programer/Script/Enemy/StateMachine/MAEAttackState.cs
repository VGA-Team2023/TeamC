using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MAEAttackState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    Vector3 _basePosition;
    float _playerSearchDistance;
    float _distance;
    float _speed;
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
        var timer = await Attack();
        _enemy.Rb.AddForce(dir * timer, ForceMode.Impulse);
        Debug.Log("MAEAttack:Enter");
    }

    public void Exit()
    {
        Debug.Log("MAEAttack:Exit");
    }

    public void Update()
    {
        float playerDis = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if (playerDis >= _playerSearchDistance)
        {
            float baseDis = Vector3.Distance(_enemy.transform.position, _basePosition);
            if (baseDis > _distance)
            {
                _enemy.transform.forward = (_basePosition - _enemy.transform.position).normalized;
                _enemy.Rb.velocity = _enemy.transform.forward * _speed;
            }
            else
            {
                Exit();
                _enemy.StateChange(EnemyBase.MoveState.FreeMove);
            }
        }
        else
        {
            Enter();
        }
    }

    async UniTask<float> Attack()
    {
        float random = Random.Range(5, 11);
        float timer = 0f;
        while (1 > timer)
        {
            timer += Time.deltaTime;
            await UniTask.Delay(1000);
        }
        return timer;
    }
}
