public interface IEnemyChaseContext
{
    EnemyMovement Movement { get; }
    EnemyPerception Perception { get; }
    EnemyCombat Combat { get; }
}
