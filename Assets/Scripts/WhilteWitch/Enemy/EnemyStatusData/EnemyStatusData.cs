using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyStatsData",
    menuName = "White Witch/Enemy/Stats Data")]
public class EnemyStatsData : ScriptableObject
{
    [Header("基本能力")]
    [Min(1f)] public float maxHealth = 100f;
    [Min(0f)] public float moveSpeed = 3.5f;
    [Tooltip("耐衝撃値。攻撃の衝撃値より小さいほど怯みやすい。")]
    [Min(0.01f)] public float impactResistance = 30f;
    [Tooltip("重量。ノックバック耐性などに関わる。")]
    [Min(0.01f)] public float weight = 1f;

    [Header("探索")]
    [Min(0f)] public float wanderRadius = 8f;
    [Min(0f)] public float wanderWaitTime = 2f;

    [Header("索敵")]
    [Tooltip("プレイヤーを視認できる距離")]
    [Min(0f)] public float detectionRange = 15f;
    [Tooltip("プレイヤーを見失う距離")]
    [Min(0f)] public float loseTargetRange = 25f;
    [Tooltip("プレイヤーを視認できる視野角")]
    [Range(0f, 360f)] public float fieldOfView = 120f;

    [Header("戦闘")]
    [Min(0f)] public float attackRange = 2.5f;
    [Min(0f)] public float attackCooldown = 1.5f;
}