using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Boss Data")]
public class BossData : EnemyData
{
    [Header("Boss Specifics")]
    [Range(0f, 1f)] public float phase2Threshold = 0.5f;
    [Range(0f, 1f)] public float phase3Threshold = 0.2f;

    [Tooltip("Chance using a special skill - phase 2")]
    [Range(0f, 1f)] public float phase2SpecialChance = 0.5f;

    [Tooltip("Chance using a special skill - phase 3")]
    [Range(0f, 1f)] public float phase3SpecialChance = 0.7f;

    [Tooltip("Chance using ultimate if available")]
    [Range(0f, 1f)] public float ultimateUsageChance = 0.8f;
}
