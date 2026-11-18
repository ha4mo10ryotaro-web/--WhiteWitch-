using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public bool IsKnocked { get; private set; }

    private float knockbackTimer;

    private EnemyMovement movement;
    private EnemyCombat combat;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();

        IsKnocked = false;
        knockbackTimer = 0f;
    }
}
