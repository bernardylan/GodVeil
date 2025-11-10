using UnityEngine;

[RequireComponent(typeof(HpComponent))]
public class PlayerUnit : CombatUnit
{
    public CharacterStats characterStats;

    public void Initialize(CharacterStats stats)
    {
        characterStats = stats;
        isEnemy = false;

        // Initialize HP
        hpComponent.InitializeFromStats(stats);

        // Assign skills
        autoAttack = stats.CurrentClass.autoAttack;
        specialSkill = stats.CurrentClass.specialSkill;
        ultimateSkill = stats.CurrentClass.ultimateSkill;

        // ATB & Energy
        atb = 0f;
        energy = 0f;
        atbSpeedMultiplier = stats.Derived.Speed;
    }
}
