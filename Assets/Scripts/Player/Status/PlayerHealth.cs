using UnityEngine;
using UnityEngine.AI;

public sealed class PlayerHealth : MonoBehaviour, IPlayerDamageReceiver
{
    private PlayerController player;
    private float currentHealth;
    //外部から読み取る用プロパティ
    public float CurrentHealth => currentHealth;
    public float MaxHealth => player.Stats.MaxHealth;
    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        currentHealth = MaxHealth;
    }

    public void ReceiveHit(PlayerHitData hitdata)
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
            $"HP {currentHealth}/{MaxHealth}");

        if(IsDead)
        {
            Debug.Log("Player HPが0になりました");
            //ゲームオーバー判定、ステータスがdeadに？
        }
    }

}