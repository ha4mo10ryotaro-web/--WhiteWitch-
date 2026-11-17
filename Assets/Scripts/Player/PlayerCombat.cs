using UnityEngine;
[DisallowMultipleComponent]
public sealed class PlayerCombat : MonoBehaviour
{
    [Header("仮値")]
    private float tempAttackDuration = 0.35f;

    private float attackElapsedTime;

    public bool IsAttacking{get; private set;}

    public bool IsAttackFinished =>
        IsAttacking && 
        attackElapsedTime >= tempAttackDuration;

    public void BeginAttack()
    {
        //既に攻撃中なら実行しない
        if(IsAttacking)
        {
            return;
        }

        attackElapsedTime = 0f;
        IsAttacking = true;
    }

    //現在はAttackステートではなくCombatが時間を管理する
    public void TickAttack()
    {
        if(IsAttacking)
        {
            return;
        }

        attackElapsedTime += Time.deltaTime;
    }

    public void EndAttack()
    {
        ResetCombatState();
        Debug.Log("Player攻撃終了");
    }

    public void CancelCurrentAction()
    {
        ResetCombatState();
        Debug.Log("Player攻撃中断");
    }

    private void ResetCombatState()
    {
        IsAttacking = false;
        attackElapsedTime = 0f; 
    }
}
