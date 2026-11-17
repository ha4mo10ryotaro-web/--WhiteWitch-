using UnityEngine;
using UnityEngine.AI;

public sealed class PlayerHealth : MonoBehaviour, IPlayerDamageReceiver
{
    [SerializeField]
    private float maxHealth = 100f;

    private float currentHealth;
    //外部から読み取る用プロパティ
    public float CurrentHealth => currentHealth;
    public float MaxHealth =>maxHealth;
    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        currentHealth = maxHealth;
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
            $"Player被弾！ {hitdata.Damage}ダメージ / " +
            $"HP {currentHealth}/{maxHealth}");

        if(IsDead)
        {
            Debug.Log("Player HPが0になりました");
            //ゲームオーバー判定、ステータスがdeadに？
        }
    }

}