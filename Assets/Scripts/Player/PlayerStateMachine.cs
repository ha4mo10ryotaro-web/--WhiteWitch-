using System;//Type使うため
using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]


//ステータス変移を管理する
public class PlayerStateMachine : MonoBehaviour
{
    //ステートマシンはコントローラーを持たない
    //private PlayerController player;


    //現在のステータス
    private IPlayerState currentState;
    private bool initialized;

    //すべてのステータスを掘図するディクショナリを作成
    //クラスで管理するためのキーの種類
    private  readonly Dictionary<Type, IPlayerState> states = new();

    //外側から現在のStateの型”のみ”を参照させる
    //呼び出される度に右辺を計算して結果を返す、読み取り専用プロパティ。そも代入が外部からできない。
    public Type CurrentStateType => currentState?.GetType();


    //コントローラー（総責任者）がこの初期化を実行する
    public void Initialize(PlayerController player)
    {
        //既に初期化済みなら
        if(initialized)
        {
            return;
        }

        states.Add(
            typeof(PlayerNormalState), 
             new PlayerNormalState(this,player));
        states.Add(
            typeof(PlayerAttackState),
            new PlayerAttackState(this,player));

        states.Add(
            typeof(PlayerStaggerState),
            new PlayerStaggerState(this,player));

        states.Add(
            typeof(PlayerDeadState),
            new PlayerDeadState(this,player));

        initialized = true;
    }

    //開始時にコントローラーから呼び出される初期ステータス設定
    public void StartStateMachine<State>()
        where State : class, IPlayerState
    {
        if (currentState != null)
        {
            return;
        }
        SwitchState<State>();
    }

    private void Update()
    {
        //null条件演算子として?が付けられている。currentStateがnullの時は実行しない。
        //管理者が現在のステータスのUpdateを呼び出す
        currentState?.Update();
    }
    private void FixedUpdate()
    {
        //null条件演算子として?が付けられている。currentStateがnullの時は実行しない。
        //管理者が現在のステータスのUpdateを呼び出す
        currentState?.FixedUpdate();
    }

    //SwitchStateの<>で指定したクラスはStateとして扱えますか？をコンパイル時に判定してくれる
    //ついでにtypeofの記述が省略できる
    public void SwitchState<State>() where State : class, IPlayerState
    {
        SwitchState(typeof(State));
    }

    //同名メソッドのオーバーロード。引数が異なるのでオッケー。同じメソッドが呼び出されても、引数の形で実行するメソッドを分けられる。
    private void SwitchState(Type newStateType)
    {

        if(!initialized)
        {
            Debug.LogError($"{this}:Playerのステートマシンが初期化されていません");
            return;
        }

        //newStateTypeをstatesディクショナリーから探し出し、
        //見つかればtrueを返し、更に戻り値としてnewStateTypeをキー値に持つバリュー（IPlayerState実装クラス）をnewStateに返す

        if(!states.TryGetValue(newStateType, out IPlayerState newState))
        {
            //newStateTypeが見つかりません！
            Debug.LogError($"{newStateType}が見つかりません");
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
    //現在のステータスとは別に、登録済みステータスを取得する
    public State GetState<State>() where State : class, IPlayerState
    {
        if(!states.TryGetValue(typeof(State), out IPlayerState state))
        {
            //newStateTypeが見つかりません！
            Debug.LogWarning($"Playerに{typeof(State)}が見つかりません");
            return null;
        }
        return  state as State;
    }

    //今のステータスがStateと同一か？
    public bool IsInState<State>() where State : IPlayerState
        => currentState is State;
}
