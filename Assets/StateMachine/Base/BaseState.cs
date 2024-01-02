public abstract class BaseState<T, U> where T : Core<T, U> where U : States<T, U>
{
    T _core;
    U _states;
    protected T Core{get{return _core;}}
    protected U States{get{return _states;}}
    public BaseState(T contextCore, U playerStates)
    {
        _core = contextCore;
        _states = playerStates;
    }
    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateFixedUpdate();
    public abstract void StateExit();
    public virtual void OnHurt(HitRequest hitParams, ref HitResult hitResult)
    {
        
    }
    protected void SwitchState(BaseState<T, U> newState)
    {
        _core.SwitchState(newState);
    }
}