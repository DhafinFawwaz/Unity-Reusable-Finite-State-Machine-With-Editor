using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BatIdleState : BaseState<BatCore, BatStates>
{
    public BatIdleState(BatCore contextCore, BatStates States) : base (contextCore, States)
    {
    }

    public override void StateEnter()
    {
        Core.OnEnterIdle();
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
