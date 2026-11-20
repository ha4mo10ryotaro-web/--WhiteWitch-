using UnityEngine;

[DisallowMultipleComponent]
public class EnemyDynamicStats : MonoBehaviour
{
    private EnemyStatusModule status;
    private bool isInitialized = false;

    public float BaseDefence => status.Stats.Defence;
    public float CurrentDefence {get; private set;}
    private float defenceMultiplier;

    public float BaseImpactResistance => status.Stats.ImpactResistance;
    public float CurrentImpactResistance {get; private set;}
    private float impactResistanceMultiplier;
    
    public float BaseMoveSpeed => status.Stats.MoveSpeed;
    public float CurrentMoveSpeed {get; private set;}
    private float moveSpeedMultiplier;

    public void Initialize(EnemyStatusModule statusModule)
    {
        if(isInitialized)
        {
            return;
        }

        if(statusModule == null)
        {
            Debug.LogError($"{name}ステータスモジュールがありません");
            enabled = false;
            return;
        }

        status = statusModule;

        if(statusModule == null)
        {
            Debug.LogError($"{name}:ステータスデータが設定されていません");
            enabled = false;
            return;
        }

        InitStats();
        isInitialized = true;
    }

    private void InitStats()
    {
        CurrentDefence = status.Stats.Defence;
        defenceMultiplier = 0f;

        CurrentImpactResistance = status.Stats.ImpactResistance;
        impactResistanceMultiplier = 0f;

        CurrentMoveSpeed = status.Stats.MoveSpeed;
        moveSpeedMultiplier = 0f;
    }

    //現在からの変化量だけ渡してもらう
    public void ChangeDefence(float addValue = 0f, float multiplierValue = 0f)
    {
        defenceMultiplier += multiplierValue;
        CurrentDefence = Mathf.Max(0f,
             (CurrentDefence + addValue) * (1f + defenceMultiplier));
        
    }

    public void AddImpactResistance(float addValue = 0f, float multiplierValue = 0f)
    {
        CurrentImpactResistance = Mathf.Max(0f, 
            (CurrentDefence + addValue) * (1f + multiplierValue));
        
    }

    public void ChangeMoveSpeed(float addValue = 0f, float multiplierValue = 0f)
    {
        moveSpeedMultiplier += multiplierValue;
        CurrentMoveSpeed = Mathf.Max(0f, 
            (CurrentMoveSpeed + addValue) * (1f + multiplierValue));
        
    }
}
