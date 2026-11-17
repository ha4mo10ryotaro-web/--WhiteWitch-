using UnityEngine;
using System.Collections.Generic;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private Collider attackHitbox;
    //攻撃を与えたオブジェクトのリスト
    private readonly HashSet<IPlayerDamageReceiver> hitTargets
        = new HashSet<IPlayerDamageReceiver>();

    [SerializeField]
    private EnemyAttackDefinition defaultAttack;
    public EnemyAttackDefinition DefaultAttack => defaultAttack;
    public EnemyAttackDefinition CurrentAttack{get; private set;}
    public AttackPhase CurrentPhase{get; private set;}
        = AttackPhase.None;


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
    public bool BeginAttack(EnemyAttackDefinition attack)
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
        IPlayerDamageReceiver receiver = 
            other.GetComponentInParent<IPlayerDamageReceiver>();

        if(receiver == null)
        {
            return;
        }

        if (CurrentAttack == null)
        {
            Debug.LogWarning(
                $"{name}: CurrentAttackがない状態でHitしました");

            return;
        }

        //リスト追加が失敗（重複）＝その攻撃内で命中済み
        if(!hitTargets.Add(receiver))
        {
            return;
        }

        PlayerHitData hitData = CreateHitData(other);

        receiver.ReceiveHit(hitData);

        Debug.Log(
            $"{name}:攻撃が{other.name}にHit!");
    }

    private PlayerHitData CreateHitData(Collider targetCollider)
{
    Vector3 hitPosition =
        targetCollider.ClosestPoint(attackHitbox.bounds.center);

    Vector3 direction =
        targetCollider.transform.position - transform.position;

    direction.y = 0f;

    if (direction.sqrMagnitude > 0f)
    {
        direction.Normalize();
    }

    return new PlayerHitData(
        CurrentAttack.Damage,
        CurrentAttack.Impact,
        CurrentAttack.Knockback,
        direction,
        hitPosition,
        gameObject,
        CurrentAttack);
}


    public void SetAttackPhase(AttackPhase phase)
    {
        CurrentPhase = phase;
    }
}
