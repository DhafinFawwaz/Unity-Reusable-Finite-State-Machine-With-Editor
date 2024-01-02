using UnityEngine;

public class PlayerCore : Core<PlayerCore, PlayerStates>
{
    public PlayerLocomotion Locomotion;
    public PlayerStats Stats;

    void Start()
    {
        States = new PlayerStates(this);
        CurrentState = States.Idle();
        CurrentState.StateEnter();

        Locomotion = GetComponent<PlayerLocomotion>();
        Stats = GetComponent<PlayerStats>();
    }

    public override void OnHurt(HitRequest hitRequest, ref HitResult hitResult)
    {

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
            if(hitResult.Type == HitType.Entity)
            {
                Debug.Log("Hit Entity");
            }
        }
    }
    void FixedUpdate()
    {
        CurrentState.StateFixedUpdate();
    }

    
}
