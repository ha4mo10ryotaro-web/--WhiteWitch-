using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field:SerializeField]
    public PlayerStatsData Stats{get; private set;}
    
    //自動で補足する攻撃対象
    [field:SerializeField]
    public Transform Tartget{get; private set;}

    public PlayerStateMachine StateMachine{get; private set;}

    private PlayerBehaviorModule behavior;
    public PlayerBehaviorModule Behavior {get
        {
            if(behavior == null)
            {
                behavior = GetComponent<PlayerBehaviorModule>();
            }
            return behavior;
            
        }
    }

    private void Awake()
    {
        StateMachine = GetComponent<PlayerStateMachine>();
    }
}
