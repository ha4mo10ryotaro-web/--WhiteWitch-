using UnityEngine;

public class PlayerStaggerState : PlayerStateBase

{    private readonly IPlayerStaggerContext context;

    private float duration = 0.25f; //怯み時間
    private float elapsed;    //経過時間

    public PlayerStaggerState(
        PlayerStateMachine stateMachine,
        IPlayerStaggerContext context) 
        : base(stateMachine)
    {
        this.context = context;
    }

    public void SetDuration(float seconds)
    {
        //怯み時間をマイナスにはしない。秒数計算はStaggerStateに変移させる側が行う。
        duration = Mathf.Max(0f, seconds);
    }
    public override void Enter()
    {
        elapsed = 0f;
        //行動を強制キャンセル
        context.Movement.Stop();
        context.Combat.CancelCurrentAction();
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= duration)
        {
            StateMachine.SwitchState<PlayerNormalState>();
        }
    }
    public override void FixedUpdate()
    {
    }
    public override void Exit()
    {
    }
}
