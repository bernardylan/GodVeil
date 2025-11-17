using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class CharacterStats
{
    public ClassData CurrentClass;
    public WeaponData EquippedWeapon;

    public float CurrentHP;
    public float CurrentEnergy;

    public DerivedStats Derived;

    public RuntimeProficiency[] RuntimeProficiencies;

    [NonSerialized] public CharacterClassState classState;

    public event Action<CharacterStats> OnStatsChanged;

    private const float HPScaling = 0.5f;
    private const float DefenseScaling = 2f;
    private const float CritScaling = 3f;
    private const float DodgeScaling = 3f;
    private const float HitScaling = 0.2f;
    private const float EnergyScaling = 0.5f;

    // Flag pour stopper les recalcs récursifs
    private bool isRecalculating = false;

    public CharacterStats(ClassData classData, WeaponData weapon = null)
    {
        CurrentClass = classData;
        EquippedWeapon = weapon;

        InitializeRuntimeProficiencies();
        classState = new CharacterClassState();

        RecalculateDerivedStats();
        CurrentHP = Derived.MaxHP;
        CurrentEnergy = Derived.EnergyRegen;
    }

    private void InitializeRuntimeProficiencies()
    {
        if (CurrentClass?.proficiencies?.stats == null)
        {
            RuntimeProficiencies = new RuntimeProficiency[0];
            return;
        }

        var src = CurrentClass.proficiencies.stats;
        RuntimeProficiencies = new RuntimeProficiency[src.Length];
        for (int i = 0; i < src.Length; i++)
            RuntimeProficiencies[i] = new RuntimeProficiency(src[i].type, src[i].proficiency);
    }

    public void ResetProficienciesForRun()
    {
        if (RuntimeProficiencies == null) InitializeRuntimeProficiencies();

        for (int i = 0; i < RuntimeProficiencies.Length; i++)
            RuntimeProficiencies[i].proficiency = Mathf.Round(RuntimeProficiencies[i].proficiency * 10f) / 10f;
    }

    public float GetProficiency(StatType type)
    {
        if (RuntimeProficiencies != null)
        {
            for (int i = 0; i < RuntimeProficiencies.Length; i++)
                if (RuntimeProficiencies[i].type == type)
                    return Mathf.Clamp01(RuntimeProficiencies[i].proficiency);
        }
        return 0f;
    }

    public void RecalculateDerivedStats()
    {
        if (isRecalculating) return;
        isRecalculating = true;

        float str = GetProficiency(StatType.Strength);
        float dex = GetProficiency(StatType.Dexterity);
        float intel = GetProficiency(StatType.Intelligence);
        float spd = GetProficiency(StatType.Speed);

        Derived = new DerivedStats
        {
            MaxHP = (CurrentClass != null ? CurrentClass.baseHP : 500f) * HPScaling * (1f + str),
            Defense = (CurrentClass != null ? CurrentClass.baseDefense : 0.05f) * DefenseScaling * str,
            CritRate = (CurrentClass != null ? CurrentClass.baseCritRate : 0.05f) * CritScaling * dex,
            Dodge = (CurrentClass != null ? CurrentClass.baseDodge : 0.05f) * DodgeScaling * dex,
            HitChance = (CurrentClass != null ? CurrentClass.baseHitChance : 0.85f) * HitScaling * intel,
            EnergyRegen = (CurrentClass != null ? CurrentClass.baseEnergyRegen : 1f) * EnergyScaling * intel,
            Speed = spd
        };

        OnStatsChanged?.Invoke(this);

        isRecalculating = false;
    }

    public Dictionary<StatType, float> GetClassProficienciesDict()
    {
        var dict = new Dictionary<StatType, float>();
        if (RuntimeProficiencies != null)
        {
            foreach (var r in RuntimeProficiencies)
                dict[r.type] = Mathf.Clamp01(r.proficiency);
        }
        else if (CurrentClass != null && CurrentClass.proficiencies != null)
        {
            foreach (var s in CurrentClass.proficiencies.stats)
                dict[s.type] = Mathf.Clamp01(s.proficiency);
        }

        foreach (StatType t in Enum.GetValues(typeof(StatType)))
            if (!dict.ContainsKey(t)) dict[t] = 0f;

        return dict;
    }

    public Dictionary<StatType, float> GetWeaponProficienciesDict()
    {
        var dict = new Dictionary<StatType, float>();
        if (EquippedWeapon == null || EquippedWeapon.proficiencies == null || EquippedWeapon.proficiencies.stats == null)
        {
            foreach (StatType t in Enum.GetValues(typeof(StatType)))
                dict[t] = 0f;
            return dict;
        }

        foreach (var s in EquippedWeapon.proficiencies.stats)
            dict[s.type] = Mathf.Clamp01(s.proficiency);

        foreach (StatType t in Enum.GetValues(typeof(StatType)))
            if (!dict.ContainsKey(t)) dict[t] = 0f;

        return dict;
    }

    public void AddRuntimeProficiency(StatType type, float amount)
    {
        if (RuntimeProficiencies == null) InitializeRuntimeProficiencies();

        for (int i = 0; i < RuntimeProficiencies.Length; i++)
        {
            if (RuntimeProficiencies[i].type == type)
            {
                RuntimeProficiencies[i].proficiency = Mathf.Clamp01(RuntimeProficiencies[i].proficiency + amount);
                RuntimeProficiencies[i].proficiency = Mathf.Round(RuntimeProficiencies[i].proficiency * 10f) / 10f;
                RecalculateDerivedStats();
                return;
            }
        }

        Array.Resize(ref RuntimeProficiencies, (RuntimeProficiencies?.Length ?? 0) + 1);
        RuntimeProficiencies[RuntimeProficiencies.Length - 1] = new RuntimeProficiency(type, Mathf.Clamp01(amount));
        RecalculateDerivedStats();
    }
}
