using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public bool IsKnocked { get; private set; }
    private EnemyMovement movement;

    private float remainingDistance;
    private Vector3 direction;
    private float speed;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        IsKnocked = false;
    }

    public void BeginKnockback(
        Vector3 Direction,
        float Distance,
        float Duration
    )
    {
        if(Distance <= 0f || Duration <= 0f)
        {
            return;
        }

        Direction.y = 0f;
        if(Direction.sqrMagnitude <= 0.001f)
        {
            return;
        }
        direction = Direction.normalized;

        remainingDistance = Distance;

        speed = Distance / Duration;

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
