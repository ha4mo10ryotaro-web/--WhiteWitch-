using UnityEngine;

public sealed class PlayerAttackState : PlayerStateBase
{
    private readonly IPlayerAttackContext context;

    private PlayerAttackDefinition attack;
    private AttackPhase currentPhase = AttackPhase.None;
    private float phaseTimer;
    private bool completed;

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

        completed = false;
        phaseTimer = 0f;
        currentPhase = AttackPhase.None;

        attack = context.Combat.CurrentAttack;

        if (attack == null)
        {
            Debug.LogWarning(
                "Player: 使用可能な攻撃がありません");

            StateMachine.SwitchState<PlayerNormalState>();
            return;
        }

        EnterStartupPhase();
    }

    public override void Update()
    {
        if (attack == null)
        {
            StateMachine.SwitchState<PlayerNormalState>();
            return;
        }

        phaseTimer -= Time.deltaTime;

        if (phaseTimer > 0f)
        {
            return;
        }

        switch (currentPhase)
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

            case AttackPhase.None:
            default:
                Debug.LogWarning(
                    $"Player: 不正な攻撃フェーズです: {currentPhase}");

                StateMachine.SwitchState<PlayerNormalState>();
                break;
        }
    }

    public override void FixedUpdate()
    {
    }

    public override void Exit()
    {
        if (!completed)
        {
            context.Combat.CancelAttack();
        }

        currentPhase = AttackPhase.None;
        phaseTimer = 0f;
        attack = null;
    }

    private void EnterStartupPhase()
    {
        currentPhase = AttackPhase.Startup;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = Mathf.Max(0f, attack.StartupTime);

        Debug.Log($"Player: {attack.name} 攻撃開始");
    }

    private void EnterActivePhase()
    {
        currentPhase = AttackPhase.Active;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = Mathf.Max(0f, attack.ActiveTime);

        if (!context.Combat.BeginAttack())
        {
            Debug.LogWarning(
                "Player: Activeフェーズを開始できませんでした");

            StateMachine.SwitchState<PlayerNormalState>();
        }
    }

    private void EnterRecoveryPhase()
    {
        context.Combat.EndAttack();

        currentPhase = AttackPhase.Recovery;
        context.Combat.SetAttackPhase(currentPhase);

        phaseTimer = Mathf.Max(0f, attack.RecoveryTime);
    }

    private void CompleteAttack()
    {
        completed = true;

        context.Combat.FinishAttack();

        Debug.Log("Player: 攻撃終了。通常状態に戻ります");

        StateMachine.SwitchState<PlayerNormalState>();
    }
}