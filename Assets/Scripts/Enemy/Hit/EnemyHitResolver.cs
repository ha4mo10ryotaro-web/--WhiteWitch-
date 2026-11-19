using UnityEngine;

[DisallowMultipleComponent]
public sealed class EnemyHitResolver : MonoBehaviour
{
    private EnemyBehaviorModule behavior;
    private EnemyStatusModule status;
    [SerializeField]
    private EnemyCounterModifierSettings modifierSettings;

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
                hitData.Knockback,modifier);

        float staggerDuration = ResolveStaggerDuration(
                modifier);

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
        float effectiveImpact =
            incomingImpact * counterModifier.ImpactMultiplier;

        float effectiveResistance =
            ResolveImpactResistance(
                status.DynamicStats.CurrentImpactResistance,
                counterModifier.ImpactResistanceMultiplier);

        return StaggerResult.None;
    }

    private float ResolveImpactResistance(float resistance, float multiplyer)
    {
        //耐衝撃値側の補正はcurrentStats側で
        return resistance * multiplyer;
    }

    private float ResolveKnockback (float knockback, EnemyCounterModifierData modifier)
    {
        //重さによる具体的なノックバック距離の計算式は未定
        //staggerResultによる強怯みも後々？
        return knockback / status.Stats.Weight * modifier.KnockbackMultiplier;
    }

    private float ResolveStaggerDuration(EnemyCounterModifierData modifier)
    {
        //staggerResultによる強怯みも後々？
        return 1 * modifier.StaggerDurationMultiplier;
    }
}