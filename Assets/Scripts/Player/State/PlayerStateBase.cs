//インターフェース側で4つの処理の実装を継承した全クラスに強制させる
public interface IPlayerState
{
    //ステータス変移時の処理
    void Enter();
    //ステータス間の処理
    void Update();
    void FixedUpdate();
    //ステータス終了時の処理
    void Exit();
}



//プレイヤーステータスが継承する親となるクラス
public abstract class PlayerStateBase : IPlayerState
{
    //ステータスの管理役。ステータスの遷移を仲介する。
    protected PlayerStateMachine StateMachine;

    public PlayerStateBase(PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    //IPlayerState（インターフェース）の実装
    //abstract……処理の中身は継承した子オブジェクトに任せる
    public abstract void Enter();

    public abstract void Update();
    public abstract void FixedUpdate();

    public abstract void Exit();


}

