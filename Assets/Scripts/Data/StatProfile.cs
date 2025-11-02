using UnityEngine;
[CreateAssetMenu(menuName = "GodVeil/Stat Profile")]

public class StatProfile : ScriptableObject
{
    public StatValue[] stats; //FOR, DEX, INT

    private void OnValidate()
    {
        if (stats == null) return;

        foreach (var s in stats)
        {
            // Snap à la dizaine
            s.proficiency = Mathf.Round(s.proficiency * 10f) / 10f;
        }
    }
}

[System.Serializable]
public class StatValue
{
    public StatType type;
    [Tooltip("1 = S+ | 0.9 = S | 0.8 = A+ | 0.7 = A | 0.6 = B+ | 0.5 = B| 0.4 = C+ | 0.3 = C | 0.2 = D+ | 0.1 = D")]
    [Range(0.1f, 1.0f)] public float proficiency;
}
