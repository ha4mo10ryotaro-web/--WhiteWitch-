using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class EnemyReturnState : EnemyStateBase
{
    public readonly IEnemyReturnContext context;
    public EnemyReturnState(
        EnemyController enemy, IEnemyReturnContext context)
         : base(enemy)
    {
        this.context = context;
    }

    //SpawnPos周辺のNavMeshを探索する範囲
    private const float ReturnSampleDistance = 2.0f;
    private Vector3 returnPos;
    //帰還地点に到着した？
    private bool hasReturnPos = false;

    //再試行
    private const float RetryInterval = 1.0f;
    private float retryTimer = 0f;

    public override void Enter()
    {
        hasReturnPos = false;
        //初回は再試行インターバル無し
        retryTimer = 0f;

        Debug.Log("帰還状態に入りました");

        TryStartReturning();
    }


    public override void Update()
    {
        //経路が見当たらなければ再試行、到着すれば帰還完了
        if(!hasReturnPos)
        {
            UpdateRetryingReturnPath();
            return;
        }
        if(!context.Movement.HasArrived)
        {
            return;
        }

        context.Movement.Stop();
        Debug.Log(
            $"{enemy.name}:帰還地点到着。探索状態へ戻ります"
        );

        enemy.StateMachine.SwitchState<EnemyExploreState>();
    }
    public override void Exit()
    {
        context.Movement.Stop();
    }

    private void TryStartReturning()
    {
        if(!context.Movement.TryGetReachablePos(
                enemy.SpawnPos,
                ReturnSampleDistance,
                out returnPos
        ))
        {
            Debug.LogWarning($"{enemy.name}:帰還地点への有効な経路を取得できませんでした。");
            return;
        }

        hasReturnPos = true;
        context.Movement.MoveTo(returnPos);

        Debug.Log($"{enemy.name}:帰還地点{returnPos}に向かいます");
    }

    private void UpdateRetryingReturnPath()
    {
        retryTimer -= Time.deltaTime;

        if(retryTimer > 0f)
        {
            return;
        }
        //インターバルを過ぎればタイマーリセット、再試行
        retryTimer = RetryInterval;
        TryStartReturning();
    }
}
