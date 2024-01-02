using System.Collections.Generic;
public class PlayerStates : States<PlayerCore, PlayerStates>
{
    Dictionary<State, BaseState<PlayerCore, PlayerStates>> _states = new Dictionary<State, BaseState<PlayerCore, PlayerStates>>();
    
    enum State
    {
        Idle, Jump, 
    }
    public PlayerStates(PlayerCore contextCore) : base (contextCore)
    {
        _states[State.Idle] = new PlayerIdleState(Core, this);
        _states[State.Jump] = new PlayerJumpState(Core, this);

    }

    public BaseState<PlayerCore, PlayerStates> Idle() => _states[State.Idle];
    public BaseState<PlayerCore, PlayerStates> Jump() => _states[State.Jump];

    
}
