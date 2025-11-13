using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Village/Building", fileName = "New building")]
public class BuildingSO : ScriptableObject
{
    [Header("Basic Info")]
    public string buildingID;
    public string displayName;
    public Sprite icon;
    public string description;

    [Header("Requirements")]
    public ResourceAmount[] cost;
    public BuildingSO[] requiredBuilding;

    [Header("Unlocks")]
    public BuildingSO[] unlocksNext;
}
