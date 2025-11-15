using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CharacterStats
{
    public ClassData CurrentClass;
    public WeaponData EquippedWeapon;

    public float CurrentHP;
    public float CurrentEnergy;

    public DerivedStats Derived;

    public event Action OnStatsChanged;

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
        RecalculateDerivedStats();
        CurrentHP = Derived.MaxHP;
        CurrentEnergy = Derived.EnergyRegen;
    }

    public void RecalculateDerivedStats()
    {
        var prof = CurrentClass.proficiencies;

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
        OnStatsChanged?.Invoke();
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
