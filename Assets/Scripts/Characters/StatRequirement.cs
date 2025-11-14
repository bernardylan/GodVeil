using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public struct StatRequirement
{
    public StatType stat;
    [Range(0f, 1f)]
    public float minimum;
}

