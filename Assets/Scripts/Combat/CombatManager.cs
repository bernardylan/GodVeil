using UnityEngine;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private List<Transform> playerSpawnPositions;
    [SerializeField] private List<Transform> enemySpawnPositions;

    [SerializeField] private List<EnemyData> enemyPool = new();

    [Header("Units")]
    public List<CombatUnit> playerUnits = new();
    public List<CombatUnit> enemyUnits = new();


    private void Start()
    {
        foreach (var unit in playerUnits)
            unit.OnPerformSkill += HandleSkill;

        foreach (var unit in enemyUnits)
            unit.OnPerformSkill += HandleSkill;

        SpawnEnemyUnits();
        SpawnPlayerUnits();
    }

    private void SpawnPlayerUnits()
    {
        var playerChars = CharacterManager.Instance.Characters;

        for (int i = 0; i < playerChars.Count; i++)
        {
            var chara = playerChars[i];
            var prefab = chara.CurrentClass.classPrefab;
            var spawn = playerSpawnPositions[i];
            var unit = Instantiate(prefab, spawn.position, Quaternion.identity);

            var cu = unit.GetComponent<CombatUnit>();
            cu.characterStats = chara;
            cu.isEnemy = false;

            playerUnits.Add(cu);
            cu.OnPerformSkill += HandleSkill;
        }
    }

    private void SpawnEnemyUnits()
    {
        foreach (var enemyData in enemyPool)
        {
            foreach(var pos in enemySpawnPositions)
            {
                GameObject enemyGO = Instantiate(enemyData.prefab, pos.position, Quaternion.identity);
                EnemyUnit enemyUnit = enemyGO.GetComponent<EnemyUnit>();
                enemyUnit.enemyData = enemyData;
                enemyUnits.Add(enemyUnit.GetComponent<CombatUnit>());
            }
        }
    }

    private void HandleSkill(CombatUnit user, SkillData skill)
    {
        // Choix cible aléatoire
        List<CombatUnit> targets = user.isEnemy ? playerUnits : enemyUnits;
        targets.RemoveAll(u => u.GetComponent<HpComponent>().HP <= 0f);

        if (targets.Count == 0)
        {
            Debug.Log($"{(user.isEnemy ? "Enemy" : "Player")} {user.characterStats.CurrentClass.className} has no target.");
            return;
        }

        CombatUnit target = targets[Random.Range(0, targets.Count)];

        // Appliquer un simple dégât placeholder
        float damageAmount = skill != null ? skill.damageMultiplier : 10f;
        target.GetComponent<HpComponent>().TakeDamage(new DamageInfo(damageAmount, user.gameObject));

        Debug.Log($"{(user.isEnemy ? "Enemy" : "Player")} {user.characterStats.CurrentClass.className} hits {target.characterStats.CurrentClass.className} for {damageAmount} dmg");

        // Energie pour actions spéciales
        user.GainEnergy(20f); // exemple : +20 énergie par action
    }

    [ContextMenu("Debug Combat Status")]
    private void DebugStatus()
    {
        Debug.Log("--- Players ---");
        foreach (var p in playerUnits)
        {
            var hp = p.GetComponent<HpComponent>();
            Debug.Log($"{p.characterStats.CurrentClass.className} | HP: {hp.HP}/{hp.MaxHP} | Energy: {p.energy}/{p.maxEnergy}");
        }

        Debug.Log("--- Enemies ---");
        foreach (var e in enemyUnits)
        {
            var hp = e.GetComponent<HpComponent>();
            Debug.Log($"{e.characterStats.CurrentClass.className} | HP: {hp.HP}/{hp.MaxHP} | Energy: {e.energy}/{e.maxEnergy}");
        }
    }
}
