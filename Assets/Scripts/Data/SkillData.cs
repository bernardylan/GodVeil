using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType skillType;
    public float cooldown;
    public float damageMultiplier;
    public ElementType element;
    public GameObject projectilePrefab;    
}
