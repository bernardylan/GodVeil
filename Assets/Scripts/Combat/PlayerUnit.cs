using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(HpComponent))]
public class PlayerUnit : CombatUnit
{
    public CharacterStats characterStats;

    public void Initialize(CharacterStats stats)
    {
        characterStats = stats;
        isEnemy = false;
        characterStats.RecalculateDerivedStats();

        // HP & Defense
        hpComponent.Initialize(characterStats.Derived.MaxHP, characterStats.Derived.Defense, team: 0);

        // Assign skills
        autoAttack = stats.CurrentClass.autoAttack;
        specialSkill = stats.CurrentClass.specialSkill;
        ultimateSkill = stats.CurrentClass.ultimateSkill;

        // ATB & Energy
        ATB = 0f;
        energy = 0f;
        ATBSpeedMultiplier = characterStats.Derived.Speed;
        maxEnergy = 100f;
    }

    protected override SkillData DecideSkill()
    {
        if (ultimateSkill != null && energy >= maxEnergy) return ultimateSkill;
        if (specialSkill != null) return specialSkill;
        return autoAttack;
    }

    public override DamageInfo ComputeDamage(CombatUnit target, float skillMultiplier = 1f, float buffs = 0f, float debuffs = 0f, float passives = 0f)
    {
        if (characterStats == null)
            return new DamageInfo(0f, gameObject, false);

        var classProfs = characterStats.GetClassProficiencies();
        var weaponProfs = characterStats.GetWeaponProficiencies();

        float baseDamage;

        if (characterStats.EquippedWeapon != null)
        {
            baseDamage = characterStats.EquippedWeapon.BaseDamage;
        }
        else
        {
            baseDamage = 10f;
            weaponProfs = new Dictionary<StatType, float> {
                { StatType.Strength, 0.1f },
                { StatType.Dexterity, 0.1f },
                { StatType.Intelligence, 0.1f }
                };
        }

        float tierMult = TierUtility.GetMultiplier(characterStats.CurrentClass.tier);

        DamageResult result = DamageService.Calculate(
            classProfs,
            weaponProfs,
            baseDamage,
            tierMult,
            buffs,
            debuffs,
            passives,
            target != null && target.hpComponent != null ? target.hpComponent.mitigation : 0f,
            characterStats.Derived.CritRate,
            0.5f,
            rollCrit: true
        );

        float finalDamage = result.FinalDamage * skillMultiplier;

        return new DamageInfo(finalDamage, gameObject, result.IsCrit);
    }
}
