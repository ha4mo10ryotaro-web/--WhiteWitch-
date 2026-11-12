using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    private readonly IEnemyAttackContext context;

    public EnemyAttackState(
        EnemyController enemy,
        IEnemyAttackContext context) 
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

    }


    public override void Exit()
    {


    }

}
