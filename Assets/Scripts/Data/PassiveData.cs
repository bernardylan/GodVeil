using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Passive Data")]
public class PassiveData : ScriptableObject
{
    [Header("Icon & Effets")]
    public Sprite icon;
    public PassiveEffect[] effects;

    [Header("Localization Keys")]
    public string passiveNameKey; // ex: "Villager_passive_Name"
    public string passiveDescriptionKey;// ex: "Villager_passive_Desc"
}

[System.Serializable]
public class PassiveEffect
{
    public StatType affectedStat;
    public float modifier;
}