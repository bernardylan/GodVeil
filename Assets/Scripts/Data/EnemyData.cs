using UnityEngine;

public enum EnemyType
{
    Minion,
    Elite,
    Boss
}

[CreateAssetMenu(menuName = "GodVeil/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Info")]
    public string enemyNameKey;
    public Sprite icon;
    public GameObject prefab;

    [Header("Type")]
    public EnemyType enemyType;

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
    [Range(0f, 1f)] public float ultimateUsageThreshold = 1f;
    [Range(0f, 1f)] public float specialUsageChance = 0.3f;
}