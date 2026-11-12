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

    //仮調整用パラメータ。後で攻撃データ等へ情報を移す。前隙、持続、後隙の時間。
            private readonly float startupTime = 0.8f;
            private readonly float activeTime = 0.3f;
            private readonly float recoveryTime = 1.0f;  
    private enum AttackPhase
    {
        Startup,
        Active,
        Recovery
    }
    private AttackPhase currentPhase;
    private float phaseTimer;


    public override void Enter()
    {
        context.Movement.Stop();
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
        phaseTimer = 0f;
    }

    private void EnterStartupPhase()
    {
        currentPhase = AttackPhase.Startup;
        phaseTimer = startupTime;

        Debug.Log($"{enemy.name}:前隙開始。{startupTime}秒");
    }
    private void EnterActivePhase()
    {
        currentPhase = AttackPhase.Active;
        phaseTimer = activeTime;

        Debug.Log($"{enemy.name}:攻撃判定開始。{activeTime}秒");
    }
    private void EnterRecoveryPhase()
    {
        currentPhase = AttackPhase.Recovery;
        phaseTimer = recoveryTime;

        Debug.Log($"{enemy.name}:攻撃判定終了、後隙開始。{recoveryTime}秒");
    }
    private void CompleteAttack()
    {
        Debug.Log($"{enemy.name}:攻撃終了。追跡状態に戻ります");
        enemy.StateMachine.SwitchState<EnemyChaseState>();
    
    }

}
