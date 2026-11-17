
using NUnit.Framework.Constraints;

public class PlayerAttackState : PlayerStateBase
{   
     private readonly IPlayerAttackContext context;

     private AttackPhase phase = AttackPhase.None;
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
        
        PlayerAttackDefinition attack = context.Combat.CurrentAttack;

        if(attack == null)
        {
            StateMachine.SwitchState<PlayerNormalState>();
            return;
        }
        
        phase = AttackPhase.Startup;

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
