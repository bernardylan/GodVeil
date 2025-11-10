using UnityEngine;

[RequireComponent(typeof(HpComponent))]
public class EnemyUnit : CombatUnit
{
    public EnemyData enemyData;

    public void Initialize(EnemyData data)
    {
        enemyData = data;
        isEnemy = true;

        // Initialize HP
        hpComponent.Initialize(enemyData.maxHP, enemyData.defense, team: 1);

        // Assign skills
        autoAttack = enemyData.autoAttack;
        specialSkill = enemyData.specialSkill;
        ultimateSkill = enemyData.ultimateSkill;

        // ATB & Energy
        atb = 0f;
        energy = 0f;
        atbSpeedMultiplier = enemyData.speed;
        maxEnergy = 100f;
    }

    protected override SkillData DecideSkill()
    {
        // Exemple simple AI : Ultimate si full, sinon special chance, sinon auto
        if (ultimateSkill != null && energy >= maxEnergy)
            return ultimateSkill;
        if (specialSkill != null && UnityEngine.Random.value < enemyData.specialUsageChance)
            return specialSkill;
        return autoAttack;
    }
}
