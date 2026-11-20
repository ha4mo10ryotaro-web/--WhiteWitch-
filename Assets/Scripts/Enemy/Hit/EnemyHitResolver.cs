using UnityEngine;

[DisallowMultipleComponent]
public sealed class EnemyHitResolver : MonoBehaviour
{
    private EnemyBehaviorModule behavior;
    private EnemyStatusModule status;
    [SerializeField]
    private EnemyCounterModifierSettings modifierSettings;
    
    [SerializeField, Min(1f)]
    private float heavyStaggerBorderline = 1.5f;
    [SerializeField, Min(1f)]
    private float heavyStaggerMultiplier = 2.0f;

    private void Awake()
    {
        behavior =
            GetComponent<EnemyBehaviorModule>();

        status =
            GetComponent<EnemyStatusModule>();
    }

    public EnemyHitResult Resolve(
        EnemyHitData hitData)
    {
        CounterType counterType =
            SelectCounterType();

        EnemyCounterModifierData modifier =
            modifierSettings.Get(counterType);

        float appliedDamage = ResolveDamage(
                hitData.Damage, modifier);

        StaggerResult staggerResult = ResolveStagger(
                hitData.Impact,modifier);

        float appliedKnockback = ResolveKnockback(
                hitData.Knockback, staggerResult, modifier);

        float staggerDuration = ResolveStaggerDuration(
                staggerResult, modifier);

        return new EnemyHitResult(
            appliedDamage,
            appliedKnockback,
            hitData.Direction,
            staggerDuration,
            counterType,
            staggerResult);
    }

    //どのカウンター倍率を適用するの？
    private CounterType SelectCounterType ()
    {
        /*超解像時は常にパニカン判定。超解像を作ったら実装
        if (status.IsSuperResolved)
        {
            return CounterType.PunishCounter;
        }
        */
        //現在の戦闘フェーズでカウンター判定を切り替える
        switch(behavior.Combat.CurrentPhase)
        {
            case AttackPhase.Startup:
                return CounterType.Counter;
            case AttackPhase.Recovery:
                return CounterType.PunishCounter;

            //case AttackPhase.Active;
                //ResolveClash(相打ち判定);

            default:
                return CounterType.None;
        };

    }

    private float ResolveDamage(float damage, EnemyCounterModifierData modifier)
    {
        //ここから更に防御力補正が乗る
        float appliedDamage = damage * modifier.DamageMultiplier;

        return Mathf.Max(0f, appliedDamage);
    }

    //怯み判定。攻撃側の衝撃値とこちら側の衝撃値にお互いに補正を掛けた後に判定を行う
    private StaggerResult ResolveStagger(float incomingImpact, EnemyCounterModifierData counterModifier)
    {
        //今後はスーパーアーマー判定を入れる

        float effectiveImpact =
            incomingImpact * counterModifier.ImpactMultiplier;

        float effectiveResistance =
            ResolveImpactResistance(
                status.DynamicStats.CurrentImpactResistance,
                counterModifier.ImpactResistanceMultiplier);

        if(effectiveResistance <= 0f)
        {
            return StaggerResult.Guaranteed;
        }
        
        if(effectiveImpact <= 0f)
        {
            return StaggerResult.Failed;
        }

        float impactRate = effectiveImpact / effectiveResistance;
        if(impactRate >= heavyStaggerBorderline)
        {
            return StaggerResult.Heavy;
        }

        double successRate = ( Mathf.Pow((effectiveImpact / effectiveResistance), 3) - 0.1) / 0.9;

        if(successRate <= 0)
        {
            return StaggerResult.Failed;
        }

        if(successRate >= 1)
        {
            return StaggerResult.Guaranteed;
        }
        else if(successRate > Random.value)
        {
            return StaggerResult.ChanceSuccess;
        }
        else
        {
            return StaggerResult.Failed;
        }
    }

    private float ResolveImpactResistance(float resistance, float multiplyer)
    {
        //耐衝撃値側の補正はcurrentStats側で
        return resistance * multiplyer;
    }

    private float ResolveKnockback (float knockback, StaggerResult staggerResult, EnemyCounterModifierData modifier)
    {
        if (staggerResult == StaggerResult.None ||
        staggerResult == StaggerResult.Failed)
        {
            return 0f;
        }

        float Weight = Mathf.Max(0.01f, status.Stats.Weight);
        float appliedknockback =
            knockback / Weight * modifier.KnockbackMultiplier;

        if (staggerResult == StaggerResult.Heavy)
        {
            appliedknockback *= heavyStaggerMultiplier;
        }

        return  Mathf.Max(0f, appliedknockback);
    }

    private float ResolveStaggerDuration(StaggerResult staggerResult, EnemyCounterModifierData modifier)
    {
        if (staggerResult == StaggerResult.None ||
            staggerResult == StaggerResult.Failed)
        {
            return 0f;
        }

        float duration = status.Stats.StaggerDuration * modifier.StaggerDurationMultiplier;

        if (staggerResult == StaggerResult.Heavy)
        {
            duration *= heavyStaggerMultiplier;
        }
        return Mathf.Max(0f, duration);
    }
}