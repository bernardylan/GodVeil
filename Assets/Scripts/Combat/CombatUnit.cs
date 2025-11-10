using UnityEngine;
using System;


[RequireComponent(typeof(HpComponent))]
public class CombatUnit : MonoBehaviour
{
    [Header("Unit Data")]
    public CharacterStats characterStats;
    public bool isEnemy = false;

    [Header("Combat Settings")]
    public float atb = 0f;                 // 0 → 100
    public float atbSpeedMultiplier = 1f;  // Modifie vitesse ATB (pour buffs/debuffs)
    public float energy = 0f;              // Pour Ultimate
    public float maxEnergy = 100f;

    [Header("Skills")]
    public SkillData autoAttack;
    public SkillData specialSkill;
    public SkillData ultimateSkill;

    public event Action<CombatUnit, SkillData> OnPerformSkill;

    private HpComponent hpComponent;

    protected virtual void Awake()
    {
        hpComponent = GetComponent<HpComponent>();
        if (characterStats != null)
        {
            energy = 0f;
            atb = 0f;
            autoAttack = characterStats.CurrentClass.autoAttack;
            specialSkill = characterStats.CurrentClass.specialSkill;
            ultimateSkill = characterStats.CurrentClass.ultimateSkill;
        }
    }

    private void Update()
    {
        if (hpComponent.HP <= 0f) return;

        UpdateATB();
        TryPerformAction();
    }

    private void UpdateATB()
    {
        float speed = characterStats.Derived.Speed;
        atb += Time.deltaTime * speed * atbSpeedMultiplier;

        if (atb > 100f) atb = 100f;
    }

    private void TryPerformAction()
    {
        if (atb < 100f) return;

        SkillData chosenSkill = DecideSkill();

        if (chosenSkill != null)
        {
            PerformSkill(chosenSkill);
            atb = 0f;
        }
    }

    protected virtual SkillData DecideSkill()
    {
        // Ultimate check
        if (ultimateSkill != null && energy >= maxEnergy)
        {
            return ultimateSkill;
        }

        // Special check
        if (specialSkill != null)
        {
            return specialSkill;
        }

        // Fallback auto attack
        return autoAttack;
    }

    private void PerformSkill(SkillData skill)
    {
        // Placeholder : déclenche event pour CombatManager ou système d'effet
        OnPerformSkill?.Invoke(this, skill);

        // Gérer l'énergie pour Ultimate
        if (skill == ultimateSkill)
        {
            energy = 0f;
        }
        else
        {
            energy = Mathf.Min(maxEnergy, energy + 10f); // Exemple : +10 énergie par action
        }

        // Debug
        Debug.Log($"{(isEnemy ? "Enemy" : "Player")} {characterStats.CurrentClass.className} uses {skill.skillNameKey}");
    }

    public void GainEnergy(float amount)
    {
        energy = Mathf.Min(maxEnergy, energy + amount);
    }
}
