using UnityEngine;

public class EnemyStun : MonoBehaviour
{
    public bool IsStunned { get; private set; }

    private float stunTimer;

    private EnemyMovement movement;
    private EnemyCombat combat;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();

        IsStunned = false;
        stunTimer = 0f;
    }
}
