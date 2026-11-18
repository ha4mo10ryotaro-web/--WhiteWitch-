using UnityEngine;

public class EnemyDeadState : EnemyStateBase
{
    private readonly IEnemyDeadContext context;
    public EnemyDeadState(
        EnemyController enemy,
        IEnemyDeadContext context) : base(enemy)
    {
        this.context = context;
    }


    //override……親クラス側でabstractで丸投げされてた中身の処理を上書きするよ！
    public override void Enter()
    {
        context.Movement.Stop();
        context.Combat.CancelAttack();

        Debug.Log($"{enemy.name}:死亡");
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }

}
