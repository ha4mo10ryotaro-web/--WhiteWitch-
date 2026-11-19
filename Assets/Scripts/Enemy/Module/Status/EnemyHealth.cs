using UnityEngine;
[RequireComponent(typeof(EnemyController))]
public sealed class EnemyHealth : MonoBehaviour
{
    private EnemyController enemy;
    private EnemyStatusModule status;
    private float currentHealth;
    private float maxHealth;

    //外部から読み取る用プロパティ
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
        status = GetComponent<EnemyStatusModule>();

        if(status.Stats == null)
        {
            Debug.LogError(
                $"{name}:EnemyStatsDataが設定されていません",this
            );
        }
        
        maxHealth = status.Stats.MaxHealth;
        currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if(IsDead)
        {
            return;
        }

        currentHealth = Mathf.Max(
            currentHealth - damage,
            0f
        );

        Debug.Log(
            $"{name}被弾！ {damage}ダメージ / " +
            $"HP {currentHealth}/{MaxHealth}");

        if(IsDead)
        {
            enemy.RequestDeath();
            Debug.Log($"{name}:HPが0になりました");
        }
    }

}