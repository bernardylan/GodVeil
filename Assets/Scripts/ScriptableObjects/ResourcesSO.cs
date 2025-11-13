using UnityEngine;

public enum ResourceType { Gold, MonsterEssence, DivineEssence, GodCrystal };

[CreateAssetMenu(menuName = "GodVeil/Resources", fileName = "Village Resources")]
public class ResourcesSO : ScriptableObject
{
    public ResourceType type;
    public string displayName;
    public Sprite icon;
}
