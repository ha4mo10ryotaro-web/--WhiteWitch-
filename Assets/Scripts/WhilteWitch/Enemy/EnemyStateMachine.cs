using System;//Type使うため
using UnityEngine;
using System.Collections.Generic;


//このスクリプトをGameObjにアタッチする時、指定したコンポーネント（エネミーコントローラー）を要求する
//コンポーネントがGameObjにアタッチされてなければ自動で追加する
[RequireComponent(typeof(EnemyController))]

//ステータス変移を管理する
public class EnemyStateMachine : MonoBehaviour
{
    private EnemyController enemy;
    //現在のステータス
    private IEnemyState currentState;

    //すべてのステータスを掘図するディクショナリを作成
    //クラスで管理するためのキーの種類
    private Dictionary<Type, IEnemyState> states;

    //外側から現在のStateの型”のみ”を参照させる
    //呼び出される度に右辺を計算して結果を返す、読み取り専用プロパティ。そも代入が外部からできない。
    public Type CurrentStateType => currentState?.GetType();

    //ゲーム開始時(生成時とは異なる)に実行される
    void Awake()
    {
        enemy = GetComponent<EnemyController>();

        //クラスを扱うため、newでインスタンス化（生成）する必要がある
        //System.Type……クラスやインターフェース情内の情報を扱う型。バリューのクラスによって情報のつくりはことなるため、typeofを使う。
        //バリューのIEnemyState型は、IEnemyStateを実装したクラスだよ～（それ以外はお断り）としている
        //生成したクラスの引数にエネミーコントローラーを渡すことで、ステータス側がこのステートマシンをコントローラー経由で呼び出せる
        states = new Dictionary<Type, IEnemyState>()
        {
            { typeof(EnemyExploreState), new EnemyExploreState(enemy, enemy.Behavior) },
            { typeof(EnemyChaseState), new EnemyChaseState(enemy) },
            { typeof(EnemyAttackState), new EnemyAttackState(enemy) },

            { typeof(EnemyReturnState), new EnemyReturnState(enemy) },
            { typeof(EnemyDeadState), new EnemyDeadState(enemy) },
        };

    }

    private void Start()
    {
        //初期ステータス決め
        SwitchState<EnemyExploreState>();
    }

    void Update()
    {
        //null条件演算子として?が付けられている。currentStateがnullの時は実行しない。
        //管理者が現在のステータスのUpdateを呼び出す
        currentState?.Update();
    }

    //SwitchStateの<>で指定したクラスはStateとして扱えますか？をコンパイル時に判定してくれる
    //ついでにtypeofの記述が省略できる
    public void SwitchState<TState>() where TState : IEnemyState
    {
        SwitchState(typeof(TState));
    }

    //同名メソッドのオーバーロード。引数が異なるのでオッケー。同じメソッドが呼び出されても、引数の形で実行するメソッドを分けられる。
    private void SwitchState(Type newStateType)
    {

        //newStateTypeをstatesディクショナリーから探し出し、
        //見つかればtrueを返し、更に戻り値としてnewStateTypeをキー値に持つバリュー（IPlayerState実装クラス）をnewStateに返す

        if(!states.TryGetValue(newStateType, out IEnemyState newState))
        {
            //newStateTypeが見つかりません！
            Debug.LogError($"State not found: {newStateType}");
            return;
        }

        //クラスの参照比較をするためのもの。インスタンス（アドレス）で判定するらしい。
        //Objに＝＝を使うと特殊な挙動になるため使わない。
        //現在のステータスと同じステータスに移行しようとしてる時は切り替えを行わない。
        if(ReferenceEquals(currentState, newState))
        {
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

    }




}
