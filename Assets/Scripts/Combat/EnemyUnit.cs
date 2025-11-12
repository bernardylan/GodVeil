using UnityEngine;

[RequireComponent(typeof(HpComponent))]
public class EnemyUnit : CombatUnit
{
    public EnemyData enemyData;

    public void Initialize(EnemyData data)
    {
        enemyData = data;
        isEnemy = true;

        // HP & Defense
        hpComponent.Initialize(enemyData.maxHP, enemyData.defense, team: 1);

        // Skills
        autoAttack = data.autoAttack;
        specialSkill = data.specialSkill;
        ultimateSkill = data.ultimateSkill;

        ATB = 0f;
        energy = 0f;
        ATBSpeedMultiplier = enemyData.speed;
        maxEnergy = 100f;
    }

    protected override SkillData DecideSkill()
    {
        float roll = Random.value;

        if (ultimateSkill != null && energy >= maxEnergy * enemyData.ultimateUsageThreshold)
            return ultimateSkill;

        if (specialSkill != null && roll < enemyData.specialUsageChance)
            return specialSkill;

        return autoAttack;
    }

    public override DamageInfo ComputeDamage(CombatUnit target, float skillMultiplier = 1f, float buffs = 0f, float debuffs = 0f, float passives = 0f)
    {
        if (enemyData == null) return new DamageInfo(0f, gameObject, false);

        // Base flat damage
        float dmg = enemyData.attack * skillMultiplier;

        // Apply buffs, debuffs, passives
        dmg *= (1f + buffs - debuffs + passives);

        // Crit chance
        bool isCrit = Random.value < enemyData.critChance;
        if (isCrit) dmg *= 1f + enemyData.critMultiplier;

        // Apply target mitigation
        if (target != null && target.hpComponent != null)
            dmg *= 1f - Mathf.Clamp01(target.hpComponent.mitigation);

        return new DamageInfo(dmg, gameObject, isCrit);
    }
}
