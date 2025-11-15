using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClassEvolutionManager : MonoBehaviour
{
    [Header("Class Pools")]
    [SerializeField] private ClassData[] tier1Classes;
    [SerializeField] private ClassData[] tier2Classes;
    [SerializeField] private ClassData[] tier3Classes;

    [Header("UI")]
    public GameObject panelPrefab;
    public Transform uiParent;

    private CharacterStats currentCharacter;

    private int currentSlotIndex = 0;
    public int CurrentSlotIndex => currentSlotIndex;

    // Stockage des panels actuels
    private List<ClassEvolutionUI> activePanels = new();
    private HashSet<ClassData> bannedClasses = new();

    public void SetTargetSlot(int index)
    {
        currentSlotIndex = Mathf.Clamp(index, 0, CharacterManager.Instance.Characters.Count - 1);

        // Se désabonner de l'ancien
        if (currentCharacter != null)
            currentCharacter.OnStatsChanged -= RefreshEvolutionPanel;

        // S'abonner au nouveau
        currentCharacter = CharacterManager.Instance.Characters[currentSlotIndex];
        currentCharacter.OnStatsChanged += RefreshEvolutionPanel;
    }

    private void RefreshEvolutionPanel()
    {
        ShowEvolutionOptions();
    }

    public void ShowEvolutionOptions()
    {
        ClearPanels();

        var character = CharacterManager.Instance.Characters[currentSlotIndex];

        TierType currentTier = character.CurrentClass.tier;
        ClassData[] pool = GetNextTierClasses(currentTier);

        // Filtrer avec requirements
        List<ClassData> validClasses = pool
            .Where(c => c.MeetsRequirements(character))
            .ToList();

        // Si aucune classe valide => fallback
        if (validClasses.Count == 0)
            return; // Aucun panel → pas de requirements atteints

        var options = GetRandomSubset(validClasses, 3); // Par exemple 3 panels

        foreach (var classData in options)
        {
            GameObject go = Instantiate(panelPrefab, uiParent);
            var ui = go.GetComponent<ClassEvolutionUI>();
            ui.Initialize(classData, OnClassSelected);

            activePanels.Add(ui);
        }
    }

    private void ClearPanels()
    {
        foreach (var p in activePanels)
            Destroy(p.gameObject);

        activePanels.Clear();
    }

    public void DebugReroll()
    {
        Debug.Log("[DEBUG] Reroll evolution options");
        ShowEvolutionOptions();
    }

    public void DebugBanCurrentOptions()
    {
        foreach (var panel in activePanels)
            bannedClasses.Add(panel.CurrentClass);

        Debug.Log("[DEBUG] Banned current options");
        ShowEvolutionOptions();
    }


    private ClassData[] GetNextTierClasses(TierType tier)
    {
        return tier switch
        {
            TierType.T0 => tier1Classes,
            TierType.T1 => tier2Classes,
            TierType.T2 => tier3Classes,
            _ => tier3Classes
        };
    }

    private List<ClassData> GetRandomSubset(List<ClassData> list, int count)
    {
        List<ClassData> pool = new(list);
        List<ClassData> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }

    private void OnClassSelected(ClassData selectedClass)
    {
        CharacterManager.Instance.EvolveCharacter(currentSlotIndex, selectedClass);
    }
}
