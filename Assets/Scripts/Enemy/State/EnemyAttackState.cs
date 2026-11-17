using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    private readonly IEnemyAttackContext context;
    public EnemyAttackState(
        EnemyController enemy,
        IEnemyAttackContext context) 
        : base(enemy)
    {
        this.context = context;
    }
    private EnemyAttackDefinition attack; 
    private AttackPhase currentPhase;
    private float phaseTimer;


    public override void Enter()
    {
        context.Movement.Stop();
        attack = context.Combat.DefaultAttack;

        if(attack == null)
        {
            Debug.LogWarning(
                $"{enemy.name}:使用可能な攻撃がありません");

            enemy.StateMachine.SwitchState<EnemyChaseState>();
            return;
        }

        EnterStartupPhase();
        Debug.Log($"{enemy.name}:戦闘状態に入りました");
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

    public override void Exit()
    {
        context.Combat.CancelAttack();
        context.Combat.SetAttackPhase(AttackPhase.None);

        phaseTimer = 0f;
        attack = null;
    }

    private void EnterStartupPhase()
    {
        currentPhase = AttackPhase.Startup;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = attack.StartupTime;
        Debug.Log($"{enemy.name}:{attack}攻撃開始");
    }
    private void EnterActivePhase()
    {
        currentPhase = AttackPhase.Active;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = attack.ActiveTime;

        context.Combat.BeginAttack(attack);
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
        Debug.Log($"{enemy.name}:攻撃終了。追跡状態に戻ります");
        enemy.StateMachine.SwitchState<EnemyChaseState>();
    
    }

}
