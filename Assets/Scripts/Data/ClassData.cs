using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Class Data")]
public class ClassData : ScriptableObject
{
    [Header("Base Info")]
    public string className;
    public Sprite classIcon;
    public GameObject classPrefab;
    public TierType tier;
    public bool isLocked = false;

    [Header("Base Stats")]
    public StatProfile baseStats; // Stock stats FOR/DEX/INT....

    [Header("Links")]
    public SkillData autoAttack;
    public SkillData specialSkill;
    public SkillData ultimateSkill;
    public PassiveData passive;
}

public enum StatType { Strength, Dexterity, Intelligence }
public enum SkillType { AutoAttack, Special, Ultimate }
public enum ElementType { None, Fire, Water, Earth, Lightning, Poison }
public enum TierType { Tier0, Tier1, Tier2, Tier3 }