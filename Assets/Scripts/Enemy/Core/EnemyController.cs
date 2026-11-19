using UnityEngine;

public sealed class EnemyController : MonoBehaviour
{
    //仮置き。攻撃目標のTransformを取得するために使用する
    [field: SerializeField]
    public Transform Target { get; private set; }

    public EnemyStateMachine StateMachine { get; private set; }


    //敵の行動部門。遅延取得させる
    private EnemyBehaviorModule behavior;
    public EnemyBehaviorModule Behavior { get
        {
            if(behavior == null)
            {
                behavior = GetComponent<EnemyBehaviorModule>();
            }
            return behavior;
        }}
    public EnemyReactionModule Reaction { get; private set; }
    public EnemyStatusModule Status { get; private set; }
    
    public Vector3 SpawnPos{ get; private set; }
    public Vector3 CurrentPos => transform.position;

    private void Awake()
    {
        StateMachine = GetComponent<EnemyStateMachine>();

        Reaction = GetComponent<EnemyReactionModule>();    
        Status = GetComponent<EnemyStatusModule>();
        SpawnPos = transform.position;
    }

    public void RequestDeath()
    {
        StateMachine.SwitchState<EnemyDeadState>();
    }
}
