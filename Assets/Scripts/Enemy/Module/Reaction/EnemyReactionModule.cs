using UnityEngine;
using UnityEngine.Rendering.LookDev;

//反応部門の責任者。ノックバックやスタンなどの反応を管理する
public sealed class EnemyReactionModule : MonoBehaviour
{
    public EnemyStagger Stagger{get; private set;}
    public EnemyKnockback Knockback {get; private set;}
    public EnemyStun Stun {get; private set;}


    public bool IsBlockingState =>
        (Stagger != null && Stagger.IsStaggered) ||
        (Knockback != null && Knockback.IsKnocked) ||
        (Stun != null && Stun.IsStunned);

    private void Awake()
    {
        Stagger = GetComponent<EnemyStagger>();
        Knockback = GetComponent<EnemyKnockback>();
        Stun = GetComponent<EnemyStun>();
    }
}