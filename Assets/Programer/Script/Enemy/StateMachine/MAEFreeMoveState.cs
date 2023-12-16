using UnityEngine;

public class MAEFreeMoveState : IStateMachine
{
    MeleeAttackEnemy _enemy;
    PlayerControl _player;
    Vector3 _basePosition;
    Vector3 _dir;
    bool _isArrived = true;

    public MAEFreeMoveState(MeleeAttackEnemy enemy, PlayerControl player)
    {
        _enemy = enemy;
        _player = player;
        _basePosition = _enemy.transform.position;
        if (_enemy.IsDemo) return;
        _dir = GetMovePoint();
        _enemy.transform.forward = (_dir - _enemy.transform.position).normalized;
    }
    public void Enter()
    {
        _enemy.VoiceAudio(VoiceState.EnemySaerch, EnemyBase.CRIType.Play);
    }

    public void Exit()
    {
    }

    public void Update()
    {
        _enemy.VoiceAudio(VoiceState.EnemySaerch, EnemyBase.CRIType.Update);
        if (_enemy.IsDemo) return;
        //プレイヤーとの距離を算出
        float playerDis = Vector3.Distance(_enemy.transform.position, _player.transform.position);
        if (playerDis < _enemy.SearchRange)
        {
            //攻撃可能な範囲から離れたらChaseステートに戻る
            Exit();
            _enemy.StateChange(EnemyBase.MoveState.Chase);
        }
        //初期位置との距離を算出
        float baseDis = Vector3.Distance(_enemy.transform.position, _basePosition);
        //目的地との距離を算出
        float destinationDis = Vector3.Distance(_enemy.transform.position, _dir);
        if (baseDis < _enemy.Distance && !_isArrived)
        {
            //次の目的地を取得
            _dir = GetMovePoint();
            _enemy.transform.forward = (_dir - _enemy.transform.position).normalized;
            _isArrived = true;
        }
        else if (destinationDis < _enemy.Distance && _isArrived)
        {
            //目的地に着いたら初期位置に戻る
            _dir = _basePosition - _enemy.transform.position;
            _enemy.transform.forward = _dir.normalized;
            _isArrived = false;
        }
        _enemy.Rb.velocity = _enemy.transform.forward * _enemy.Speed;
        _enemy.transform.position = new Vector3(_enemy.transform.position.x, _basePosition.y, _enemy.transform.position.z);
    }

    //次の目的地を計算してreturnする
    Vector3 GetMovePoint()
    {
        float random = Random.Range(0, 361);
        var dir = new Vector3(Mathf.Sin(random) * _enemy.MoveRange + _enemy.transform.position.x, _basePosition.y, Mathf.Cos(random) * _enemy.MoveRange + _enemy.transform.position.z);
        return dir;
    }

    //壁に当たったら初期位置に戻る
    public void WallHit()
    {
        _dir = _basePosition - _enemy.transform.position;
        _enemy.transform.forward = _dir.normalized;
        _isArrived = false;
    }
}
