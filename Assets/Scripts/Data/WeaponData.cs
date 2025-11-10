using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Base Info")]
    public string weaponName;
    public Sprite weaponIcon;
    public int BaseDamage;

    [Header("Proficiencies")]
    public StatProfile proficiencies; // STR/DEX/INT
}
