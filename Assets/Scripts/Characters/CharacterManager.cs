using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [Header("Characters List")]
    [SerializeField] private List<CharacterStats> characters = new();
    public IReadOnlyList<CharacterStats> Characters => characters;

    [Header("Max Characters")]
    [SerializeField] private int maxCharacters = 4;

    [Header("Classes T0 available")]
    [SerializeField] private List<ClassData> defaultT0Classes = new(); // Villager1..4

    [Header("Starting T1 pool")]
    [SerializeField] private List<ClassData> baseT1Pool = new();
    [Header("Starting T2 pool")]
    [SerializeField] private List<ClassData> baseT2Pool = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (characters.Count == 0)
            {
                // Auto creation of T0 when run start
                foreach (var t0 in defaultT0Classes)
                    AddCharacter(t0);
            }
        }
        else Destroy(gameObject);
    }

    public void ResetAllCharactersForNewRun()
    {
        foreach (var character in Characters)
        {
            character.ResetProficienciesForRun();
            character.RecalculateDerivedStats();
        }
        ClassEvolutionManager.Instance.ClearPanels();
    }

    public CharacterStats AddCharacter(ClassData classData, WeaponData weapon = null)
    {
        if (characters.Count >= maxCharacters)
            return null;

        CharacterStats newChar = new CharacterStats(classData, weapon);
        // Initialise le state des classes avec la pool de base (T1)
        ClassEvolutionService.InitState(newChar, baseT1Pool);

        characters.Add(newChar);

        // Optional: subscribe ClassEvolutionManager to this character's OnStatsChanged
        if (ClassEvolutionManager.Instance != null)
            ClassEvolutionManager.Instance.RegisterCharacterForStats(newChar);

        Debug.Log($"[CharacterManager] New character added : {classData.className}");
        return newChar;
    }

    public void RecalculateAllCharacters()
    {
        foreach (var c in characters)
            c.RecalculateDerivedStats();
    }

    public void EvolveCharacter(int slotIndex, ClassData newClass)
    {
        if (slotIndex < 0 || slotIndex >= characters.Count)
        {
            Debug.LogWarning("Slot null");
            return;
        }

        CharacterStats chara = characters[slotIndex];
        float oldHPPercent = chara.CurrentHP / chara.Derived.MaxHP; //HP ratio
        chara.CurrentClass = newClass;
        chara.RecalculateDerivedStats();
        chara.CurrentHP = chara.Derived.MaxHP * oldHPPercent;

        Debug.Log($"[CharacterManager] {slotIndex} evolve to {newClass.className}");
    }

    [ContextMenu("Debug Character Slots")]
    private void DebugCharacterSlots()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            var c = characters[i];
            Debug.Log($"Slot {i + 1}: {c.CurrentClass.className} | HP: {c.CurrentHP}/{c.Derived.MaxHP} | Weapon: {(c.EquippedWeapon ? c.EquippedWeapon.weaponName : "None")}");
        }
    }

}