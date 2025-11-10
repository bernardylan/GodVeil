using UnityEngine;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawnPositions;
    [SerializeField] private Transform[] enemySpawnPositions;

    [SerializeField] private List<EnemyData> enemyPool = new();

    public List<PlayerUnit> playerUnits = new();
    public List<EnemyUnit> enemyUnits = new();

    private void Start()
    {
        SpawnPlayerUnits();
        SpawnEnemyUnits(2);
    }

    private void SpawnPlayerUnits()
    {
        var characters = CharacterManager.Instance.Characters;
        for (int i = 0; i < characters.Count; i++)
        {
            var prefab = characters[i].CurrentClass.classPrefab;
            var unitGO = Instantiate(prefab, playerSpawnPositions[i].position, Quaternion.identity);
            var playerUnit = unitGO.GetComponent<PlayerUnit>();
            playerUnit.Initialize(characters[i]);
            playerUnit.OnPerformSkill += HandleSkill;
            playerUnits.Add(playerUnit);
        }
    }

    private void SpawnEnemyUnits(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (enemyPool.Count == 0) break;

            var data = enemyPool[Random.Range(0, enemyPool.Count)];
            var spawn = enemySpawnPositions[i % enemySpawnPositions.Length];
            var unitGO = Instantiate(data.prefab, spawn.position, spawn.rotation);
            var enemyUnit = unitGO.GetComponent<EnemyUnit>();
            enemyUnit.Initialize(data);
            enemyUnit.OnPerformSkill += HandleSkill;
            enemyUnits.Add(enemyUnit);
        }
    }

    private void HandleSkill(CombatUnit user, SkillData skill)
    {
        List<CombatUnit> targets = user.isEnemy ? new List<CombatUnit>(playerUnits) : new List<CombatUnit>(enemyUnits);
        targets.RemoveAll(u => u.GetComponent<HpComponent>().HP <= 0f);
        if (targets.Count == 0) return;

        var target = targets[Random.Range(0, targets.Count)];
        float dmg = skill != null ? skill.damageMultiplier : 10f;
        target.GetComponent<HpComponent>().TakeDamage(new DamageInfo(dmg, user.gameObject));

        user.GainEnergy(20f);
    }
}
