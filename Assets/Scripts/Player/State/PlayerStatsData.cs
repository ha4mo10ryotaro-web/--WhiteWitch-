using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerStatsData",
    menuName = "WhiteWitch/Player/Stats Data")]
public sealed class PlayerStatsData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float moveSpeed = 3.5f;

    public float MoveSpeed => moveSpeed;
}