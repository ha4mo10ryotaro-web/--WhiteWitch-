using UnityEditor.Rendering;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyDinamicStats : MonoBehaviour
{
    private EnemyController enemy;

    public float BaseDefence => enemy.Stats.Defence;
    public float CurrentDefence {get; private set;}
    private float defenceMultiplier;

    public float BaseImpactResistance => enemy.Stats.ImpactResistance;
    public float CurrentImpactResistance {get; private set;}
    private float impactResistanceMultiplier;


    private void Awake()
    {
        enemy = GetComponent<EnemyController>();

        if(enemy.Stats == null)
        {
            Debug.LogError($"{enemy.name}:ステータスデータが設定されていません");
            enabled = false;
            return;
        }

        InitStats();
    }

    private void InitStats()
    {
        CurrentDefence = enemy.Stats.Defence;
        defenceMultiplier = 0f;
        CurrentImpactResistance = enemy.Stats.ImpactResistance;
    }

    //現在からの変化量だけ渡してもらう
    public void AddDefence(float addValue = 0f, float addMultiplier = 0f)
    {
        defenceMultiplier += addMultiplier;
        CurrentDefence = Mathf.Max(0f,
             (CurrentDefence + addValue) * (1f + addMultiplier));
        
    }

    public void AddImpactResistance(float addValue = 0f, float multiplier = 0f)
    {
        CurrentImpactResistance = Mathf.Max(0f, 
            (CurrentDefence + addValue) * (1f + multiplier));
        
    }
}
