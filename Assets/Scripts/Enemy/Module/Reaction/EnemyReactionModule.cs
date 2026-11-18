using UnityEngine;
using UnityEngine.Rendering.LookDev;

//反応部門の責任者。ノックバックやスタンなどの反応を管理する
public sealed class EnemyReactionModule : MonoBehaviour
{
    public EnemyStagger Stagger{get; private set;}

    public bool IsBlockingState =>
        Stagger != null &&
        Stagger.IsStaggered;

    private void Awake()
    {
        Stagger = GetComponent<EnemyStagger>();
    }
}