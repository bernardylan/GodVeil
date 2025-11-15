using UnityEngine;

public class DebugStatCheat : MonoBehaviour
{
    [SerializeField] private ClassEvolutionManager evoManager;

    private CharacterStats current => CharacterManager.Instance.Characters[evoManager.CurrentSlotIndex];

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddStat(StatType.Strength);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddStat(StatType.Dexterity);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            AddStat(StatType.Intelligence);

        if (Input.GetKeyDown(KeyCode.R))
            evoManager.DebugReroll();

        if (Input.GetKeyDown(KeyCode.B))
            evoManager.DebugBanCurrentOptions();
    }

    private void AddStat(StatType stat)
    {
        var prof = current.CurrentClass.proficiencies.stats;

        for (int i = 0; i < prof.Length; i++)
        {
            if (prof[i].type == stat)
            {
                prof[i].proficiency += 0.1f;
                break;
            }
        }

        current.RecalculateDerivedStats();
        Debug.Log($"[DEBUG] +0.1 ¨ {stat}");
    }
}
