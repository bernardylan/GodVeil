using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public string enemyNameKey;
    public Sprite icon;
    public GameObject prefab;

    [Header("Base Stats")]
    public float maxHP = 100f;
    public float attack = 20f;
    public float defense = 10f;
    public float speed = 1f;
    public float critChance = 0.1f;
    public float critMultiplier = 1.5f;

    [Header("Skills")]
    public SkillData autoAttack;
    public SkillData specialSkill;
    public SkillData ultimateSkill;

    [Header("AI Settings")]
    [Range(0f, 1f)] public float ultimateUsageThreshold = 1f; // Ex: 1f = 100% energy
    [Range(0f, 1f)] public float specialUsageChance = 0.3f;   // Chance of using special skill
}
