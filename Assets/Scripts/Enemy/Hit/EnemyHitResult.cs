using UnityEngine;

public enum CounterType
{
    None,
    Counter,
    PunishCounter
}

public enum StaggerResult
{
    None,
    Failed,
    ChanceSuccess,
    Guaranteed,
    Heavy
}

public readonly struct EnemyHitResult
{
    public float Damage { get; }
    public float Impact { get; }
    public float Knockback { get; }

    public Vector3 KnockbackDirection { get; }

    public CounterType CounterType { get; }
    public StaggerResult StaggerResult { get; }

    public bool ShouldStagger =>
        StaggerResult != StaggerResult.None &&
        StaggerResult != StaggerResult.Failed;

    public bool ShouldKnockback =>
        Knockback > 0f;

    public bool ShouldCancelAttack { get; }

    public float StaggerDuration { get; }

    public EnemyHitResult(
        float damage,
        float knockback,
        Vector3 knockbackDirection,
        float staggeraDuration,
        CounterType counterType,
        StaggerResult staggerResult)
    {
        Damage = damage;
        Knockback = knockback;
        KnockbackDirection = knockbackDirection;
        StaggerDuration = staggeraDuration;
        CounterType = counterType;
        StaggerResult = staggerResult;
    }
}