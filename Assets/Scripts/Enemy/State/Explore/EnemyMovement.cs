using UnityEngine;
using UnityEngine.AI;

//このスクリプトはNavMeshAgentを前提として要求します
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    // NavMeshAgentが有効な状態で移動中かどうかを返す
    public bool IsMoving =>
        agent.isOnNavMesh && !agent.isStopped && agent.hasPath;

    public bool HasArrived
    {
        get
        {
            if (!agent.isOnNavMesh || agent.isStopped)
                return false;
            if(!agent.hasPath)
                return true;

            // NavMeshAgentの移動先までの残り距離が停止距離以下ならTrue(到着)を返す
            return agent.remainingDistance <= agent.stoppingDistance;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // 目的地設定、ナビ開始（停止状態の解除）
    public bool MoveTo(Vector3 destination)
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning(
                $"{name} : NavMesh上に配置されていないため移動できません。"
            );
            return false;
        }

        //停止状態の終了、NavMeshAgentの移動先をdestinationに決定
        agent.isStopped = false;
        return agent.SetDestination(destination);
    }

    // ナビ終了、移動先リセット
    public void Stop()
    {
        if(!agent.isOnNavMesh)
        {
            Debug.LogWarning(
                $"{name} : NavMesh上に配置されていないため停止できません。"
            );
            return;
        }

        agent.isStopped = true;
        agent.ResetPath();

    }




//目的地探索用。後で探索専用クラスのみ扱えるように分離する。

    public bool MoveToRandomPoint(
        Vector3 center, float radius,
        out Vector3 point)
    {
        //探索試行回数
        const int maxAttempt = 10;
        const float sampleDistance = 2.0f;

        for(int tmp = 0; tmp < maxAttempt; tmp++)
        {

            //中心点から半径内のランダムな点を生成
            Vector2 randomCircle =
                Random.insideUnitCircle * radius;

            Vector3 randomPoint =
                center + new Vector3(
                    randomCircle.x,
                    0f,
                    randomCircle.y
                );

            //NavMesh上の有効な点を取得
            if(NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, sampleDistance, NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
        }

        point = center;
        return false;
    }

}
