using UnityEngine;

[RequireComponent(typeof(HpComponent))]
public class BossUnit : EnemyUnit
{
    private BossData bossData;
    private int currentPhase = 1;

    protected override void Awake()
    {
        base.Awake();
        bossData = enemyData as BossData;
        if (bossData == null)
        {
            Debug.LogError($"{name}: BossData missing !");
            return;
        }
        DeterminePhase();
    }

    protected override void Update()
    {
        if (hpComponent.HP <= 0f) return;
        DeterminePhase();
        base.Update();
    }

    private void DeterminePhase()
    {
        float hpPercent = hpComponent.GetHPPercent();

        if (hpPercent <= bossData.phase3Threshold)
            currentPhase = 3;
        else if (hpPercent <= bossData.phase2Threshold)
            currentPhase = 2;
        else
            currentPhase = 1;
    }

    protected override SkillData DecideSkill()
    {
        float roll = Random.value;

        switch (currentPhase)
        {
            case 1:
                // Phase 1 = Normal
                if (specialSkill != null && roll < enemyData.specialUsageChance)
                    return specialSkill;
                break;

            case 2:
                // Phase 2 = More aggressive
                if (ultimateSkill != null && energy >= maxEnergy * enemyData.ultimateUsageThreshold && roll < bossData.ultimateUsageChance)
                    return ultimateSkill;
                if (specialSkill != null && roll < bossData.phase2SpecialChance)
                    return specialSkill;
                break;

            case 3:
                // Phase 3 = Berzerk
                if (ultimateSkill != null && energy >= maxEnergy * enemyData.ultimateUsageThreshold && roll < bossData.ultimateUsageChance)
                    return ultimateSkill;
                if (specialSkill != null && roll < bossData.phase3SpecialChance)
                    return specialSkill;
                break;
        }

        return autoAttack;
    }
}
