using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Passive Data")]
public class PassiveData : ScriptableObject
{
    [Header("Icon & Effets")]
    public Sprite icon;
    public PassiveEffect[] effects;

    [Header("Localization Keys")]
    public string passiveNameKey;        // ex: "Villager_Passive_Name"
    public string passiveDescriptionKey;

}

[System.Serializable]
public class PassiveEffect
{
    public StatType affectedStat;
    public float modifier;
}