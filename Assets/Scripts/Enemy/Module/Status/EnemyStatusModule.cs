using UnityEngine;
public sealed class EnemyStatusModule : MonoBehaviour
{
    private EnemyController enemy;

    public EnemyHealth Health { get; private set; }
    public EnemyMixedColorStatus MixedColor { get; private set; }
    public EnemySuperResolution SuperResolution { get; private set; }

    public bool IsSuperResolved =>
        SuperResolution != null &&
        SuperResolution.IsActive;

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

    private void Awake()
    {
        enemy =
            GetComponent<EnemyController>();

        Health =
            GetComponent<EnemyHealth>();

        MixedColor =
            GetComponent<EnemyMixedColorStatus>();

        SuperResolution =
            GetComponent<EnemySuperResolution>();
    }
}