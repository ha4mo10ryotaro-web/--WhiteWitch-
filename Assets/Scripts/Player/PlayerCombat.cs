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

    public PlayerAttackDefinition DefaultAttack => defaultAttack;

    public PlayerAttackDefinition CurrentAttack
    {
        get;
        private set;
    }

    public AttackPhase CurrentPhase
    {
        get;
        private set;
    } = AttackPhase.None;

    //持続中かどうか
    public bool IsAttacking {get; private set;}

    private void Awake()
    {
        DisableHitbox();
    }

    /// <summary>
    /// 暫定的に通常攻撃1段目を準備する。
    /// 将来はコンボ段階からDefinitionを選択する。
    /// </summary>
    public bool PrepareDefaultAttack()
    {
        return PrepareAttack(defaultAttack);
    }

    /// <summary>
    /// 今回実行する攻撃を確定する。
    /// この時点ではHitboxを有効にしない。
    /// </summary>
    public bool PrepareAttack(PlayerAttackDefinition attack)
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
        CurrentPhase = AttackPhase.Startup;

        return true;
    }

    /// <summary>
    /// Active開始。
    /// </summary>
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
            return false;
        }

        IsAttacking = true;
        CurrentPhase = AttackPhase.Active;

        hitTargets.Clear();
        EnableHitbox();

        return true;
    }

    /// <summary>
    /// Active終了、Recovery開始。
    /// CurrentAttackはRecovery終了まで保持する。
    /// </summary>
    public void EndAttack()
    {
        IsAttacking = false;
        CurrentPhase = AttackPhase.Recovery;

        DisableHitbox();
        hitTargets.Clear();
    }

    /// <summary>
    /// Recoveryまで正常に完了した。
    /// </summary>
    public void FinishAttack()
    {
        ResetCombatState();
    }

    /// <summary>
    /// 被弾や死亡などによる強制中断。
    /// 現在状態に関係なく安全状態へ戻す。
    /// </summary>
    public void CancelCurrentAction()
    {
        ResetCombatState();
    }

    public void SetAttackPhase(AttackPhase phase)
    {
        CurrentPhase = phase;
    }

    public void OnHit(Collider other)
    {
        if (!IsAttacking)
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

        IEnemyDamageReceiver receiver =
            other.GetComponentInParent<IEnemyDamageReceiver>();

        if (receiver == null)
        {
            return;
        }

        // falseなら、この攻撃中に同じ対象へ命中済み。
        if (!hitTargets.Add(receiver))
        {
            return;
        }

        EnemyHitData hitData = CreateHitData(other);

        receiver.ReceiveHit(hitData);

        Debug.Log(
            $"{name}: 攻撃が{other.name}にHit!",
            this);
    }

    private EnemyHitData CreateHitData(Collider targetCollider)
    {
        Vector3 hitPosition =
            attackHitbox != null
                ? targetCollider.ClosestPoint(
                    attackHitbox.bounds.center)
                : targetCollider.ClosestPoint(transform.position);

        Vector3 direction =
            targetCollider.transform.position - transform.position;

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
        if (attackHitbox != null)
        {
            attackHitbox.enabled = true;
        }
    }

    private void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.enabled = false;
        }
    }
}