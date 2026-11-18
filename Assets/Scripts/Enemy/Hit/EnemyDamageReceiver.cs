using UnityEngine;

public sealed class EnemyDamageReceiver :
    MonoBehaviour,
    IEnemyDamageReceiver
{
    private EnemyHealth health;
    private EnemyHitResolver hitResolver;
    private EnemyReactionModule reaction;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        hitResolver = GetComponent<EnemyHitResolver>();
        reaction = GetComponent<EnemyReactionModule>();
    }

    public void ReceiveHit(
        EnemyHitData hitData)
    {
        if (health.IsDead)
        {
            return;
        }

        EnemyHitResult result =
            hitResolver.Resolve(hitData);

        health.TakeDamage(result.Damage);

        if (health.IsDead)
        {
            return;
        }

        if (result.ShouldStagger)
        {
            reaction.RequestStagger(
                result.StaggerDuration);
        }

        if (result.ShouldKnockback)
        {
            reaction.RequestKnockback(
                result.KnockbackDirection,
                result.Knockback);
        }
    }
}
