using System.Collections.Generic;
using UnityEngine;

public class ClassEvolutionManager : MonoBehaviour
{
    [SerializeField] private ClassData[] tier1Classes;
    [SerializeField] private ClassData[] tier2Classes;
    [SerializeField] private ClassData[] tier3Classes;
    [SerializeField] private ClassEvolutionUI[] panels;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowEvolutionsOptions();
        }
    }

    public void ShowEvolutionsOptions()
    {
        var options = GetRandomClasses(tier1Classes, panels.Length);

        for (int i = 0; i < panels.Length; i++)
            panels[i].Initialize(options[i], OnClassSelected);
    }

    private void OnClassSelected(ClassData selectedClass)
    {
        CharacterManager.Instance.InitializeCharacter(selectedClass);
        Debug.Log($"Selected class: {selectedClass.className}");
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
