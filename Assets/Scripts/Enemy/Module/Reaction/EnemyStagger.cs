using UnityEngine;

public class EnemyStagger : MonoBehaviour
{
    public bool IsStaggered { get; private set; }

    private float staggerTimer;

    private EnemyMovement movement;
    private EnemyCombat combat;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        combat = GetComponent<EnemyCombat>();
    }

    public void BeginStagger(float duration)
    {
        IsStaggered = true;
        staggerTimer = Mathf.Max(0f, duration);

        movement.Stop();
        combat.CancelAttack();
    }

    private void Update()
    {
        if(!IsStaggered)
        {
            return;
        }

        staggerTimer -= Time.deltaTime;

        if(staggerTimer <= 0f)
        {
            EndStagger();
        }

    }

        private void EndStagger()
    {
        IsStaggered = false;
        staggerTimer = 0f;
    }
}
