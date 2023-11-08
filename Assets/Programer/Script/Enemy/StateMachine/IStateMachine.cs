using UnityEngine;

public interface IStateMachine
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}
