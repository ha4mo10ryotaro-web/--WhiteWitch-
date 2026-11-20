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
        Debug.Log("Player:Normal状態開始");
        return;
    }

    public override void Update()
    {
        context.Movement.SetMoveInput(context.Input.MoveInput);

        if(context.Input.ConsumeAttackPressed())
        {
            Debug.Log("NormalStateが攻撃受理を受け取りました");

            if(context.Combat.PrepareDefaultAttack())
            {
                StateMachine.SwitchState<PlayerAttackState>();

            }
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
