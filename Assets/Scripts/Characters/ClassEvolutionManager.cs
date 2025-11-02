using System.Collections.Generic;
using UnityEngine;

public class ClassEvolutionManager : MonoBehaviour
{
    [SerializeField] private ClassData[] tier1Classes;
    [SerializeField] private ClassData[] tier2Classes;
    [SerializeField] private ClassData[] tier3Classes;
    [SerializeField] private ClassEvolutionUI[] panels;

    [Header("Slot ciblé pour l’évolution (0–3)")]
    [SerializeField] private int currentSlotIndex = 0;

    private void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Choisir un personnage actif aléatoire
            int randomSlot = Random.Range(0, CharacterManager.Instance.Characters.Count);
            SetTargetSlot(randomSlot);

            ShowEvolutionsOptions();
        }
    }

    public void SetTargetSlot(int index)
    {
        currentSlotIndex = Mathf.Clamp(index, 0, CharacterManager.Instance.Characters.Count - 1);
    }

    public void ShowEvolutionsOptions()
    {
        var options = GetRandomClasses(tier1Classes, panels.Length);

        for (int i = 0; i < panels.Length; i++)
            panels[i].Initialize(options[i], OnClassSelected);
    }

    private void OnClassSelected(ClassData selectedClass)
    {
        CharacterManager.Instance.EvolveCharacter(currentSlotIndex, selectedClass);
        Debug.Log($"Slot {currentSlotIndex} évolue en {selectedClass.className}");
    }

    private static ClassData[] GetRandomClasses(ClassData[] available, int count)
    {
        List<ClassData> pool = new(available);
        List<ClassData> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result.ToArray();
    }
}
