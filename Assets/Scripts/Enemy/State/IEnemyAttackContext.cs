using UnityEngine;

public interface IEnemyAttackContext
{
    EnemyMovement Movement{get;}
    EnemyCombat Combat{get;}
}
