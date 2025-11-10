using UnityEngine;

[RequireComponent(typeof(CombatUnit), typeof(HpComponent))]
public class EnemyUnit : MonoBehaviour
{
    public EnemyData enemyData;
    private CombatUnit combatUnit;
    private HpComponent hpComponent;

    private void Awake()
    {
        combatUnit = GetComponent<CombatUnit>();
        hpComponent = GetComponent<HpComponent>();

        if (enemyData != null)
        {
            // Initialize HpComponent
            hpComponent.Initialize(enemyData.maxHP, enemyData.defense, team: 1);

            // Initialize CombatUnit
            combatUnit.characterStats = null; // No CharacterStats
            combatUnit.isEnemy = true;
            combatUnit.autoAttack = enemyData.autoAttack;
            combatUnit.specialSkill = enemyData.specialSkill;
            combatUnit.ultimateSkill = enemyData.ultimateSkill;
            combatUnit.atb = 0f;
            combatUnit.energy = 0f;
            combatUnit.maxEnergy = 100f;
            combatUnit.atbSpeedMultiplier = enemyData.speed;
        }
    }
}
