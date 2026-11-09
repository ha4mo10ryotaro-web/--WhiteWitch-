using UnityEngine;

public class EnemyExploreState : EnemyStateBase
{
    //クラス名と同名のコンストラクタが一番最初に呼ばれるため同名の必要アリ
    //当クラスのインスタンス時に引数として渡されたEnemyStateMachineをbase(親クラス)に更に引数として渡す！
    //親クラスのコンストラクタが走る
    public EnemyExploreState(EnemyController enemy) : base(enemy)
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
