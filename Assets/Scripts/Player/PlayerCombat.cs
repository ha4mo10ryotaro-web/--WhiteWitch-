using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField]
    private PlayerAttackDefinition defaultAttack;

    [SerializeField]
    private Collider attackHitbox;

    private readonly HashSet<IEnemyDamageReceiver> hitTargets
        = new HashSet<IEnemyDamageReceiver>();

    //テスト用デフォ攻撃
    public PlayerAttackDefinition DefaultAttack
        => defaultAttack;

    public PlayerAttackDefinition CurrentAttack { get;  private set;}

    public AttackPhase CurrentPhase {get;private set;} = AttackPhase.None;

    //攻撃判定は有効か？
    public bool IsAttacking { get; private set;}

    private void Awake()
    {
        ResetCombatState();
    }

    //試験的にデフォの攻撃を設定する
    public bool PrepareDefaultAttack()
    {
        return PrepareAttack(defaultAttack);
    }

    //攻撃準備。使う攻撃の定義
    public bool PrepareAttack(
        PlayerAttackDefinition attack)
    {
        if (attack == null)
        {
            Debug.LogWarning(
                $"{name}: PlayerAttackDefinitionが設定されていません。",
                this);

            return false;
        }

        if (CurrentAttack != null)
        {
            Debug.LogWarning(
                $"{name}: すでに別の攻撃が準備されています。",
                this);

            return false;
        }

        CurrentAttack = attack;

        // Startupへの移行はPlayerAttackStateが担当する。
        CurrentPhase = AttackPhase.None;
        IsAttacking = false;

        DisableHitbox();
        hitTargets.Clear();

        return true;
    }

   //前隙開始。前回の攻撃対象の履歴削除
    public bool BeginAttack()
    {
        if (CurrentAttack == null)
        {
            Debug.LogWarning(
                $"{name}: CurrentAttackがない状態で" +
                "BeginAttackが呼ばれました。",
                this);

            return false;
        }

        if (IsAttacking)
        {
            Debug.LogWarning(
                $"{name}: すでにActiveフェーズ中です。",
                this);

            return false;
        }

        IsAttacking = true;
        CurrentPhase = AttackPhase.Active;

        hitTargets.Clear();
        EnableHitbox();

        return true;
    }

    //攻撃終了。後隙開始。
    public void EndAttack()
    {
        IsAttacking = false;
        CurrentPhase = AttackPhase.Recovery;

        DisableHitbox();
        hitTargets.Clear();
    }

    //正常終了
    public void FinishAttack()
    {
        ResetCombatState();
    }

    /// 被弾、死亡、別行動への割り込み等による強制終了
    public void CancelAttack()
    {
        ResetCombatState();
    }

    /// PlayerAttackStateが管理するフェーズを取ってくる
    public void SetAttackPhase(AttackPhase phase)
    {
        CurrentPhase = phase;
    }

    /// PlayerAttackHitboxから接触対象を受け取る。
    public void OnHit(Collider other)
    {
        if (!IsAttacking)
        {
            return;
        }

        if (CurrentPhase != AttackPhase.Active)
        {
            return;
        }

        if (CurrentAttack == null)
        {
            Debug.LogWarning(
                $"{name}: CurrentAttackがない状態でHitしました。",
                this);

            return;
        }

        if (other == null)
        {
            return;
        }

        IEnemyDamageReceiver receiver =
            other.GetComponentInParent<IEnemyDamageReceiver>();

        if (receiver == null)
        {
            return;
        }

        // falseなら、同じ攻撃のActive中に命中済み。
        if (!hitTargets.Add(receiver))
        {
            return;
        }

        EnemyHitData hitData =
            CreateHitData(other);

        receiver.ReceiveHit(hitData);

        Debug.Log(
            $"{name}: 攻撃が{other.name}にHit!",
            this);
    }

    private EnemyHitData CreateHitData(
        Collider targetCollider)
    {
        Vector3 hitOrigin =
            attackHitbox != null
                ? attackHitbox.bounds.center
                : transform.position;

        Vector3 hitPosition =
            targetCollider.ClosestPoint(hitOrigin);

        Vector3 direction =
            targetCollider.transform.position
            - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            direction.Normalize();
        }
        else
        {
            direction = transform.forward;
        }

        return new EnemyHitData(
            CurrentAttack.BaseDamage,
            CurrentAttack.BaseImpact,
            CurrentAttack.BaseKnockback,
            direction,
            hitPosition,
            gameObject,
            CurrentAttack);
    }

    private void ResetCombatState()
    {
        IsAttacking = false;
        CurrentAttack = null;
        CurrentPhase = AttackPhase.None;

        DisableHitbox();
        hitTargets.Clear();
    }

    private void EnableHitbox()
    {
        if (attackHitbox == null)
        {
            Debug.LogWarning(
                $"{name}: AttackHitboxが設定されていません。",
                this);

            return;
        }

        attackHitbox.enabled = true;
    }

    private void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (attackHitbox != null)
        {
            attackHitbox.isTrigger = true;
        }
    }
#endif
}