using UnityEngine;
public readonly struct PlayerHitData
{
    public float Damage { get; }
    public float Impact { get; }
    public float Knockback { get; } 

    public Vector3 Direction { get; }
    public Vector3 HitPosition { get; }

    public GameObject Attacker { get; }
    public EnemyAttackDefinition Definition { get; }

    public PlayerHitData(
        float damage,
        float impact,
        float knockback,
        Vector3 direction,
        Vector3 hitPosition,
        GameObject attacker,
        EnemyAttackDefinition definition)
    {
        Damage = damage;
        Impact = impact;
        Knockback = knockback;

        Direction = direction;
        HitPosition = hitPosition;
        
        Attacker = attacker;
        Definition = definition;
    }
}
