using UnityEngine;

public class BatCore : Core<BatCore, BatStates>
{
    void Awake()
    {
        States = new BatStates(this);
        CurrentState = States.Idle();
        CurrentState.StateEnter();
    }

    public override void OnHurt(HitRequest hitRequest, ref HitResult hitResult)
    {
        hitResult.Type = HitType.Entity;
    }


    void Update()
    {
        CurrentState.StateUpdate();
        if(Input.GetKeyDown(KeyCode.H))
        {
            HitResult hitResult = new HitResult();
            GetComponent<Core>().OnHurt(new HitRequest(
                damage: 10, 
                knockback: 100, 
                direction: Vector2.up, 
                element: Element.Fire, 
                stunDuration: 0.5f
            ), ref hitResult);
        }
    }
    void FixedUpdate()
    {
        CurrentState.StateFixedUpdate();
    }

    public void OnEnterIdle()
    {
        Debug.Log("Enter Idle");
    }

    
}
