
public class PlayerAttackState : PlayerStateBase
{   
     private readonly IPlayerAttackContext context;
    public PlayerAttackState(
        PlayerStateMachine stateMachine,
        IPlayerAttackContext context) 
        : base(stateMachine)
    {
        this.context = context;
    }

    public override void Enter()
    {
        context.Movement.Stop();
        context.Combat.BeginAttack();
        return;
    }

    public override void Update()
    {
        context.Combat.TickAttack();
        
        if(context.Combat.IsAttackFinished)
        {
            StateMachine.SwitchState<PlayerNormalState>();
        }

        return;

    }
    public override void FixedUpdate()
    {

        return;

    }
    public override void Exit()
    {
        context.Combat.EndAttack();
        return;
    }
}
