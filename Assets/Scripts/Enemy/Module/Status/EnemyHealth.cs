using UnityEngine;
[RequireComponent(typeof(EnemyController))]
public sealed class EnemyHealth : MonoBehaviour, IEnemyDamageReceiver
{
    private EnemyController enemy;
    private float currentHealth;
    private float maxHealth;

    //外部から読み取る用プロパティ
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();

        if(enemy.Stats == null)
        {
            Debug.LogError(
                $"{name}:EnemyStatsDataが設定されていません",this
            );
        }
        
        maxHealth = enemy.Stats.MaxHealth;
        currentHealth = MaxHealth;
    }

    public void ReceiveHit(EnemyHitData hitdata)
    {
        if(IsDead)
        {
            return;
        }

        currentHealth = Mathf.Max(
            currentHealth - hitdata.Damage,
            0f
        );

        Debug.Log(
            $"{enemy.name}被弾！ {hitdata.Damage}ダメージ / " +
            $"HP {currentHealth}/{MaxHealth}");

        if(IsDead)
        {
            enemy.RequestDeath();
            Debug.Log($"{enemy.name}:HPが0になりました");
        }
    }

}