using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyHitModifierSettings",
    menuName = "WhiteWitch/Enemy/Counter Modifier")]
public class EnemyCounterModifierData : ScriptableObject
{
    [field: SerializeField, Min(0f)]
    public float DamageMultiplier {get; private set;}

    [field: SerializeField, Min(0f)]
    public float ImpactMultiplier {get; private set;}

    [field: SerializeField, Min(0f)]
    public float ImpactResistanceMultiplier {get; private set;}

    [field: SerializeField, Min(0f)]
    public float KnockbackMultiplier {get; private set;}

    [field: SerializeField, Min(0f)]
    public float StaggerDurationMultiplier {get; private set;}
}