using UnityEngine;
using System.Collections.Generic;

public struct DamageResult
{
    public float BaseDamage;
    public float STRSum;
    public float DEXSum;
    public float INTSum;
    public float TotalProficiency;
    public float TieredDamage;
    public float AfterModifiers;
    public float AfterMitigation;
    public bool IsCrit;
    public float FinalDamage;
    public float CritRoll;
}

public static class DamageService
{
    public static DamageResult Calculate(
        Dictionary<StatType, float> classProfs,
        Dictionary<StatType, float> weaponProfs,
        float baseDamage,
        float tierMultiplier,
        float buffs = 0f,
        float debuffs = 0f,
        float passiveBonuses = 0f,
        float targetMitigation = 0f,
        float critChance = -1f,
        float critMultiplier = 0.5f,
        bool rollCrit = true
    )
    {
        targetMitigation = Mathf.Clamp01(targetMitigation);

        float dynamicMultiplier = 1f + buffs - debuffs + passiveBonuses;

        var mainStats = new[] { StatType.Strength, StatType.Dexterity, StatType.Intelligence };
        var statSums = new Dictionary<StatType, float>();

        foreach (var type in mainStats)
            statSums[type] = GetStatProduct(classProfs, weaponProfs, type);

        float totalProficiency = 0f;
        foreach (var kvp in statSums)
            totalProficiency += kvp.Value;

        float tieredDamage = baseDamage * totalProficiency * tierMultiplier;
        float afterModifiers = tieredDamage * dynamicMultiplier;
        float afterMitigation = afterModifiers * (1f - targetMitigation);

        float roll = 0f;
        bool isCrit = false;
        float finalDamage = afterMitigation;

        if (critChance > 0f)
        {
            roll = rollCrit ? Random.value : critChance;
            isCrit = roll <= critChance;
            if (isCrit) finalDamage *= (1f + critMultiplier);
        }

        finalDamage = Mathf.Max(1f, finalDamage);

        // --- Debug Logs détaillés ---
        Debug.Log(
            $"--- DamageService Debug ---\n" +
            $"BaseDamage={baseDamage}, TierMultiplier={tierMultiplier}\n" +
            $"ClassProfs: STR={GetVal(classProfs, StatType.Strength)}, DEX={GetVal(classProfs, StatType.Dexterity)}, INT={GetVal(classProfs, StatType.Intelligence)}\n" +
            $"WeaponProfs: STR={GetVal(weaponProfs, StatType.Strength)}, DEX={GetVal(weaponProfs, StatType.Dexterity)}, INT={GetVal(weaponProfs, StatType.Intelligence)}\n" +
            $"STRSum={statSums[StatType.Strength]}, DEXSum={statSums[StatType.Dexterity]}, INTSum={statSums[StatType.Intelligence]}\n" +
            $"TotalProficiency={totalProficiency}\n" +
            $"TieredDamage={tieredDamage}, AfterModifiers={afterModifiers}, AfterMitigation={afterMitigation}\n" +
            $"DynamicMultiplier={dynamicMultiplier}, TargetMitigation={targetMitigation}\n" +
            $"IsCrit={isCrit}, CritRoll={roll}, FinalDamage={finalDamage}\n" +
            $"Buffs={buffs}, Debuffs={debuffs}, Passives={passiveBonuses}"
        );

        return new DamageResult
        {
            BaseDamage = baseDamage,
            STRSum = statSums[StatType.Strength],
            DEXSum = statSums[StatType.Dexterity],
            INTSum = statSums[StatType.Intelligence],
            TotalProficiency = totalProficiency,
            TieredDamage = tieredDamage,
            AfterModifiers = afterModifiers,
            AfterMitigation = afterMitigation,
            IsCrit = isCrit,
            FinalDamage = finalDamage,
            CritRoll = roll
        };
    }

    private static float GetStatProduct(Dictionary<StatType, float> classProfs, Dictionary<StatType, float> weaponProfs, StatType type)
    {
        float classVal = classProfs.ContainsKey(type) ? Mathf.Clamp01(classProfs[type]) : 0f;
        float weaponVal = weaponProfs.ContainsKey(type) ? Mathf.Clamp01(weaponProfs[type]) : 0f;
        return classVal * weaponVal;
    }

    private static float GetVal(Dictionary<StatType, float> dict, StatType type)
    {
        return dict != null && dict.ContainsKey(type) ? dict[type] : 0f;
    }
}
