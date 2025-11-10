using UnityEngine;
using System;

[RequireComponent(typeof(HpComponent))]
public abstract class CombatUnit : MonoBehaviour
{
    [Header("Unit Data")]
    public bool isEnemy = false;

    [Header("Combat Settings")]
    public float atb = 0f;
    public float atbSpeedMultiplier = 1f;
    public float energy = 0f;
    public float maxEnergy = 100f;

    [Header("Skills")]
    public SkillData autoAttack;
    public SkillData specialSkill;
    public SkillData ultimateSkill;

    public event Action<CombatUnit, SkillData> OnPerformSkill;

    protected HpComponent hpComponent;

    protected virtual void Awake()
    {
        hpComponent = GetComponent<HpComponent>();
    }

    protected virtual void Update()
    {
        if (hpComponent.HP <= 0f) return;

        UpdateATB();
        TryPerformAction();
    }

    private void UpdateATB()
    {
        atb += Time.deltaTime * atbSpeedMultiplier;
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
        if (ultimateSkill != null && energy >= maxEnergy)
            return ultimateSkill;
        if (specialSkill != null)
            return specialSkill;
        return autoAttack;
    }

    protected virtual void PerformSkill(SkillData skill)
    {
        OnPerformSkill?.Invoke(this, skill);

        if (skill == ultimateSkill)
            energy = 0f;
        else
            energy = Mathf.Min(maxEnergy, energy + 10f);

        Debug.Log($"{(isEnemy ? "Enemy" : "Player")} uses {skill.skillNameKey}");
    }

    public void GainEnergy(float amount)
    {
        energy = Mathf.Min(maxEnergy, energy + amount);
    }
}