using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(HpComponent))]
public abstract class CombatUnit : MonoBehaviour
{
    [Header("Unit Info")]
    public bool isEnemy = false;

    [Header("Combat Stats")]
    public float ATB = 0f;
    public float ATBSpeedMultiplier = 1f;
    public float energy = 0f;
    public float maxEnergy = 100f;

    [Header("Skills")]
    public SkillData autoAttack;
    public SkillData specialSkill;
    public SkillData ultimateSkill;

    public event Action<CombatUnit, SkillData> OnPerformSkill;

    public HpComponent hpComponent;

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
        ATB += Time.deltaTime * ATBSpeedMultiplier;
        if (ATB > 100f) ATB = 100f;
    }

    private void TryPerformAction()
    {
        if (ATB < 100f) return;

        SkillData chosen = DecideSkill();
        if (chosen != null)
        {
            PerformSkill(chosen);
            ATB = 0f;
        }
    }

    protected abstract SkillData DecideSkill();

    protected virtual void PerformSkill(SkillData skill)
    {
        OnPerformSkill?.Invoke(this, skill);

        CombatUnit target = FindTarget();
        if (target != null)
        {
            DealDamage(target, skillMultiplier: skill.damageMultiplier,
                                buffs: skill.Buffs,
                                debuffs: skill.Debuffs,
                                passives: skill.Passives);
        }

        if (skill == ultimateSkill) energy = 0f;
        else energy = Mathf.Min(maxEnergy, energy + 10f);
    }

    protected virtual CombatUnit FindTarget()
    {
        if (CombatManager.Instance == null) return null;

        List<CombatUnit> allUnits = new();
        allUnits.AddRange(CombatManager.Instance.playerUnits);
        allUnits.AddRange(CombatManager.Instance.enemyUnits);

        foreach (var unit in allUnits)
        {
            if (unit == this) continue; // pas se cibler soi-même
            if (unit.hpComponent.HP <= 0f) continue; // doit être vivant
            if (unit.hpComponent.TeamID == hpComponent.TeamID) continue; // pas same team
            return unit;
        }

        return null;
    }

    /// <summary>
    /// Apply damage to a target
    /// </summary>
    public void DealDamage(CombatUnit target, float skillMultiplier = 1f, float buffs = 0f, float debuffs = 0f, float passives = 0f)
    {
        if (target == null || target.hpComponent == null) return;

        DamageInfo dmg = ComputeDamage(target, skillMultiplier, buffs, debuffs, passives);
        float applied = target.hpComponent.TakeDamage(dmg);

        Debug.Log($"{name} dealt {applied} damage to {target.name} (Crit: {dmg.IsCrit})");
    }

    /// <summary>
    /// Compute damage this unit deals to a target
    /// </summary>
    /// <param name="target">Target unit</param>
    /// <param name="skillMultiplier">Optional skill multiplier</param>
    /// <param name="buffs">Buffs percentage (0.1 = +10%)</param>
    /// <param name="debuffs">Debuffs percentage (0.2 = -20%)</param>
    /// <param name="passives">Passive bonuses percentage</param>
    /// <returns>DamageInfo ready to pass to HpComponent.TakeDamage</returns>
    public abstract DamageInfo ComputeDamage(CombatUnit target, float skillMultiplier = 1f, float buffs = 0f, float debuffs = 0f, float passives = 0f);

    public void GainEnergy(float amount) => energy = Mathf.Min(maxEnergy, energy + amount);
}
