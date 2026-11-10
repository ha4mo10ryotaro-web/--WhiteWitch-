using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    private IEnemyChaseContext context;
    public EnemyChaseState(
        EnemyController enemy,
        IEnemyChaseContext context) : base(enemy)
    {
        this.context = context;
    }


    //override……親クラス側でabstractで丸投げされてた中身の処理を上書きするよ！
    public override void Enter()
    {
            Debug.Log("追跡状態に入りました");
    }


    public override void Update()
    {
        if(enemy.Target == null)
        {
            return;
        }

        
        if(context.Combat.IsInAttackRange(enemy.Target))
        {
            enemy.StateMachine.SwitchState<EnemyAttackState>();
            return;
        }

        context.Movement.MoveTo(enemy.Target.position);
    }

    public override void Exit()
    {
        context.Movement.Stop();
    }

}
