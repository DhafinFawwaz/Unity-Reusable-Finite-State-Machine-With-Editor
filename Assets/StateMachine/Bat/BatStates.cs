using System.Collections.Generic;
public class BatStates : States<BatCore, BatStates>
{
    Dictionary<State, BaseState<BatCore, BatStates>> _states = new Dictionary<State, BaseState<BatCore, BatStates>>();
    
    enum State
    {
        Idle, Jump, 
    }
    public BatStates(BatCore contextCore) : base (contextCore)
    {
        _states[State.Idle] = new BatIdleState(Core, this);
        _states[State.Jump] = new BatJumpState(Core, this);

    }

    public BaseState<BatCore, BatStates> Idle() => _states[State.Idle];
    public BaseState<BatCore, BatStates> Jump() => _states[State.Jump];

    
}
