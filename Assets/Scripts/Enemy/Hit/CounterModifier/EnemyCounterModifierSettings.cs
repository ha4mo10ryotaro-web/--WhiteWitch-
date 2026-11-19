using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyCounterModifierSet",
    menuName = "WhiteWitch/Enemy/Counter Modifier Set")]
public sealed class EnemyCounterModifierSettings : MonoBehaviour
{
    [field: SerializeField]
    public EnemyCounterModifierData Normal {get; private set;}

    [field: SerializeField]
    public EnemyCounterModifierData Counter {get; private set;}

    [field: SerializeField]
    public EnemyCounterModifierData PunishCounter {get; private set;}

    public EnemyCounterModifierData Get(
        CounterType counterType)
    {
        switch (counterType)
        {
            case CounterType.Counter:
                return Counter;

            case CounterType.PunishCounter:
                return PunishCounter;

            default:
                return Normal;
        }
    }
}
