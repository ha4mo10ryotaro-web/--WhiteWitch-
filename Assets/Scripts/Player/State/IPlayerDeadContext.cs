using UnityEngine.InputSystem;

public interface IPlayerDeadContext
{
    //各機能の停止をするため
    PlayerInputReader Input { get; }
    PlayerMovement Movement { get; }
    PlayerCombat Combat { get; }
}