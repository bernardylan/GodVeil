using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

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

    [Header("Aggro")]
    public Dictionary<CombatUnit, float> aggroTable = new();
    public float AggroModifier = 1f;
    public CombatUnit forcedTarget;
    private float forcedTargetTimer;

    public Dictionary<CombatUnit, float> damageDealt = new();

    protected virtual void Awake()
    {
        hpComponent = GetComponent<HpComponent>();
    }

    protected virtual void Update()
    {
        if (hpComponent.HP <= 0f) return;
        UpdateATB();
        TryPerformAction();
        UpdateForcedTarget(Time.deltaTime);
        UpdateAggroDecay(Time.deltaTime);
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

    public void GenerateAggro(CombatUnit attacker, float amount)
    {
        if (attacker == null || attacker == this) return;

        if (!aggroTable.ContainsKey(attacker))
            aggroTable[attacker] = 0f;

        aggroTable[attacker] += amount * attacker.AggroModifier;
    }

    public void UpdateForcedTarget(float deltaTime)
    {
        if (forcedTargetTimer > 0)
        {
            forcedTargetTimer -= deltaTime;
            if (forcedTargetTimer <= 0)
                forcedTarget = null;
        }
    }

    private void UpdateAggroDecay(float deltaTime)
    {
        var toRemove = new List<CombatUnit>();

        foreach (var kvp in aggroTable.ToList())
        {
            if (kvp.Key == null || kvp.Key.hpComponent.HP <= 0)
            {
                toRemove.Add(kvp.Key);
                continue;
            }

            // Décroit 2 % par seconde
            aggroTable[kvp.Key] = Mathf.Max(0, kvp.Value - deltaTime * 0.02f * kvp.Value);
        }

        foreach (var unit in toRemove)
            aggroTable.Remove(unit);
    }

    public void ApplyTaunt(CombatUnit source, float duration)
    {
        forcedTarget = source;
        forcedTargetTimer = duration;
    }

    public float GetAggroTowards(CombatUnit source)
    {
        if (aggroTable.TryGetValue(source, out float value))
            return value;

        return 0f;
    }

    protected virtual CombatUnit FindTarget()
    {
        if (CombatManager.Instance == null) return null;

        // taunt
        if(forcedTarget != null && forcedTarget.hpComponent.HP > 0)
            return forcedTarget;

        //Get opposite team enemy
        List<CombatUnit> enemies = isEnemy 
        ? CombatManager.Instance.playerUnits.Cast<CombatUnit>().ToList()
        : CombatManager.Instance.enemyUnits.Cast<CombatUnit>().ToList();

        // Get the first enemy on aggro table range
        if (aggroTable.Count > 0)
        {
            CombatUnit topAggro = aggroTable
                .Where(kvp => kvp.Key.hpComponent.HP > 0)
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .FirstOrDefault();

            if (topAggro != null)
                return topAggro;
        }

        // Target randomly an enemy
        return enemies
            .Where(e => e.hpComponent.HP > 0)
            .OrderBy(_ => UnityEngine.Random.value)
            .FirstOrDefault();
    }

    /// <summary>
    /// Apply damage to a target
    /// </summary>
    public void DealDamage(CombatUnit target, float skillMultiplier = 1f, float buffs = 0f, float debuffs = 0f, float passives = 0f)
    {
        if (target == null || target.hpComponent == null) return;

        DamageInfo dmg = ComputeDamage(target, skillMultiplier, buffs, debuffs, passives);
        DamageInfo final = new DamageInfo(dmg.Amount, this.gameObject, dmg.IsCrit);

        float applied = target.hpComponent.TakeDamage(dmg);

        if (!damageDealt.ContainsKey(target))
              damageDealt[target] = 0f;

        damageDealt[target] += applied;

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
