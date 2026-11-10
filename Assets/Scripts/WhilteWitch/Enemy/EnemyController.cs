using UnityEngine;

public sealed class EnemyController : MonoBehaviour
{
    [field: SerializeField]
    public EnemyStatsData Stats { get; private set; }

    public EnemyStateMachine StateMachine { get; private set; }

    public EnemyBehaviorModule Behavior { get; private set; }
    public EnemyReactionModule Reaction { get; private set; }
    public EnemyStatusModule Status { get; private set; }
    

    private void Awake()
    {
        StateMachine = GetComponent<EnemyStateMachine>();

        Behavior = GetComponent<EnemyBehaviorModule>();
        Reaction = GetComponent<EnemyReactionModule>();    
        Status = GetComponent<EnemyStatusModule>();
    }
}
