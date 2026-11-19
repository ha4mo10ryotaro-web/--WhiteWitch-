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
    //privateにして、BehaviorModuleを実装しているコンテクスト達からはpublicでプロパティ化したステータスしか見れないようにする
    private EnemyStatusModule status;
    public EnemyStatsData Status { get; private set; }
    public EnemyMovement Movement { get; private set; }
    public EnemyPerception Perception { get; private set; }
    public EnemyCombat Combat { get; private set; }

    private void Awake()
    {
        status = GetComponent<EnemyStatusModule>();
        Status = status.Stats;
        Movement = GetComponent<EnemyMovement>();
        Perception = GetComponentInChildren<EnemyPerception>();
        Combat = GetComponent<EnemyCombat>();
    }
}