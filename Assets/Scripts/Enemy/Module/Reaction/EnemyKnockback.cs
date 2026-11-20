using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public bool IsKnocked { get; private set; }
    private EnemyMovement movement;

    private float remainingDistance;
    private Vector3 direction;
    //仮値。等速直線運動じゃなくしたい。
    private float speed = 2.0f;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        IsKnocked = false;
    }

    public void BeginKnockback(
        Vector3 Direction,
        float Distance
    )
    {
        if(Distance <= 0f)
        {
            return;
        }

        Direction.y = 0f;
        if(Direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Debug.Log($"{name}:ノックバック開始。{Distance}");
        direction = Direction.normalized;

        remainingDistance = Distance;

        IsKnocked = true;

        movement.Stop();
    }

    private void Update()
    {
        if(!IsKnocked)
        {
            return;
        }

        float moveDistance =
            Mathf.Min(
                speed * Time.deltaTime,
                remainingDistance
            );

        Vector3 force = direction * moveDistance;
        movement.ForcedMove(force);

        remainingDistance -=moveDistance;

        if(remainingDistance <= 0f)
        {
            EndKnocback();
        }
    }

    private void EndKnocback()
    {
        IsKnocked = false;
        remainingDistance = 0f;
    }
}
