using UnityEngine;

public sealed class EnemyPerception : MonoBehaviour
{
    [Header("視点")]
    [SerializeField]
    private Transform eyePoint;

    [Header("検知範囲")]
    [SerializeField, Min(0f)]
    private float detectionRange = 12f;

    [SerializeField, Range(0f, 360f)]
    private float horizontalViewAngle = 120f;

    [SerializeField, Range(0f, 180f)]
    private float verticalViewAngle = 60f;

    [Header("遮蔽判定")]
    [SerializeField]
    private LayerMask sightMask;

    //自分から攻撃対象が見えてますか？
    public bool CanSee(Transform target)
    {
        if (target == null || eyePoint == null)
        {
            return false;
        }

        Vector3 toTarget =
            target.position - eyePoint.position;

        float TargetDistance = toTarget.sqrMagnitude;
        float rangeSquared = detectionRange * detectionRange;

        // 1. 距離判定
        if (TargetDistance > rangeSquared)
        {
            return false;
        }

        float distance = Mathf.Sqrt(TargetDistance);

        if (distance <= Mathf.Epsilon)
        {
            return true;
        }

        // 2. 水平視野角
        Vector3 horizontalForward =
            Vector3.ProjectOnPlane(
                transform.forward,
                Vector3.up
            ).normalized;

        Vector3 horizontalDirection =
            Vector3.ProjectOnPlane(
                toTarget,
                Vector3.up
            ).normalized;

        float horizontalLimit = Mathf.Cos(
            horizontalViewAngle
            * 0.5f
            * Mathf.Deg2Rad
        );

        if (Vector3.Dot(
            horizontalForward,
            horizontalDirection
        ) < horizontalLimit)
        {
            return false;
        }

        // 3. 垂直視野角
        float horizontalDistance = new Vector2(
            toTarget.x,
            toTarget.z
        ).magnitude;

        float verticalAngle = Mathf.Abs(
            Mathf.Atan2(
                toTarget.y,
                horizontalDistance
            ) * Mathf.Rad2Deg
        );

        if (verticalAngle > verticalViewAngle * 0.5f)
        {
            return false;
        }

        // 4. 遮蔽判定
        if (!Physics.Raycast(
            eyePoint.position,
            toTarget / distance,
            out RaycastHit hit,
            distance,
            sightMask,
            QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return hit.transform == target
            || hit.transform.IsChildOf(target);
    }
}