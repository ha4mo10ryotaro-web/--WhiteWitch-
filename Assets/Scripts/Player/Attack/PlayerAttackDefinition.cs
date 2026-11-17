using UnityEngine;


[CreateAssetMenu(
    menuName = "WhiteWitch/Combat/Player Attack")]
public sealed class PlayerAttackDefinition : ScriptableObject
{
    [Header("戦闘")]
    [SerializeField, Min(0f)]private float baseDamage;
    [SerializeField, Min(0f)]private float baseImpact;
    [SerializeField, Min(0f)]private float baseKnockback;

    [Header("リソース")]
    [SerializeField, Min(0f)]private float inkCost;

    [Header("色 / 解像")]
    [SerializeField, Min(0f)]private float colorAccumulation;
    [SerializeField, Min(0f)]private float resolutionGain;

    [Header("移動")]
    [SerializeField, Min(0f)] private float forwardMovement;

    [Header("攻撃時間内訳")]
    [SerializeField, Min(0f)]private float windupTime;
    [SerializeField, Min(0f)]private float activeTime;
    [SerializeField, Min(0f)]private float recoveryTime;

    public float BaseDamage => baseDamage;
    public float BaseImpact => baseImpact;
    public float BaseKnockback => baseKnockback;

    public float InkCost => inkCost;

    public float ColorAccumulation => colorAccumulation;
    public float ResolutionGain => resolutionGain;

    public float ForwardMovement => forwardMovement;

    public float WindupTime => windupTime;
    public float ActiveTime => activeTime;
    public float RecoveryTime => recoveryTime;

}