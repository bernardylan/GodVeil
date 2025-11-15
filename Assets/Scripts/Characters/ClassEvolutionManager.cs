using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ClassEvolutionManager : MonoBehaviour
{
    public static ClassEvolutionManager Instance { get; private set; }

    [Header("Class Pools")]
    [SerializeField] private ClassData[] tier1Classes;
    [SerializeField] private ClassData[] tier2Classes;
    [SerializeField] private ClassData[] tier3Classes;

    [Header("UI")]
    [SerializeField] private Transform container;
    [SerializeField] private ClassEvolutionUI panelPrefab;

    //private CharacterStats currentCharacter;

    private int currentSlotIndex = 0;
    public int CurrentSlotIndex => currentSlotIndex;

    // Stockage des panels actuels
    private List<ClassEvolutionUI> activePanels = new();
    public HashSet<ClassData> bannedClasses = new();

    int maxOptions = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        foreach (var character in CharacterManager.Instance.Characters)
            character.OnStatsChanged += OnCharacterStatsChanged;
    }

    private void OnDisable()
    {
        foreach (var character in CharacterManager.Instance.Characters)
            character.OnStatsChanged -= OnCharacterStatsChanged;
    }

    public void SetTargetSlot(int index)
    {
        currentSlotIndex = Mathf.Clamp(index, 0, CharacterManager.Instance.Characters.Count - 1);
    }

    private void OnCharacterStatsChanged(CharacterStats stats)
    {
        int index = FindCharacterIndex(stats);
        if (index == -1) return;

        if (index == -1)
            return;

        // On fixe le slot courant
        SetTargetSlot(index);

        // On montre les options
        ShowEvolutionOptions();
    }

    private int FindCharacterIndex(CharacterStats target)
    {
        var list = CharacterManager.Instance.Characters;
        for (int i = 0; i < list.Count; i++)
        {
            if (ReferenceEquals(list[i], target))
                return i;
        }
        return -1;
    }

    public void ShowEvolutionOptions()
    {
        Debug.Log("[CLASS EVO] ShowEvolutionOptions() CALLED");

        var character = CharacterManager.Instance.Characters[currentSlotIndex];

        TierType currentTier = character.CurrentClass.tier;
        ClassData[] pool = GetNextTierClasses(currentTier);

        // Filtrer avec requirements
        List<ClassData> validClasses = pool
            .Where(c => c.MeetsRequirements(character))
            .ToList();

        // Si aucune classe valide => fallback
        if (validClasses.Count == 0)
            return;
        Debug.Log("Valid classes count = " + validClasses.Count);

        var options = GetRandomSubset(validClasses, maxOptions); // Par exemple 3 panels

        ClearPanels();

        foreach (var classData in options)
        {
            var ui = Instantiate(panelPrefab, container);
            ui.Initialize(classData, OnClassSelected);
            activePanels.Add(ui);
        }
    }

    public void ClearPanels()
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

    public void OnClassSelected(ClassData selectedClass)
    {
        CharacterManager.Instance.EvolveCharacter(currentSlotIndex, selectedClass);
    }
}
