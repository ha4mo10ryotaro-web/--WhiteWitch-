using UnityEngine;

public class EnemyExploreState : EnemyStateBase
{
    private readonly IEnemyExploreContext context;

    public EnemyExploreState(
        EnemyController enemy,
        IEnemyExploreContext context)
        : base(enemy)
    {
        this.context = context;
    }

    public override void Enter()
    {
        Debug.Log("探索状態に入りました");
    }

    public override void Update()
    {
        /*検知スクリプト君がプレイヤーを見つけたら(CanSeePlayer(仮))追跡状態に切り替える
        if (context.Perception.CanSeePlayer)
        {
            enemy.StateMachine.SwitchState<EnemyChaseState>();
            return;
        }
        */
        // 探索処理
    }

    public override void Exit()
    {
    }
}