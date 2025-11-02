using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public Dictionary<StatType, float> stats = new();

    public CharacterStats(StatProfile profile)
    {
        foreach (var s in profile.stats)
            stats[s.type] = s.proficiency;
    }
    public float GetProficiency(StatType type) => stats.ContainsKey(type) ? stats[type] : 0;

}
