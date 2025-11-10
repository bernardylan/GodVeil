using UnityEngine;

public struct DamageResult
{
    public float BaseDamage;
    public float ProficiencyTotal;
    public float TierMultiplier;
    public float AfterModifiers;
    public float AfterMitigation;
    public bool IsCrit;
    public float FinalDamage;
    public float CritRoll;
}

public static class DamageService
{
    public static DamageResult Calculate(
        float classSTR, float classDEX, float classINT,
        float weaponSTR, float weaponDEX, float weaponINT,
        float baseDamage,
        float tierMultiplier,
        float globalMultiplier = 1f,
        float targetMitigation = 0f,
        float critChance = -1f,
        float critMultiplier = 0.5f,
        bool rollCrit = true)
    {
        classSTR = Mathf.Clamp01(classSTR);
        classDEX = Mathf.Clamp01(classDEX);
        classINT = Mathf.Clamp01(classINT);
        weaponSTR = Mathf.Clamp01(weaponSTR);
        weaponDEX = Mathf.Clamp01(weaponDEX);
        weaponINT = Mathf.Clamp01(weaponINT);
        targetMitigation = Mathf.Clamp01(targetMitigation);

        float strSum = classSTR * weaponSTR;
        float dexSum = classDEX * weaponDEX;
        float intSum = classINT * weaponINT;
        float proficiencyTotal = strSum + dexSum + intSum;

        float preModifier = proficiencyTotal * tierMultiplier;
        float afterModifiers = baseDamage * preModifier * globalMultiplier;
        float afterMitigation = afterModifiers * (1f - targetMitigation);

        float roll = 0f;
        bool isCrit = false;
        float final = afterMitigation;

        if (critChance > 0f)
        {
            roll = rollCrit ? Random.value : critChance;
            isCrit = roll <= critChance;
            if (isCrit) final *= (1f + critMultiplier);
        }

        return new DamageResult
        {
            BaseDamage = baseDamage,
            ProficiencyTotal = proficiencyTotal,
            TierMultiplier = tierMultiplier,
            AfterModifiers = afterModifiers,
            AfterMitigation = afterMitigation,
            IsCrit = isCrit,
            FinalDamage = final,
            CritRoll = roll
        };
    }
}
