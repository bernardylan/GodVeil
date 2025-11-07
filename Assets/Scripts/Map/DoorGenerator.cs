using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    [Header("Doors configs")]
    public List<GameObject> allDoorsPrefabs;
    public List<Transform> spawnPositions;
    public int numberOfDoors = 2;

    private List<GameObject> currentDoors = new List<GameObject>();

    private void Start()
    {
        SpawnDoors();
    }

    public void SpawnDoors()
    {
        currentDoors.Clear();

        int doorsToSpawn = Mathf.Min(numberOfDoors, spawnPositions.Count);

        List<GameObject> availableDoors = new List<GameObject>(allDoorsPrefabs);

        for (int i = 0; i < doorsToSpawn; i++)
        {
            if (availableDoors.Count == 0) break;

            int index = Random.Range(0, availableDoors.Count);
            GameObject chosenDoor = Instantiate(availableDoors[index], spawnPositions[i].position, spawnPositions[i].rotation);

            chosenDoor.transform.SetParent(this.transform);

            currentDoors.Add(chosenDoor);
            availableDoors.RemoveAt(index);
        }
    }

    public void ClearDoors()
    {
        foreach(var door in currentDoors)
        {
            if (door != null)
                Destroy(door);
        }
        currentDoors.Clear();
    }

}
