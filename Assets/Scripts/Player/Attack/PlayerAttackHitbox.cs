using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class PlayerAttackHitbox :
    MonoBehaviour
{
    [SerializeField]
    private PlayerCombat combat;

    private void Awake()
    {
        Collider hitbox =
            GetComponent<Collider>();
        combat = GetComponentInParent<PlayerCombat>();

        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (combat == null)
        {
            return;
        }

        combat.OnHit(other);
    }
}