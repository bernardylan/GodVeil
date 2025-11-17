using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ClassEvolutionManager : MonoBehaviour
{
    public static ClassEvolutionManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Transform container;
    [SerializeField] private ClassEvolutionUI panelPrefab;

    [Header("Options")]
    [SerializeField]
    private int maxOptions = 3;

    // Stockage des panels actuels
    private List<ClassEvolutionUI> activePanels = new();
    private int currentSlotIndex = 0;
    public int CurrentSlotIndex => currentSlotIndex;

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
        if (CharacterManager.Instance == null) return;
        foreach (var character in CharacterManager.Instance.Characters)
            RegisterCharacterForStats(character);
    }

    private void OnDisable()
    {
        if (CharacterManager.Instance == null) return;
        foreach (var character in CharacterManager.Instance.Characters)
            character.OnStatsChanged -= OnCharacterStatsChanged;
    }


    public void RegisterCharacterForStats(CharacterStats character)
    {
        if (character == null) return;
        character.OnStatsChanged -= OnCharacterStatsChanged; // safe unsubscribe
        character.OnStatsChanged += OnCharacterStatsChanged;
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
        ClearPanels();

        var character = CharacterManager.Instance.Characters[currentSlotIndex];

        var validClasses = ClassEvolutionService.GetValidClassesForCharacter(character);

        // Si aucune classe valide => fallback
        if (validClasses.Count == 0)
            return;
        Debug.Log("Valid classes count = " + validClasses.Count);

        var options = ClassEvolutionService.GetRandomSubset(validClasses, maxOptions); // Par exemple 3 panels

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
        var character = CharacterManager.Instance.Characters[currentSlotIndex];
        foreach (var panel in activePanels)
            ClassEvolutionService.BanClassForCharacter(character, panel.CurrentClass);

        Debug.Log("[DEBUG] Banned current options");
        ShowEvolutionOptions();
    }

    private void OnClassSelected(ClassData selectedClass)
    {
        var character = CharacterManager.Instance.Characters[currentSlotIndex];
        int nextTier = (int)selectedClass.tier;
        ClassEvolutionService.PickClassAndAdvance(character, selectedClass, nextTier);

        Debug.Log($"[ClassEvolutionManager] Character {currentSlotIndex} picked {selectedClass.className} (Tier {nextTier})");

        ShowEvolutionOptions(); // mettre à jour UI pour prochaine tier
    }
}
