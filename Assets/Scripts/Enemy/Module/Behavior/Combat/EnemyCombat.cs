using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private Collider attackHitbox;
    //攻撃を与えたオブジェクトのリスト
    private readonly HashSet<GameObject> hitTargets
        = new HashSet<GameObject>();

    [SerializeField]
    private AttackDefinition defaultAttack;
    public AttackDefinition DefaultAttack => defaultAttack;
    public AttackDefinition CurrentAttack{get; private set;}

    public bool IsAttacking {get; private set;}

    private void Awake()
    {
        if(attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }    
    }


    public bool IsInAttackRange(Transform target)
    {
        if(target == null)
        {
            return false;
        }

        Vector3 toTarget =
            target.position - transform.position;
        toTarget.y = 0f;
            
        float rangeSquared = attackRange * attackRange;

        return toTarget.sqrMagnitude <= rangeSquared;
    }

//攻撃開始！
    public bool BeginAttack(AttackDefinition attack)
    {
        if (IsAttacking)
        {
            return false;
        }

        if(attack == null)
        {
            Debug.LogWarning(
                $"{name}:AttackDefinitionが設定されていません");
            return false;
        }

        IsAttacking = true;
        CurrentAttack = attack;

        //攻撃の命中履歴リセット
        hitTargets.Clear();

        if(attackHitbox != null)
        {
            attackHitbox.enabled = true;
        }

        return true;
    }

//攻撃終了
    public void EndAttack()
    {
        if (!IsAttacking)
        {
            return;
        }

        IsAttacking = false;

        if(attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }

        hitTargets.Clear();
        CurrentAttack = null;
    }

//攻撃中断。攻撃中とか関係ない
    public void CancelAttack()
    {
        IsAttacking = false;

        if(attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }

        hitTargets.Clear();
        CurrentAttack = null;
    }

    public void OnHit(Collider other)
    {
        GameObject target = other.gameObject;

        //リスト追加が失敗（重複）＝その攻撃内で命中済み
        if(!hitTargets.Add(target))
        {
            return;
        }
        Debug.Log(
            $"{name}:攻撃が{other.name}にHit!");
    }

}
