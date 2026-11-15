using UnityEngine;

[CreateAssetMenu(
    fileName = "AttackDefinition",
    menuName = "WhiteWitch/Combat/Attack Definition"
)]
public sealed class AttackDefinition : ScriptableObject
{
    [Header("攻撃時間内訳")]
    [SerializeField] private float startupTime = 0.8f;
    [SerializeField] private float activeTime = 0.3f;
    [SerializeField] private float recoveryTime = 1.0f;

    [Header("攻撃ステータス")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float impact = 10f;
    [SerializeField] private float knockback = 1f;

    public float StartupTime => startupTime;
    public float ActiveTime => activeTime;
    public float RecoveryTime => recoveryTime;

    public float Damage => damage;
    public float Impact => impact;
    public float Knockback => knockback;
}
