
//敵の探索状態の責任者。敵の探索状態に必要な情報のみを管理する
public interface IEnemyExploreContext 
{
    EnemyMovement Movement { get; } 
    EnemyPerception Perception { get; }
}
