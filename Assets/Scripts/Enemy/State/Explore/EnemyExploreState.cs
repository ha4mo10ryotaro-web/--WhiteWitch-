using UnityEngine;

public class EnemyExploreState : EnemyStateBase
{
    private readonly IEnemyExploreContext context;

    //本来は敵Stats内で作られるステータス。仮置きでつくる。
        private readonly float exploreRadius = 8.0f;
        //探索状態での待機時間の最小値と最大値
            private readonly float waitDurationMin = 1.0f;
            private readonly float waitDurationMax = 3.0f;


    //探索範囲の基準となる点（現在地）
    private Vector3 explorePointCenter;

    private float waitTimer = 0f;
    private bool isWaiting = false;


    //インスタンス化時に呼ばれる。中央指令室と探索状態の責任者を登録する
    public EnemyExploreState(
        EnemyController enemy,
        IEnemyExploreContext context)
        : base(enemy)
    {
        this.context = context;
    }

    public override void Enter()
    {
        Debug.Log("探索状態に入りました");

        explorePointCenter = enemy.transform.position;
        isWaiting = false;
        waitTimer = 0f;

        //探索開始時に目的地を決定する
        SelectNextDestination();

    }

    public override void Update()
    {
        /*検知スクリプト君がプレイヤーを見つけたら(CanSeePlayer(仮))追跡状態に切り替える
        if (context.Perception.CanSeePlayer)
        {
            enemy.StateMachine.SwitchState<EnemyChaseState>();
            return;
        }
        */
        

        if(isWaiting)
        {
            UpdateWaiting();
            return;
        }

        if(context.Movement.HasArrived)
        {
            BeginWaiting();
        }
        
    }

    public override void Exit()
    {
        context.Movement.Stop();

        isWaiting = false;
        waitTimer = 0f;
    }

    private void BeginWaiting()
    {
        context.Movement.Stop();

        waitTimer = Random.Range(
            waitDurationMin,
            waitDurationMax
        );

        isWaiting = true;
    }

    private void UpdateWaiting()
    {
        waitTimer -= Time.deltaTime;

        if(waitTimer <= 0f)
        {
            return;
        }

        isWaiting = false;
        SelectNextDestination();
    }

    private void SelectNextDestination()
    {
        //探索範囲内のランダムな目的地を決定する
        if(!context.Movement.MoveToRandomPoint(
            explorePointCenter,
            exploreRadius,
            out Vector3 nextDestination))
        {
            Debug.LogWarning(
                $"{enemy.name} : 探索状態で目的地の設定に失敗しました。"
            );
            
            //待機状態に入る
            BeginWaiting();
            return;

        }

        if(!context.Movement.MoveTo(nextDestination))
        {
            Debug.LogWarning(
                $"{enemy.name} : 探索状態で目的地の移動に失敗しました。"
            );

            //待機状態に入る
            BeginWaiting();
            return;
        }
    }


}

