using UnityEditor.Rendering;
using UnityEngine;

//アタック可能な距離まで近づいたらアタックする
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
        Debug.Log(_player);
    }

    public void Enter()
    {
        if (_enemy.IsDemo) return;
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                _enemy.VoiceAudio(VoiceState.EnemyDiscovPattern1, EnemyBase.CRIType.Play);
                break;
            case 1:
                _enemy.VoiceAudio(VoiceState.EnemyDiscovPattern2, EnemyBase.CRIType.Play);
                break;
        }
        _isHit = false;
        //加速してプレイヤーに近づく
        _dir = (_player.transform.position - _enemy.transform.position).normalized;
        _enemy.transform.forward = new Vector3(_dir.x, 0, _dir.z);
        _enemy.Rb.AddForce(_enemy.transform.forward * _enemy.Speed * 10, ForceMode.Impulse);

    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (_enemy.IsDemo)
        {
            _timer += Time.deltaTime;
            if (_timer > 1f)
            {
                int random = Random.Range(0, 2);
                switch (random)
                {
                    case 0:
                        _enemy.VoiceAudio(VoiceState.EnemyAttackPattern1, EnemyBase.CRIType.Play);
                        break;
                    case 1:
                        _enemy.VoiceAudio(VoiceState.EnemyAttackPattern2, EnemyBase.CRIType.Play);
                        break;
                }
                _enemy.SeAudio(SEState.EnemyCloseAttack, MeleeAttackEnemy.CRIType.Play);
                _enemy.Animator.Play("Attack");
                _timer = 0f;
            }
        }
        else
        {
            //プレイヤーに近づいたらランダムで攻撃を出す
            float distance = Vector3.Distance(_enemy.transform.position, _player.transform.position);
            if (distance < 2f && !_isHit)
            {
                PlayerControl player = null;
                _enemy.Rb.velocity = Vector3.zero;
                int random = Random.Range(0, 2);
                Ray ray = new Ray(_enemy.transform.position, _enemy.transform.forward * 5f);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (_enemy.TryGet(out PlayerControl getObject, hit.collider.gameObject))
                    {
                        player = getObject;
                    }
                }
                _enemy.SeAudio(SEState.EnemyCloseAttack, MeleeAttackEnemy.CRIType.Play);
                _enemy.Animator.Play("Attack");
                //rayを飛ばして目の前に敵がいたらひっかき攻撃を出す
                if (player)
                {
                    player.Rb.AddForce(_dir * 2f + Vector3.up * 3f, ForceMode.Impulse);
                    player.Damage(_enemy.Attack);
                }
                //switch (random)
                //{
                //    case 0:
                //        //タックル攻撃の後後ろにのけぞる
                //        _enemy.Audio(SEState.EnemyCloseAttack);
                //        _enemy.Rb.AddForce(-_dir * 3f + Vector3.up * 3f, ForceMode.Impulse);
                //        player.Rb.AddForce(_dir * 3f + Vector3.up * 3f, ForceMode.Impulse);
                //        player.Damage(_enemy.Attack);
                //        break;
                //    case 1:
                //        _enemy.Audio(SEState.EnemyCloseAttack);
                //        _enemy.Animator.Play("Attack");
                //        //rayを飛ばして目の前に敵がいたらひっかき攻撃を出す
                //        player.Rb.AddForce(_dir * 2f + Vector3.up * 3f, ForceMode.Impulse);
                //        player.Damage(_enemy.Attack);
                //        break;
                //}
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
}
