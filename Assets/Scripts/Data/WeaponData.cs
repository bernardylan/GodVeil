using UnityEngine;

[CreateAssetMenu(menuName = "GodVeil/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Base Info")]
    public string weaponName;
    public Sprite weaponIcon;
    public int BaseDamage;

    [Header("Base Stats")]
    public StatProfile baseStats; // Stock stats STR/DEX/INT....

}
