using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Village/Building", fileName = "New building")]
public class BuildingSO : ScriptableObject
{
    [Header("Basic Info")]
    public string buildingID;
    public string displayName;
    public Sprite icon;
    public string description;

    [Header("Cost")]
    public ResourceAmount[] cost;

    [Header("Unlocks")]
    public BuildingSO[] unlocksNext;
}
