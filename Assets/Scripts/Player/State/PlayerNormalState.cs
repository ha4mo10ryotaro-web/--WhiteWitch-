using UnityEngine;

public class PlayerNormalState : PlayerStateBase

{    private readonly IPlayerNormalContext context;
    public PlayerNormalState(
        PlayerStateMachine stateMachine,
        IPlayerNormalContext context) 
        : base(stateMachine)
    {
        this.context = context;
    }

    public override void Enter()
    {

        return;
    }

    public override void Update()
    {
        context.Movement.SetMoveInput(context.Input.MoveInput);

        if(context.Input.ConsumeAttackPressed())
        {
            StateMachine.SwitchState<PlayerAttackState>();
        }
        return;

    }
    public override void FixedUpdate()
    {

        return;
    }
    public override void Exit()
    {
        context.Movement.Stop();
        return;
    }
}
