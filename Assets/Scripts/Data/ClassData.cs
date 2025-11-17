using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Class Data")]
public class ClassData : ScriptableObject
{
    [Header("Base Info")]
    public string className;
    public Sprite classIcon;
    public GameObject classPrefab;
    public TierType tier;
    public bool isLocked = false;

    [Header("Stat Requirements")]
    [Tooltip("Stats needed for unlock a class")]
    public StatRequirement[] statRequirements;
    public bool MeetsRequirements(CharacterStats character)
    {
        if (statRequirements == null || statRequirements.Length == 0)
            return true;

        // 🔥 IMPORTANT :
        // Force la synchro runtime si jamais le profil a changé
        character.RecalculateDerivedStats();

        foreach (var req in statRequirements)
        {
            float currentValue = character.GetProficiency(req.stat); // prend maintenant le profil joueur

            if (currentValue < req.minimum)
                return false;
        }

        return true;
    }

    private void OnValidate()
    {
        if (statRequirements == null) return;

        for (int i = 0; i < statRequirements.Length; i++)
        {
            var req = statRequirements[i];
            req.minimum = Mathf.Round(req.minimum * 10f) / 10f; // Snap 
            statRequirements[i] = req;
        }
    }

    [Header("Base Stats")]
    public float baseHP = 500f;
    public float baseDefense = 0.05f;
    public float baseCritRate = 0.05f;
    public float baseDodge = 0.05f;
    public float baseHitChance = 0.85f;
    public float baseEnergyRegen = 1f;
    public float baseSpeed = 1f;

    [Header("Proficiencies")]
    [Tooltip("Main stats: STR / DEX / INT / SPEED")]
    public StatProfile proficiencies;

    [Header("Skills & Passive")]
    public SkillData autoAttack;
    public SkillData specialSkill;
    public SkillData ultimateSkill;
    public PassiveData passive;
}

public enum StatType 
{
    Strength,   // Phys dmg + HP/Def via End scaling
    Dexterity,  // Crit rate + Dodge
    Intelligence, // Magic dmg + Hit chance + Focus regen
    Speed      // Action speed (ATB/Turn speed)
}

[System.Serializable]
public struct DerivedStats
{
    public float BaseHP;
    public float MaxHP;
    public float Defense;
    public float CritRate;
    public float Dodge;
    public float HitChance;
    public float EnergyRegen;
    public float Speed;
}

public enum SkillType { AutoAttack, Special, Ultimate }
public enum ElementType { None, Fire, Water, Earth, Lightning, Poison }

public enum TierType { T0, T1, T2, T3 }
public static class TierUtility
{
    public static float GetMultiplier(TierType tier) => tier switch
    {
        TierType.T0 => 1f,
        TierType.T1 => 1.25f,
        TierType.T2 => 1.75f,
        TierType.T3 => 2.25f,
        _ => 1f
    };
}