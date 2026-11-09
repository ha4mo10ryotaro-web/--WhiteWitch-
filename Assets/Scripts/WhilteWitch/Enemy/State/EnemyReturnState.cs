using UnityEngine;

public class EnemyReturnState : EnemyStateBase
{
    public EnemyReturnState(EnemyController enemy) : base(enemy)
    {
        //stateMachine(管理者)の登録処理はEnemyStateBaseにある
    }


    //override……親クラス側でabstractで丸投げされてた中身の処理を上書きするよ！
    public override void Enter()
    {
            Debug.Log("探索状態に入りました");
    }


    public override void Update()
    {
        /*
        if(プレイヤー発見処理)
        {
            stateMachine.SwitchState(typeof(EnemyChaseState));
        }
        */
    }


    public override void Exit()
    {


    }

}
