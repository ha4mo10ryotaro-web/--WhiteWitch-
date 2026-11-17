using UnityEngine;

[DisallowMultipleComponent]//このスクリプトの重複を禁止する
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombat))]
//コントローラにコンテクストを実装させる
public sealed class PlayerController : 
    MonoBehaviour,
    IPlayerNormalContext,
    IPlayerAttackContext,
    IPlayerStaggerContext,
    IPlayerDeadContext
{
    [field: SerializeField] 
    public PlayerStatsData Stats { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerInputReader Input { get; private set; }
    public PlayerMovement Movement {get; private set;}
    public PlayerCombat Combat { get; private set; }


    private void Awake()
    {
        StateMachine = GetComponent<PlayerStateMachine>();
        Input = GetComponent<PlayerInputReader>();


        StateMachine.Initialize(this);
    }

    private void Start()
    {
        StateMachine.StartStateMachine<PlayerNormalState>();
    }

    //いつでも割り込める、duration秒の怯みを実行。
    public void RequestStagger(float duration)
    {
        //死亡➡怯み➡通常にさせない。死んだら死。
        if (StateMachine.IsInState<PlayerDeadState>())
        {
            return;
        }
        StateMachine.GetState<PlayerStaggerState>()
        .SetDuration(duration);
        StateMachine.SwitchState<PlayerStaggerState>();
    }

    public void RequestDeath()
    {
        StateMachine.SwitchState<PlayerDeadState>();
    }
}
