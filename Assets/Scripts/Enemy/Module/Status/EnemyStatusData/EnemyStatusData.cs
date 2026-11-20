using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyStatsData",
    menuName = "White Witch/Enemy/Stats Data")]
public class EnemyStatsData : ScriptableObject
{
    [Header("基本能力")]
    [SerializeField, Min(1f)] private float maxHealth = 100f;
    [SerializeField, Min(1f)] private float defence = 100f;
    [SerializeField, Min(0f)] private float moveSpeed = 3.5f;
    [Tooltip("重量。ノックバック耐性などに関わる。")]
    [SerializeField, Min(0.01f)] private float weight = 1f;

    [Header("耐性")]
    [Tooltip("耐衝撃値。攻撃の衝撃値より小さいほど怯みやすい。")]
    [SerializeField, Min(0.01f)] private float impactResistance = 30f;
    [SerializeField, Min(0.01f)] private float staggerDuration = 0.6f;

    [Header("徘徊")]
    [SerializeField, Min(0f)] private const float exploreRadius = 8f;
    [SerializeField, Min(0f)] private float exploreWaitTime = 2f;

    [Header("索敵")]
    [Tooltip("プレイヤーを視認できる距離")]
    [SerializeField, Min(0f)] private float detectionRange = 15f;
    [Tooltip("プレイヤーを見失う距離")]
    [SerializeField, Min(0f)] private float loseTargetRange = 25f;
    [Tooltip("プレイヤーを視認できる視野角")]
    [SerializeField, Range(0f, 360f)] private float horizontalView = 120f;
    [SerializeField, Range(0f, 180f)] private float verticalView = 120f;

    [Header("追跡")]
    [SerializeField, Min(0f)] private float chaseRadius = 25f;

    [Header("戦闘")]
    [SerializeField, Min(0f)] private float attackRange = 2.5f;
    [SerializeField, Min(0f)] private float attackCooldown = 1.5f;

    public float MaxHealth => maxHealth;
    public float Defence => defence;
    public float MoveSpeed => moveSpeed;
    public float Weight => weight;

    public float ImpactResistance => impactResistance;
    public float StaggerDuration => staggerDuration;


    public float ExploreRadius => exploreRadius;
    public float ExploreWaitTime => exploreWaitTime;

    public float DetectionRange => detectionRange;
    public float LoseTargetRange => loseTargetRange;
    public float HorizontalView => horizontalView;
    public float VerticalView => verticalView;

    public float ChaseRadius => chaseRadius;

    public float AttacRange => attackRange;
    public float AttackCooldown => attackCooldown;

}