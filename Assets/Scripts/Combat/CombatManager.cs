using UnityEngine;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> playerSpawnPositions;
    [SerializeField] private Transform[] enemySpawnPositions;

    [Header("Enemy Pool")]
    public int maxEnemy = 4;
    [SerializeField] private List<EnemyData> enemyPool = new();

    [Header("UI")]
    [SerializeField] private CombatUIManager uiManager;
    [SerializeField] private GameObject hpBarPrefab;

    [Header("Units")]
    public List<PlayerUnit> playerUnits = new();
    public List<EnemyUnit> enemyUnits = new();

    public Vector3 offset;
    public Transform target;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SpawnPlayerUnits();
        SpawnEnemyUnits(maxEnemy);
        SoundManager.Instance.MusicVolume(0.2f);
        SoundManager.Instance.PlayMusicWithFade("BattleTheme", 1.5f);
        uiManager.Initialize(playerUnits);
    }

    private void SpawnPlayerUnits()
    {
        var chars = CharacterManager.Instance.Characters;
        for (int i = 0; i < chars.Count; i++)
        {
            var prefab = chars[i].CurrentClass.classPrefab;
            var spawn = playerSpawnPositions[i];
            var obj = Instantiate(prefab, spawn.position, spawn.rotation);

            var unit = obj.GetComponent<PlayerUnit>();
            unit.Initialize(chars[i]);

            playerUnits.Add(unit);
        }
    }

    private void SpawnEnemyUnits(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (enemyPool.Count == 0) break;
            var data = enemyPool[Random.Range(0, enemyPool.Count)];
            var spawn = enemySpawnPositions[i % enemySpawnPositions.Length];

            var obj = Instantiate(data.prefab, spawn.position, spawn.rotation);
            var enemy = obj.GetComponent<EnemyUnit>();
            enemy.Initialize(data);
            enemy.hpComponent = enemy.GetComponent<HpComponent>();
            Vector3 barSpawn = enemy.transform.position + offset;
            var hpBarObj = Instantiate(hpBarPrefab, barSpawn, Quaternion.identity);
            var barScript = hpBarObj.GetComponent<WorldSpaceUnitUI>();
            barScript.hpComponent = enemy.hpComponent;
            barScript.target = enemy.transform;
            barScript.offset = offset;
            barScript.GetComponent<Canvas>().worldCamera = Camera.main;

            enemyUnits.Add(enemy);
        }
    }

}
