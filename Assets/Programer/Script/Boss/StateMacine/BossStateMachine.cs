using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossStateMachine : StateMachine
{
    [SerializeField] private BossIdleState _stateIdle = default;

    [SerializeField] private BossFinishState _stateFinish = default;




    private BossControl _bossController;
    public BossControl BossController => _bossController;

    public BossFinishState StateFinish => _stateFinish;

    public BossIdleState StateIdle => _stateIdle;


    public void Init(BossControl playerController)
    {
        _bossController = playerController;
        Initialize(_stateIdle);
    }

    protected override void StateInit()
    {
        _stateIdle.Init(this);
        _stateFinish.Init(this);
    }
}