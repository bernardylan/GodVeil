using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ClassEvolutionManager : MonoBehaviour
{
    [SerializeField] private ClassData[] tier1Classes;
    [SerializeField] private ClassData[] tier2Classes;
    [SerializeField] private ClassData[] tier3Classes;

    [SerializeField] private Transform[] visuSlot;

    private ClassData currentClass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentClass = CharacterManager.Instance.GetComponent<ClassData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
            TryEvolveClass();
    }

    public void TryEvolveClass()
    {
        ClassData[] evolutionsT1 = GetRandomEvolutions(tier1Classes, 3);
        Debug.Log("Evolutions possibles :");
        foreach (var c in evolutionsT1)
        {
            Debug.Log(c.className);
        }


        //if (currentClass == null)
        //{
        //    Debug.LogWarning("No class found");
        //    return;
        //}

        //ClassData nextClass = GetEvolution(currentClass);

        //if (nextClass != null)
        //{
        //    CharacterManager.Instance.InitializeCharacter(nextClass);
        //    currentClass = nextClass;
        //    Debug.Log($"Evolution of {currentClass} to a {nextClass}");
        //}
        //else
        //    Debug.Log($"No class available for {currentClass}");
    }

    private ClassData[] GetRandomEvolutions(ClassData[] availableClasses, int count)
    {
        List<ClassData> pool = new List<ClassData>(availableClasses);
        List<ClassData> result = new List<ClassData>();

        for(int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);

            result.Add(pool[index]);
            pool.RemoveAt(index);
        }
        return result.ToArray();
    }

   /* private ClassData GetEvolution(ClassData baseClass)
    {
        var possibleT1Evolutions = GetRandomEvolutions(tier1Classes, 3);
        var possibleT2Evolutions = GetRandomEvolutions(tier2Classes, 3);
        var possibleT3Evolutions = GetRandomEvolutions(tier3Classes, 3);

        switch(baseClass.tier)
        {
            case TierType.Tier0:
                for (int i = 0; i < possibleT1Evolutions.Count; i++)
                {
                    Transform slot = visuSlot[i];
                    GameObject go = possibleT1Evolutions[i].classPrefab;

                    Instantiate(go, slot.transform);
                }
                return ;
            case TierType.Tier1:
                for(int i = 0; i < possibleT2Evolutions.Count; i++)
                {
                    Transform slot = visuSlot[i];
                    GameObject go = possibleT2Evolutions[i].classPrefab;

                    Instantiate(go, slot.transform);
                }
                return tier2Classes[Random.Range(0, tier2Classes.Length)];
            case TierType.Tier2:
                for(int i = 0; i < possibleT3Evolutions.Count; i++)
                {
                    Transform slot = visuSlot[i];
                    GameObject go = possibleT3Evolutions[i].classPrefab;
                    
                    Instantiate(go, slot.transform);
                }
                return tier3Classes[Random.Range(0, tier3Classes.Length)];
            case TierType.Tier3:
                return null;
            
            default:
                return null;
        }
    }*/
}


