using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Door Data")]
public class DoorData : ScriptableObject
{
    public DoorType type;
    public string sceneName;
    public GameObject prefab;

    [Range(0f, 1f)] public float spawnProba = 1f;
    public bool canAppearAtStart = true;
    public bool canAppearConsecutively = true;
}

public enum DoorType
{
    Minion,
    Elite,
    Boss,
    Event,
    Chest,
    Blacksmith
}
