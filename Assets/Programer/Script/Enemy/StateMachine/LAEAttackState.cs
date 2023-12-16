using UnityEngine;
using CriWare;

//遠距離攻撃敵の弾を出す攻撃
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
        int random = Random.Range(0, 2);
        switch(random)
        {
            case 0:
                _enemy.VoiceAudio(VoiceState.EnemyDiscovPattern1, EnemyBase.CRIType.Play);
                break;
            case 1:
                _enemy.VoiceAudio(VoiceState.EnemyDiscovPattern2, EnemyBase.CRIType.Play);
                break;
        }
    }

    public void Exit()
    {

    }

    public void Update()
    {
        //一定時間経過したら弾を生成する
        _timer += Time.deltaTime;
        if(_timer > _enemy.AttackInterval)
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
            _enemy.SeAudio(SEState.EnemyLongAttackShoot, LongAttackEnemy.CRIType.Play);
            //_enemy.Animator.Play("WitchHag_Attack_Swipe01");
            _enemy.Attack();
            _timer = 0;
            Debug.Log("攻撃");
        }
        if (_enemy.IsDemo) return;
        _enemy.transform.forward = (_player.transform.position - _enemy.transform.position).normalized;
        //サーチ範囲から離れたら通常行動に戻る
        float distance = Vector3.Distance(_player.transform.position, _enemy.transform.position);
        if(distance > _enemy.SearchRange) 
        {
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.FreeMove);
        }

        CriAtomExPlayer criAtomExPlayer = new CriAtomExPlayer();

        criAtomExPlayer.Stop(false);
    }
}
