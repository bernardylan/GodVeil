using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Passive Data")]
public class PassiveData : ScriptableObject
{
    public string passiveName;
    public string description;
    public PassiveEffect[] effects;
}

[System.Serializable]
public class PassiveEffect
{
    public StatType affectedStat;
    public float modifier;
}