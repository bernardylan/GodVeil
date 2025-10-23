using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Class Data")]
public class ClassData : ScriptableObject
{
    [Header("Base Info")]
    public string className;
    public Sprite classIcon;
    public RuntimeAnimatorController animator;
    public RankType rank;
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
public enum RankType { Rank0, Rank1, Rank2, Rank3 }