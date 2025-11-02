using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class HpComponent : MonoBehaviour, IHasHp
{
    [SerializeField] private int teamID = 0;
    [Range(0f, 0.95f)][SerializeField] private float mitigation = 0f;

    private float currentHP;
    private float maxHP;

    public float HP => currentHP;
    public float MaxHP => maxHP;
    public int TeamID => teamID;

    public event Action<float> OnHPChanged;
    public event Action<GameObject> OnDied;

    public void InitializeFromStats(CharacterStats stats)
    {
        maxHP = stats.Derived.MaxHP;
        mitigation = stats.Derived.Defense;
        currentHP = maxHP;
        OnHPChanged?.Invoke(currentHP);
    }

    public float TakeDamage(in DamageInfo dmg)
    {
        if (currentHP <= 0f) return 0f;

        float afterMitigation = dmg.Amount * (1f - Mathf.Clamp01(mitigation));
        currentHP = Mathf.Max(0f, currentHP - afterMitigation);

        OnHPChanged?.Invoke(currentHP);

        if (currentHP <= 0f) HandleDeath(dmg.Attacker);

        return afterMitigation;
    }

    public void Heal(float amount)
    {
        if (amount <= 0f) return;
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHPChanged?.Invoke(currentHP);
    }

    private void HandleDeath(GameObject killer)
    {
        OnDied?.Invoke(killer);
        if (TryGetComponent(out Collider2D col))
            col.enabled = false;
    }

    public float GetHPPercent() => maxHP <= 0f ? 0f : currentHP / maxHP;
}


public readonly struct DamageInfo
{
    public readonly float Amount;
    public readonly GameObject Attacker;
    public readonly bool IsCrit;

    public DamageInfo(float amount, GameObject attacker, bool isCrit = false)
    {
        Amount = amount;
        Attacker = attacker;
        IsCrit = isCrit;
    }
}

public interface IHasHp
{
    float HP { get; }
    float MaxHP { get; }
    int TeamID { get; }

    float TakeDamage(in DamageInfo dmg);
    void Heal(float amount);

    event Action<float> OnHPChanged;
    event Action<GameObject> OnDied;
}
