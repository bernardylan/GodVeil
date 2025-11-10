using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Door Config")]
public class DoorConfig : ScriptableObject
{
    public List<DoorData> allDoors;

    public DoorData GetRandomDoor(bool isFirstRoom, List<DoorType> excludedTypes)
    {
        var validDoors = new List<DoorData>();

        foreach (var door in allDoors)
        {
            if (!isFirstRoom || door.canAppearAtStart)
            {
                if (!excludedTypes.Contains(door.type))
                    validDoors.Add(door);
            }
        }

        if (validDoors.Count == 0)
        {
            Debug.LogWarning("Noo doors founds, get random one");
            return allDoors[Random.Range(0, allDoors.Count)];
        }

        float totalWeight = 0f;
        foreach (var d in validDoors)
            totalWeight += d.spawnProba;

        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var d in validDoors)
        {
            cumulative += d.spawnProba;
            if (randomValue <= cumulative)
                return d;
        }

        return validDoors[validDoors.Count - 1];
    }
}
