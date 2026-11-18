using Unity.VisualScripting;
using UnityEngine;

//行動部門の
public sealed class EnemyBehaviorModule :
 MonoBehaviour,
 IEnemyExploreContext,
 IEnemyChaseContext,
 IEnemyReturnContext,
 IEnemyAttackContext,
 IEnemyDeadContext
{
    
    public EnemyMovement Movement { get; private set; }
    public EnemyPerception Perception { get; private set; }
    public EnemyCombat Combat { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<EnemyMovement>();
        Perception = GetComponentInChildren<EnemyPerception>();
        Combat = GetComponent<EnemyCombat>();
    }
}