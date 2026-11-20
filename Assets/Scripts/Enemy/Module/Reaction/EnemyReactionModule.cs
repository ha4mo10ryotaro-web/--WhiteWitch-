using UnityEngine;
[RequireComponent(typeof(EnemyStateMachine))]
[RequireComponent(typeof(EnemyBehaviorModule))]
[RequireComponent(typeof(EnemyStatusModule))]
[RequireComponent(typeof(EnemyStagger))]
[RequireComponent(typeof(EnemyKnockback))]
//反応部門の責任者。ノックバックやスタンなどの反応を管理する
public sealed class EnemyReactionModule : MonoBehaviour
{
    public EnemyStagger Stagger{get; private set;}
    public EnemyKnockback Knockback {get; private set;}
    public EnemyStun Stun {get; private set;}

    private EnemyStatusModule status;
    private EnemyBehaviorModule behavior;
    private EnemyStateMachine stateMachine;



    public bool IsBlockingState =>
        (Stagger != null && Stagger.IsStaggered) ||
        (Knockback != null && Knockback.IsKnocked) ||
        (Stun != null && Stun.IsStunned);

    private void Awake()
    {
        Stagger = GetComponent<EnemyStagger>();
        Knockback = GetComponent<EnemyKnockback>();
        Stun = GetComponent<EnemyStun>();

        status = GetComponent<EnemyStatusModule>();
        behavior = GetComponent<EnemyBehaviorModule>();
        stateMachine = GetComponent<EnemyStateMachine>();
    }


    public void RequestStagger(float duration)
    {
        if (status.Health.IsDead)
        {
            return;
        }

        behavior.Movement.Stop();
        behavior.Combat.CancelAttack();

        if (stateMachine.CurrentStateType == typeof(EnemyAttackState))
        {
            stateMachine.SwitchState<EnemyChaseState>();
        }

        Stagger.BeginStagger(duration);
    }

    public void RequestKnockback(
        Vector3 direction,
        float distance)
    {
        if (status.Health.IsDead)
        {
            return;
        }

        behavior.Movement.Stop();

        Knockback.BeginKnockback(
            direction,
            distance);
    }
}