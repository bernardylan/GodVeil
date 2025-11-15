using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Stat Profile")]
public class StatProfile : ScriptableObject
{
    public StatValue[] stats; // STR / DEX / INT / SPEED

    private void OnValidate()
    {
        if (stats == null) return;
        foreach (var s in stats)
            s.proficiency = Mathf.Round(s.proficiency * 10f) / 10f; // Snap
    }
}

[System.Serializable]
public class StatValue
{
    public StatType type;
    [Range(0f, 1f)]
    [Tooltip("0=D, 0.1=D+, 0.2=D+, ... 1=S+")]
    public float proficiency;
}
