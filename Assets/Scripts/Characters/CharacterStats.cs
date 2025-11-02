using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    public Dictionary<StatType, float> stats = new();

    public CharacterStats(StatProfile profile)
    {
        foreach (var s in profile.stats)
        {
            float snapped = Mathf.Round(s.proficiency * 10f) / 10f;
            stats[s.type] = snapped;
        }
    }
    public float GetProficiency(StatType type) => stats.ContainsKey(type) ? stats[type] : 0;

}
