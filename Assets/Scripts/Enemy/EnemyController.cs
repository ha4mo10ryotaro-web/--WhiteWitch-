using UnityEngine;

public sealed class EnemyController : MonoBehaviour
{
    [field: SerializeField]
    public EnemyStatsData Stats { get; private set; }
    //仮置き。攻撃目標のTransformを取得するために使用する
    [field: SerializeField]
    public Transform Target { get; private set; }

    public EnemyStateMachine StateMachine { get; private set; }


    //敵の行動部門。遅延取得させる
    private EnemyBehaviorModule behaviour;
    public EnemyBehaviorModule Behavior { get
        {
            if(behaviour == null)
            {
                behaviour = GetComponent<EnemyBehaviorModule>();
            }
            return behaviour;
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
}
