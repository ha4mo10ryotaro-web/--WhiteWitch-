using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStatsData",
    menuName = "White Witch/Player/Stats Data")]
public class PlayerStatsData : ScriptableObject
{
    [Header("基本能力")]
    [Min(1f)] private float maxHealth = 100f;
    [Min(0f)] private float moveSpeed = 3.5f;
    [Tooltip("耐衝撃値。攻撃の衝撃値より小さいほど怯みやすい。")]
    [Min(0.01f)] private float impactResistance = 30f;
    [Tooltip("重量。ノックバック耐性などに関わる。")]
    [Min(0.01f)] private float weight = 1f;

    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float ImpactResistance => impactResistance;
    public float Weight => weight;

}