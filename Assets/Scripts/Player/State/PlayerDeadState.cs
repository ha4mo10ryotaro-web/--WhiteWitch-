using UnityEngine;

public class PlayerDeadState : PlayerStateBase

{    private readonly IPlayerDeadContext context;
    public PlayerDeadState(
        PlayerStateMachine stateMachine,
        IPlayerDeadContext context) 
        : base(stateMachine)
    {
        this.context = context;
    }

    public override void Enter()
    {
        context.Movement.SetEnabled(false);
        context.Combat.CancelCurrentAction();
        context.Input.SetInputEnabled(false);
    }

    public override void Update()
    {

    }
    public override void FixedUpdate()
    {
    }
    public override void Exit()
    {
    }
}
