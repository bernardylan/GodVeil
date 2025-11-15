using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[System.Serializable]
public class CharacterStats
{
    public ClassData CurrentClass;
    public WeaponData EquippedWeapon;

    public float CurrentHP;
    public float CurrentEnergy;

    public DerivedStats Derived;

    public float modifiedProficiency; // Valeur dynamique pendant la run

    private RuntimeProficiency[] originalProficiencies;
    public RuntimeProficiency[] RuntimeProficiencies;

    public event Action<CharacterStats> OnStatsChanged;

    // Scaling coefficients
    private const float HPScaling = 0.5f;
    private const float DefenseScaling = 2f;
    private const float CritScaling = 3f;
    private const float DodgeScaling = 3f;
    private const float HitScaling = 0.2f;
    private const float EnergyScaling = 0.5f;

    public CharacterStats(ClassData classData, WeaponData weapon = null)
    {
        CurrentClass = classData;
        EquippedWeapon = weapon;

        // clone des proficiencies
        RuntimeProficiencies = new RuntimeProficiency[classData.proficiencies.stats.Length];
        
        // Clone de backup, jamais modifié
        originalProficiencies = new RuntimeProficiency[classData.proficiencies.stats.Length];

        for (int i = 0; i < RuntimeProficiencies.Length; i++)
        {
            var s = classData.proficiencies.stats[i];

            RuntimeProficiencies[i] = new RuntimeProficiency(s.type, s.proficiency);
            originalProficiencies[i] = new RuntimeProficiency(s.type, s.proficiency);
        }

        RecalculateDerivedStats();
        CurrentHP = Derived.MaxHP;
        CurrentEnergy = Derived.EnergyRegen;
    }

    public void ResetStatsForRun()
    {
        for (int i = 0; i < RuntimeProficiencies.Length; i++)
        {
            RuntimeProficiencies[i].proficiency = originalProficiencies[i].proficiency;
        }
        modifiedProficiency = 0f;
    }

    public void RecalculateDerivedStats()
    {
        var prof = this;

        Derived = new DerivedStats
        {
            MaxHP = CurrentClass.baseHP * HPScaling * (1 + prof.GetProficiency(StatType.Strength)),
            Defense = CurrentClass.baseDefense * DefenseScaling * prof.GetProficiency(StatType.Strength),
            CritRate = CurrentClass.baseCritRate * CritScaling * prof.GetProficiency(StatType.Dexterity),
            Dodge = CurrentClass.baseDodge * DodgeScaling * prof.GetProficiency(StatType.Dexterity),
            HitChance = CurrentClass.baseHitChance * HitScaling * prof.GetProficiency(StatType.Intelligence),
            EnergyRegen = CurrentClass.baseEnergyRegen * EnergyScaling * prof.GetProficiency(StatType.Intelligence),
            Speed = CurrentClass.baseSpeed * prof.GetProficiency(StatType.Speed)
        };
        OnStatsChanged?.Invoke(this);
    }

    public float GetProficiency(StatType type)
    {
        foreach (var r in RuntimeProficiencies)
            if (r.type == type)
                return r.proficiency;

        return 0f;
    }

    // Return a dictionary for all main stats
    public Dictionary<StatType, float> GetClassProficiencies()
    {
        var dict = new Dictionary<StatType, float>();
        foreach (var stat in CurrentClass.proficiencies.stats)
            dict[stat.type] = stat.proficiency;
        return dict;
    }

    public Dictionary<StatType, float> GetWeaponProficiencies()
    {
        var dict = new Dictionary<StatType, float>();
        if (EquippedWeapon == null) return dict;

        foreach (var stat in EquippedWeapon.proficiencies.stats)
            dict[stat.type] = stat.proficiency;

        return dict;
    }
}
