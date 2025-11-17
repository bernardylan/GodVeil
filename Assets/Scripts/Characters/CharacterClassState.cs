using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Per-character state for class evolution : pool, banned classes and current picks per tier.
/// Serializable so you can inspect in Inspector if needed.
/// </summary>
[Serializable]
public class CharacterClassState
{
    public TierType currentTier = TierType.T1;                // nouveau : tier courant
    public List<ClassData> pool = new List<ClassData>();       // Available classes for the current tier
    public List<ClassData> banned = new List<ClassData>();     // Banned classes (per character)
    public ClassData chosenT1;
    public ClassData chosenT2;
    public ClassData chosenT3;

    public CharacterClassState() { }

    public CharacterClassState(IEnumerable<ClassData> initialPool)
    {
        if (initialPool != null)
            pool = new List<ClassData>(initialPool);
        currentTier = TierType.T1;
    }

    public void ClearPool() => pool.Clear();
    public void ClearBans() => banned.Clear();
}
