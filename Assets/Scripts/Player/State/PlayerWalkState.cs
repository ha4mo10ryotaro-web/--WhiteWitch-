using UnityEngine;

public class PlayerWalkState : PlayerStateBase

{    private readonly IPlayerWalkContext context;
    public PlayerWalkState(
        PlayerController player,
        IPlayerWalkContext context) 
        : base(player)
    {
        this.context = context;
    }

    public override void Enter()
    {

        return;
    }

    public override void Update()
    {

        return;

    }
    public override void Exit()
    {

        return;
    }
}
