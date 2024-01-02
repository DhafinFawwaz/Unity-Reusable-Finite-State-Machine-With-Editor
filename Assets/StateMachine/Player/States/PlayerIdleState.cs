using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerIdleState : BaseState<PlayerCore, PlayerStates>
{
    public PlayerIdleState(PlayerCore contextCore, PlayerStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        
    }

    public override void StateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            SwitchState(States.Jump());
    }
    public override void StateFixedUpdate()
    {

    }

    public override void StateExit()
    {
        
    }
    public override void OnHurt(HitRequest hitRequest, ref HitResult hitResult)
    {
        base.OnHurt(hitRequest, ref hitResult);
    }
}
