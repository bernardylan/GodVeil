using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Icon & Type")]
    public Sprite icon;
    public SkillType skillType; // AutoAttack / Special / Ultimate
    public ElementType element;
    
    [Header("Localization Keys")]
    public string skillNameKey;        // ex: "Villager_AutoAttack_Name"
    public string skillDescriptionKey; // ex: "Villager_AutoAttack_Desc"

    [Header("Stats")]
    public float cooldown;
    public float damageMultiplier;
}
