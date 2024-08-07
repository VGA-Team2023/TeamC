using UnityEngine;
using static EnemyBase;

public class TestStateChange : MonoBehaviour
{
    [SerializeField]
    bool _testOn;
    [SerializeField]
    MeleeAttackEnemy _melee;
    [SerializeField]
    LongAttackEnemy _long;
    [SerializeField]
    MoveState _changeState;

    public void StateChange()
    {
        if (!_testOn) return;
        if(_melee)
        {
            _melee.StateChange(_changeState);
        }
        else if(_long)
        {
            _long.StateChange(_changeState);
        }
    }
}
