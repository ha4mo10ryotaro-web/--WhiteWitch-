using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;

    public bool IsInAttackRange(Transform target)
    {
      if(target == null)
        {
            return false;
        }

        Vector3 toTarget =
            target.position - transform.position;
            
        float rangeSquared = attackRange * attackRange;

        return toTarget.sqrMagnitude <= rangeSquared;
    }

}
