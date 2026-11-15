using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private EnemyCombat combat;

    private void Awake()
    {
        combat = GetComponentInParent<EnemyCombat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!combat.IsAttacking)
        {
            return;
        }

        combat.OnHit(other);
    }

}
