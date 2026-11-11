using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    private enum ChasePhase
    {
        Chasing,
        StayingBorder,
        StayingLastPos
    }
    private ChasePhase currentPhase = ChasePhase.Chasing;
    //仮調整用ステータス。敵StatsDataに入れるべきもの
        private readonly float chaseRadius = 15f;
        //境界線の場所をNavMesh上でサンプリングする範囲。
        //半径内のNavMesh上に有効な位置がない場合は、境界線の場所を決定できない
        private readonly float borderSampleDistance = 2f;
        private readonly float borderWaitingTime = 3f;
        //追跡が外れてから、最後に見た位置で待機する時間
        private readonly float searchWaitingTime = 5f;

    private Vector3 border;
    private Vector3 lastSeenPosition;
    private float borderWaitingTimer = 0f;
    private float searchWaitingTimer = 0f;
    private bool hasReachedBorder = false;
    private bool hasReachedLastSeenPosition = false;

    private IEnemyChaseContext context;
    public EnemyChaseState(
        EnemyController enemy,
        IEnemyChaseContext context) : base(enemy)
    {
        this.context = context;
    }


    //override……親クラス側でabstractで丸投げされてた中身の処理を上書きするよ！
    public override void Enter()
    {
            Debug.Log("追跡状態に入りました");

            currentPhase = ChasePhase.Chasing;

            borderWaitingTimer = 0f;
            searchWaitingTimer = 0f;

            hasReachedBorder = false;
            hasReachedLastSeenPosition = false;

            //最後に敵を視認した位置の初期化。前見た位置に引っ張られないように。
            if(enemy.Target != null)
            {
                lastSeenPosition = enemy.Target.position; 
            }
    }


    public override void Update()
    {
        if(enemy.Target == null)
        {
            SwitchToReturn();
            return;
        }

        //追跡状態ごとにUpdate処理が分かれる
        switch(currentPhase)
        {
            case ChasePhase.Chasing:
                UpdateChasing();
                break;
            case ChasePhase.StayingBorder:
                UpdateStayingBorder();
                break;
            case ChasePhase.StayingLastPos:
                UpdateStayingLastPos();
                break;
        }
    }

    public override void Exit()
    {
        context.Movement.Stop();

        borderWaitingTimer = 0f;
        searchWaitingTimer = 0f;

        hasReachedBorder = false;
        hasReachedLastSeenPosition = false;
    }



    private void EnterChasingPhase()
    {
        currentPhase = ChasePhase.Chasing;

        hasReachedBorder = false;
        hasReachedLastSeenPosition = false;

        borderWaitingTimer = 0f;
        searchWaitingTimer = 0f;

        lastSeenPosition =
            enemy.Target.position;

        context.Movement.MoveTo(
            enemy.Target.position
        );

        Debug.Log("通常追跡を再開しました");
    }
    private void UpdateChasing()
    {
        bool canSeeTarget = 
            context.Perception.CanSee(enemy.Target);

        //敵が視認できなければ、最後に見た位置に向かうフェーズに移行する
        if(!canSeeTarget)
        {
            EnterStayingLastPosPhase();
            return;
        }

        lastSeenPosition =
            enemy.Target.position;

        /*Combat実装後に解放
        if(context.Combat.IsInAttackRange(enemy.Target))
        {
            enemy.StateMachine.SwitchState<EnemyAttackState>();

            return;
        }
        */

        //追跡対象が追跡半径外に出た場合、追跡をやめる
        if(!IsTargetInChaseRadius())
        {
            EnterStayingBorderPhase();
            return;
        }

        context.Movement.MoveTo(
            enemy.Target.position
        );

    }

    private void EnterStayingBorderPhase()
    {
        currentPhase = ChasePhase.StayingBorder;

        hasReachedBorder = false;
        borderWaitingTimer = 0f;

        //追跡対象が追跡半径外に出た場合、追跡境界地点に向かう
        if(!TryGetBorderPoint(out border))
        {
            Debug.LogError($"{enemy.name}: 追跡境界地点の取得に失敗しました");
            SwitchToReturn();
            return;
        }

        //追跡境界地点に向かう
        if(!context.Movement.MoveTo(border))
        {
            Debug.LogError($"{enemy.name}: 追跡境界地点への移動に失敗しました");
            SwitchToReturn();
            return;
        }

        Debug.Log($"{enemy.name}: " + 
            $"追跡対象が追跡半径外に出たため、" +
            $"追跡境界地点{border}に向かいます");

    }
    private void UpdateStayingBorder()
    {
        //追跡対象が追跡半径内に戻り、かつ視認できれば追跡続行
        if(IsTargetInChaseRadius() &&
            context.Perception.CanSee(enemy.Target))
        {
            EnterChasingPhase();
            return;
        }

        //境界線上に到着するまでタイマーは動かない
        if(!hasReachedBorder)
        {
            if(!context.Movement.HasArrived)
            {
                return;
            }
            
            //境界線上に到着した
            context.Movement.Stop();

            hasReachedBorder = true;
            borderWaitingTimer = borderWaitingTime;

            Debug.Log($"{enemy.name}: 追跡境界地点に到着しました。{borderWaitingTime}秒間待機します");
            return;
        }

        //境界線上に到着してからの待機処理
        borderWaitingTimer -= Time.deltaTime;

        if(borderWaitingTimer > 0f)
        {
            return;
        }   

        Debug.Log($"{enemy.name}: 待機が終了。追跡を終了し、ナワバリに帰還します");
        SwitchToReturn();

    }


    private void EnterStayingLastPosPhase()
    {
        currentPhase = ChasePhase.StayingLastPos;

        hasReachedLastSeenPosition = false;
        searchWaitingTimer = 0f;

        //最後に見た位置に向かう
        if(!context.Movement.MoveTo(
            lastSeenPosition
        ))
        {
            Debug.LogError($"{enemy.name}: 最後に見た位置への移動に失敗しました");
            return;
        }

        Debug.Log($"{enemy.name}: " + 
            $"追跡対象を見失ったため、最後に見た位置{lastSeenPosition}に向かいます");
    }
    private void UpdateStayingLastPos()
    {
        
    }


    //追跡対象はナワバリ内にいるか？
    private bool IsTargetInChaseRadius()
    {
        Vector3 spawnPos2Target = 
            enemy.Target.position -
            enemy.SpawnPos;

        //高さは無視する
        spawnPos2Target.y = 0f;

        float chaseRadiusSquared =
            chaseRadius * chaseRadius;

        return spawnPos2Target.sqrMagnitude <=
            chaseRadiusSquared;   
    }
    //
    private bool TryGetBorderPoint(out Vector3 point)
    {
        Vector3 spawnPos2Target = 
            enemy.Target.position -
            enemy.SpawnPos;

        //高さは無視する
        spawnPos2Target.y = 0f;

        //0ベクトルの正規化はできないので、0ベクトルだった場合はfalseを返す
        if(spawnPos2Target.magnitude <= Mathf.Epsilon)
        {
            point = Vector3.zero;
            return false;
        }

        Vector3 dir2Target =
            spawnPos2Target.normalized;

        //追跡対象の方向に、追跡半径の距離だけ伸ばした位置（境界点の候補地）を取得する
        Vector3 borderTemp = 
            enemy.SpawnPos +
            dir2Target * chaseRadius;

        //候補地をNavMesh上の有効な位置に変換する
        return context.Movement.TryGetNavMeshPoint(
            borderTemp,
            borderSampleDistance,
            out point
        );
    }

    private void SwitchToReturn()
    {
        context.Movement.Stop();

        enemy.StateMachine
            .SwitchState<EnemyReturnState>();
    }
}
