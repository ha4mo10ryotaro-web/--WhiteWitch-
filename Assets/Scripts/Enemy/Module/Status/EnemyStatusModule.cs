using UnityEngine;
public sealed class EnemyStatusModule : MonoBehaviour
{
    [field: SerializeField]
    public EnemyStatsData Stats {get; private set;}
    public EnemyDynamicStats DynamicStats {get; private set; }

    public EnemyHealth Health { get; private set; }


    public EnemyMixedColorStatus MixedColor { get; private set; }
    public EnemySuperResolution SuperResolution { get; private set; }

    /*
    public bool IsSuperResolved =>
        SuperResolution != null &&
        SuperResolution.IsActive? : false;

    public float CurrentImpactResistance
    {
        get
        {
            if (IsSuperResolved)
            {
                return 0f;
            }

            float value =
                enemy.Stats.ImpactResistance;

            if (MixedColor != null)
            {
                value *=
                    MixedColor.ImpactResistanceMultiplier;
            }

            return Mathf.Max(0f, value);
        }
    }
    */
    private void Awake()
    {
        if(Stats == null)
        {
            Debug.LogError($"{name}:StatsDataが設定されていません");
            enabled = false;
            return;
        }

        DynamicStats = 
            GetComponent<EnemyDynamicStats>();

        DynamicStats.Initialize(this);

        Health =
            GetComponent<EnemyHealth>();

        MixedColor =
            GetComponent<EnemyMixedColorStatus>();

        SuperResolution =
            GetComponent<EnemySuperResolution>();
    }
}