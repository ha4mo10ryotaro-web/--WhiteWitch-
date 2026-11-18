using UnityEngine;
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
    private PlayerAttackDefinition attack;
    private AttackPhase currentPhase = AttackPhase.None;
    private float phaseTimer;

    public override void Enter()
    {
        context.Movement.Stop();
        attack = context.Combat.CurrentAttack;

        if(attack == null)
        {
            Debug.LogWarning(
                "Player:使用可能な攻撃がありません");

            StateMachine.SwitchState<PlayerNormalState>();
            return;
        }
        
        EnterStartupPhase();

    }

    public override void Update()
    {
        phaseTimer -= Time.deltaTime;

        if(phaseTimer > 0f)
        {
            return;
        }

         switch(currentPhase)
        {
            case AttackPhase.Startup:
                EnterActivePhase();
                break;
            case AttackPhase.Active:
                EnterRecoveryPhase();
                break;            
            case AttackPhase.Recovery:
                CompleteAttack();
                break;
        }

    }
    public override void FixedUpdate()
    {

        return;

    }
   public override void Exit()
    {
        context.Combat.CancelCurrentAction();
        context.Combat.SetAttackPhase(AttackPhase.None);

        phaseTimer = 0f;
        attack = null;
    }

    private void EnterStartupPhase()
    {
        currentPhase = AttackPhase.Startup;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = attack.StartupTime;
        Debug.Log($"Player:{attack}攻撃開始");
    }
    private void EnterActivePhase()
    {
        currentPhase = AttackPhase.Active;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = attack.ActiveTime;

        context.Combat.PrepareAttack(attack);
    }
    private void EnterRecoveryPhase()
    {
        context.Combat.EndAttack();

        currentPhase = AttackPhase.Recovery;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = attack.RecoveryTime;
    }
    private void CompleteAttack()
    {
        Debug.Log("Player:攻撃終了。追跡状態に戻ります");
        StateMachine.SwitchState<PlayerNormalState>();
    
    }
}
