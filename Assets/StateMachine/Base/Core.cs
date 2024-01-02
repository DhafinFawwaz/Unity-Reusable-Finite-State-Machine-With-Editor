using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public abstract string GetCurrentState();
    public abstract string GetPreviousState();
    public abstract void OnHurt(HitRequest hitRequest, ref HitResult hitResult);
}

public abstract class Core<T, U> : Core where T : Core<T, U> where U : States<T, U>
{
    #region StateMachine
    U _states;
    BaseState<T, U> _currentState;
    BaseState<T,U> _previousState;


    public U States {get {return _states;} set {_states = value;}}
    public BaseState<T,U> CurrentState {get {return _currentState;} set {_currentState = value;}}
    public BaseState<T,U> PreviousState {get {return _previousState;} set {_previousState = value;}}
    public void SwitchState(BaseState<T,U> newState)
    {
        _previousState = CurrentState;
        _currentState.StateExit();
        _currentState = newState;
        _currentState.StateEnter();
    }
    public override string GetCurrentState()
    {
        return _currentState.ToString();
    }
    public override string GetPreviousState()
    {
        return _previousState.ToString();
    }

    public override void OnHurt(HitRequest hitRequest, ref HitResult hitResult)
    {
        _currentState.OnHurt(hitRequest, ref hitResult);
    }
#endregion StateMachine
}
