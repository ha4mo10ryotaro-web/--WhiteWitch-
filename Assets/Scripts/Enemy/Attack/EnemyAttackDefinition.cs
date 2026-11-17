using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyAttackkDefinition",
    menuName = "WhiteWitch/Combat/Enemy Attack Definition"
)]
public sealed class EnemyAttackDefinition : ScriptableObject
{


    [Header("攻撃ステータス")]
    [SerializeField, Min(0f)] private float damage = 10f;
    [SerializeField, Min(0f)] private float impact = 10f;
    [SerializeField, Min(0f)] private float knockback = 1f;

    [Header("浸食")]
    [SerializeField, Min(0f)] private float corruption = 1f;

    [Header("攻撃時間内訳")]
    [SerializeField, Min(0f)] private float startupTime = 0.8f;
    [SerializeField, Min(0f)] private float activeTime = 0.3f;
    [SerializeField, Min(0f)] private float recoveryTime = 1.0f;

    public float StartupTime => startupTime;
    public float ActiveTime => activeTime;
    public float RecoveryTime => recoveryTime;
    public float Corruption => corruption;
    public float Damage => damage;
    public float Impact => impact;
    public float Knockback => knockback;
}
