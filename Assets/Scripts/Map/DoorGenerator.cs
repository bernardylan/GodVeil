using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    [Header("Config")]
    public DoorConfig doorConfig;
    public List<Transform> spawnPositions;
    public int numberOfDoors = 2;

    private List<GameObject> currentDoors = new();

    private void Start()
    {
        SpawnDoors(isFirstRoom: true);
    }

    public void SpawnDoors(bool isFirstRoom)
    {
        ClearDoors();
        int doorsToSpawn = Mathf.Min(numberOfDoors, spawnPositions.Count);
        List<DoorType> excluded = new();

        for (int i = 0; i < doorsToSpawn; i++)
        {
            DoorData data = doorConfig.GetRandomDoor(isFirstRoom, excluded);
            excluded.Add(data.type);

            GameObject go = Instantiate(data.prefab, spawnPositions[i].position, spawnPositions[i].rotation);
            go.transform.SetParent(transform);
            currentDoors.Add(go);
        }
    }

    public void ClearDoors()
    {
        foreach (var door in currentDoors)
            if (door != null) Destroy(door);

        currentDoors.Clear();
    }
}
