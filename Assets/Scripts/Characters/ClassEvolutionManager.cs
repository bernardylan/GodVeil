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
    [SerializeField] private ClassEvolutionUI[] panels;

    private int currentSlotIndex = 0;

    private void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Randomly choose a character to evolve
            int randomSlot = Random.Range(0, CharacterManager.Instance.Characters.Count);
            SetTargetSlot(randomSlot);

            ShowEvolutionOptions();
        }
    }

    public void SetTargetSlot(int index)
    {
        currentSlotIndex = Mathf.Clamp(index, 0, CharacterManager.Instance.Characters.Count - 1);
    }

    public void ShowEvolutionOptions()
    {
        var character = CharacterManager.Instance.Characters[currentSlotIndex];

        TierType currentTier = character.CurrentClass.tier;
        ClassData[] pool = GetNextTierClasses(currentTier);

        // Filtrer avec requirements
        List<ClassData> validClasses = pool
            .Where(c => c.MeetsRequirements(character))
            .ToList();

        // Si aucune classe valide => fallback (au moins 1)
        if (validClasses.Count == 0)
            validClasses = pool.ToList();

        // Prendre un subset random
        var options = GetRandomSubset(validClasses, panels.Length);

        // Injecter dans l'UI
        for (int i = 0; i < options.Count; i++)
            panels[i].Initialize(options[i], OnClassSelected);
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
