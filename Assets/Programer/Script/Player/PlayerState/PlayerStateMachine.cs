using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerStateMachine : StateMachine
{
    [SerializeField]
    private IdleState _stateIdle = default;
    [SerializeField]
    private WalkState _stateWalk = default;
    [SerializeField]
    private RunState _stateRun = default;
    [SerializeField]
    private JumpState _stateJump = default;
    [SerializeField]
    private UpAirState _stateUpAir = default;
    [SerializeField]
    private DownAirState _stateDownAir = default;

    [SerializeField]
    private AttackState _stateAttack = default;
    [SerializeField]
    private FinishAttackState _stateFinishAttack = default;
    [SerializeField]
    private AvoidState _stateAvoid = default;

    public IdleState StateIdle => _stateIdle;
    public WalkState StateWalk => _stateWalk;
    public RunState StateRun => _stateRun;
    public JumpState StateJump => _stateJump;
    public UpAirState StateUpAir => _stateUpAir;
    public DownAirState StateDownAir => _stateDownAir;

    public AttackState AttackState => _stateAttack;
    public FinishAttackState FinishAttackState => _stateFinishAttack;
    public AvoidState AvoidState => _stateAvoid;

    private PlayerControl _playerController;

    public PlayerControl PlayerController => _playerController;

    public void Init(PlayerControl playerController)
    {
        _playerController = playerController;
        Initialize(_stateIdle);
    }

    protected override void StateInit()
    {
        _stateIdle.Init(this);
        _stateWalk.Init(this);
        _stateRun.Init(this);
        _stateJump.Init(this);
        _stateUpAir.Init(this);
        _stateDownAir.Init(this);

        _stateAttack.Init(this);
        _stateFinishAttack.Init(this);
        _stateAvoid.Init(this);
    }

}