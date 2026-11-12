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
    //帰還経路、ある？
    private bool hasReturnPath = false;

    //再試行
    private const float RetryInterval = 1.0f;
    private float retryTimer = 0f;
    //帰還不能判定
    private const float ReturnFailureTimeout = 10.0f;//10秒はプロトタイプ版の仮値とする
    private float returnFailureTimer = 0f;

    //ログ管理用フラグ
    private bool hasLoggedReturnFailure;
    private bool hasReachedFailureTimeout;

    public override void Enter()
    {
        hasReturnPath = false;
        //初回は再試行インターバル無し
        retryTimer = 0f;

        returnFailureTimer = 0f;
        hasLoggedReturnFailure = false;
        hasReachedFailureTimeout = false;

        context.Movement.Stop ();

        Debug.Log("帰還状態に入りました");

        TryStartReturning();
    }


    public override void Update()
    {
        //経路が見当たらなければ再試行、到着すれば帰還完了
        if(!hasReturnPath)
        {
            UpdateRetryingReturnPath();
            return;
        }
        if(!context.Movement.HasArrived)
        {
            return;
        }

        CompleteReturn();

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
            hasReturnPath = false;

            //再試行の一連の流れの中で、警告文を出すのは最初だけ
            if(!hasLoggedReturnFailure)
            {
                hasLoggedReturnFailure = true;

                Debug.LogWarning(
                    $"{enemy.name}:帰還地点への有効な経路を取得できませんでした。"+
                     "経路取得の再試行を開始します");
            }

            return;
        }

        //経路はある。が、移動できない
        if(!context.Movement.MoveTo(returnPos))
        {
            hasReturnPath = false;

            if(!hasLoggedReturnFailure)
            {
                hasLoggedReturnFailure = true;

                Debug.LogWarning(
                    $"{enemy.name}: 帰還地点への移動を開始できません。"+
                    "経路の再試行を開始します");

            }

            return;
        }


    //経路があって移動もできる
        //元々帰還不能に陥っていたか
        bool wasUnableToReturn = returnFailureTimer > 0f;
        //諸々リセット
        hasReturnPath = true;
        retryTimer = 0f;
        returnFailureTimer = 0f;
        hasLoggedReturnFailure = false;
        hasReachedFailureTimeout = false;

        if(wasUnableToReturn)
        {
            Debug.Log(
                $"{enemy.name}:帰還経路を再取得しました。"+
                $"帰還地点{returnPos}へ向かいます");

            return;
        }

        Debug.Log($"{enemy.name}:帰還地点{returnPos}へ向かいます");

    }

    private void UpdateRetryingReturnPath()
    {
        retryTimer -= Time.deltaTime;
        returnFailureTimer += Time.deltaTime;

        //帰還不能状態が続いたらタイムアウト
        if(!hasReachedFailureTimeout && returnFailureTimer >= ReturnFailureTimeout)
        {
            hasReachedFailureTimeout = true;
            Debug.LogWarning(
                $"{enemy.name}:帰還不能状態が{ReturnFailureTimeout}秒以上継続しました" +
                "Despawn候補として扱います"
            );
        }


        if(retryTimer > 0f)
        {
            return;
        }
        //インターバルを過ぎればタイマーリセット、再試行
        retryTimer = RetryInterval;
        TryStartReturning();
    }

    private void CompleteReturn()
    {
        context.Movement.Stop();
        Debug.Log(
            $"{enemy.name}:帰還地点到着。探索状態へ戻ります"
        );

        enemy.StateMachine.SwitchState<EnemyExploreState>();
    }

}
