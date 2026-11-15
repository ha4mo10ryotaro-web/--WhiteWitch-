using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStatsData",
    menuName = "White Witch/Player/Stats Data")]
public class PlayerStatsData : ScriptableObject
{
    [Header("基本能力")]
    [Min(1f)] public float maxHealth = 100f;
    [Min(0f)] public float moveSpeed = 3.5f;
    [Tooltip("耐衝撃値。攻撃の衝撃値より小さいほど怯みやすい。")]
    [Min(0.01f)] public float impactResistance = 30f;
    [Tooltip("重量。ノックバック耐性などに関わる。")]
    [Min(0.01f)] public float weight = 1f;

}