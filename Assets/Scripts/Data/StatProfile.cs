using UnityEngine;
[CreateAssetMenu(menuName = "GodVeil/Stat Profile")]

public class StatProfile : ScriptableObject
{
    public StatValue[] stats; //FOR, DEX, INT
}

[System.Serializable]
public class StatValue
{
    public StatType type;
    [Range(0.1f, 1.0f)] public float proficiency; 
}
