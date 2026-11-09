using UnityEngine;

public sealed class EnemyController : MonoBehaviour
{
    [field: SerializeField]
    public EnemyStatsData Stats { get; private set; }

    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyHealth Health { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public EnemyPerception Perception { get; private set; }
    public EnemyCombat Combat { get; private set; }
    public EnemyKnockback Knockback { get; private set; }
    

    private void Awake()
    {
        StateMachine = GetComponent<EnemyStateMachine>();
        Health = GetComponent<EnemyHealth>();
        Movement = GetComponent<EnemyMovement>();
        Perception = GetComponent<EnemyPerception>();
        Combat = GetComponent<EnemyCombat>();
        Knockback = GetComponent<EnemyKnockback>();
    }
}
