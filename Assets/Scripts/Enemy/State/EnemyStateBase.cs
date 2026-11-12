//インターフェース側で３つの処理の実装を継承した全クラスに強制させる
public interface IEnemyState
{
    //ステータス変移時の処理
    void Enter();
    //ステータス間の処理
    void Update();
    //ステータス終了時の処理
    void Exit();
}



//敵ステータスが継承する親となるクラス
public abstract class EnemyStateBase : IEnemyState
{
    //敵全体の初期化・スクリプト（コンポーネント）の参照先をまとめる窓口役。
    //readonly（読み取り専用）にして他人とすげ変わらないようにできる。
    protected readonly EnemyController enemy;
    //ステータスの管理役。ステータスの遷移を仲介する。
    protected EnemyStateMachine StateMachine => enemy.StateMachine ;

    //各ステータスがインスタンス時に呼び出す。（同名クラスにして最初に処理が走るようになっている）
    //ステータス達の管理者（ステートマシン）や処理内で呼び出すコンポーネントの窓口は、今引数で持ってきたステートマシンだよ～と、
    // 一括で参照元にできる連絡帳を登録する。
    public EnemyStateBase(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    //IEnemyState（インターフェース）の実装
    //abstract……処理の中身は継承した子オブジェクトに任せる
    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();


}








