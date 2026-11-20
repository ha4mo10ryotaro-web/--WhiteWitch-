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
        //死んでたら攻撃を受け付けない
        if (health.IsDead)
        {
            return;
        }

        //攻撃側のデータをもとに、自身のステータスで補正した最終結果のデータを作成
        EnemyHitResult result =
            hitResolver.Resolve(hitData);

        Debug.Log(
            $"HitResult: " +
            $"Damage={result.Damage}, " +
            $"Counter={result.CounterType}, " +
            $"Stagger={result.StaggerResult}, " +
            $"StaggerTime={result.StaggerDuration}, " +
            $"Knockback={result.Knockback}");

        health.TakeDamage(result.Damage);

        //死んでいたら怯みやノックバックしない
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
