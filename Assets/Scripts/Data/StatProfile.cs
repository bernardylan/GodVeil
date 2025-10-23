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
    [Range(0, 7)] public int proficiency; //F = 0, E = 1, D = 2, C = 3, B = 4, A = 5, S = 6, SS = 7, SSS = 8
}
