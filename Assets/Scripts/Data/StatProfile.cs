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
    [Range(0, 5)] public int proficiency; //E = 0, D = 1, C = 2, B = 3, A = 4, S = 5
}
