using UnityEngine;

//行動部門の
public sealed class PlayerBehaviorModule :
 MonoBehaviour
{
    
    //動作。移動や入力など
    public PlayerMovement Movement { get; private set; }
    //戦闘。
    public PlayerCombat Combat { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
    }
}
