[System.Serializable]
public class CharacterStats
{
    public ClassData CurrentClass;
    public WeaponData EquippedWeapon;
    public float CurrentHP;
    public float CurrentEnergy;
    public bool IsLocked;
    public int Level;

    public DerivedStats Derived;

    // scaling coeff.
    private const float HPScaling      = 0.5f;  // STR boost MaxHP
    private const float DefenseScaling = 2f;    // STR boost Defense
    private const float CritScaling    = 3f;    // DEX boost CritRate
    private const float DodgeScaling   = 3f;    // DEX boost Dodge
    private const float HitScaling     = 0.2f;  // INT boost HitChance
    private const float EnergyScaling  = 0.5f;  // INT boost EnergyRegen

    public CharacterStats(ClassData classData, WeaponData weapon = null)
    {
        CurrentClass = classData;
        EquippedWeapon = weapon;
        RecalculateDerivedStats();
        CurrentHP = Derived.MaxHP;
        CurrentEnergy = Derived.EnergyRegen;
    }

    public void RecalculateDerivedStats()
    {
        // Calculate damage from classData
        var baseStats = CurrentClass.baseStats;
        Derived = new DerivedStats
        {
            MaxHP = CurrentClass.baseHP * HPScaling * (1 + baseStats.stats[0].proficiency),
            Defense = CurrentClass.baseDefense * DefenseScaling * baseStats.stats[0].proficiency,
            CritRate = CurrentClass.baseCritRate * CritScaling * baseStats.stats[1].proficiency,
            Dodge = CurrentClass.baseDodge * DodgeScaling * baseStats.stats[1].proficiency,
            HitChance = CurrentClass.baseHitChance * HitScaling * baseStats.stats[2].proficiency,
            EnergyRegen = CurrentClass.baseEnergyRegen * EnergyScaling * baseStats.stats[2].proficiency
        };
    }
}
