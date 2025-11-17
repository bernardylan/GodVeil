using System;
using UnityEngine;

[Serializable]
public class RuntimeProficiency
{
    public StatType type;
    public float proficiency;

    public RuntimeProficiency(StatType t, float p)
    {
        type = t;
        proficiency = Mathf.Clamp01(p);
    }
}

