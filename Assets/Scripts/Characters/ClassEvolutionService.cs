using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// Helper service that operates on CharacterStats and CharacterClassState to:
/// - initialize per-character pool
/// - get valid classes (requirements + not banned)
/// - pick a class and advance pool to next tier
/// - ban classes for a specific character
/// Keep logic centralized to avoid duplication.
/// </summary>
public static class ClassEvolutionService
{
    /// <summary>
    /// Initialize character class state with a base pool (typically the tier1 classes).
    /// Call this once when you create the character (CharacterManager).
    /// </summary>
    public static void InitState(CharacterStats characterStats, IEnumerable<ClassData> basePool)
    {
        if (characterStats == null) return;
        characterStats.classState = new CharacterClassState(basePool);
    }

    /// <summary>
    /// Returns classes from the state's pool that:
    /// - are not banned for that character
    /// - satisfy the class' requirements (Uses CharacterStats.GetProficiency)
    /// </summary>
    public static List<ClassData> GetValidClassesForCharacter(CharacterStats characterStats)
    {
        var state = characterStats.classState;
        if (state == null)
        {
            // nothing initialized => return empty (safe) or try to init with a fallback
            return new List<ClassData>();
        }

        if (state.pool == null) state.pool = new List<ClassData>(); // safe guard

        var valid = state.pool
            .Where(c => c != null && !state.banned.Contains(c) && c.MeetsRequirements(characterStats))
            .ToList();

        return valid;
    }

    /// <summary>
    /// Pick a class in current tier and replace the pool with the chosen class' nextTierClasses,
    /// filtered by banned list.
    /// tier = 1,2,3
    /// </summary>
    public static void PickClassAndAdvance(CharacterStats characterStats, ClassData chosen, int tier)
    {
        if (characterStats == null || characterStats.classState == null || chosen == null) return;

        var state = characterStats.classState;

        if (tier == (int)TierType.T1)
        {
            state.chosenT1 = chosen;
            characterStats.CurrentClass = chosen;
            characterStats.RecalculateDerivedStats();
            // Advance to T2 and use the global T2 pool
            state.currentTier = TierType.T2;
            var globalPool = CharacterManager.Instance != null ? CharacterManager.Instance.GetBasePoolForTier(TierType.T2) : new List<ClassData>();
            state.pool = globalPool.Where(c => c != null && !state.banned.Contains(c)).ToList();
        }
        else if (tier == (int)TierType.T2)
        {
            state.chosenT2 = chosen;
            // Advance to T3 and use the global T3 pool
            characterStats.CurrentClass = chosen;
            characterStats.RecalculateDerivedStats();
            state.currentTier = TierType.T3;
            var globalPool = CharacterManager.Instance != null ? CharacterManager.Instance.GetBasePoolForTier(TierType.T3) : new List<ClassData>();
            state.pool = globalPool.Where(c => c != null && !state.banned.Contains(c)).ToList();
        }
        else if (tier == (int)TierType.T3)
        {
            state.chosenT3 = chosen;
            characterStats.CurrentClass = chosen;
            characterStats.RecalculateDerivedStats();
            state.pool.Clear(); // final tier - no next pool
            // Optionally set currentTier to T3 (already)
            state.currentTier = TierType.T3;
        }
    }

    /// <summary>
    /// Ban a class for a specific character (adds to banned list and removes from pool)
    /// </summary>
    public static void BanClassForCharacter(CharacterStats characterStats, ClassData classToBan)
    {
        if (characterStats == null || classToBan == null) return;

        // Ensure the character has a classState instance
        if (characterStats.classState == null)
            characterStats.classState = new CharacterClassState();

        var state = characterStats.classState;

        if (!state.banned.Contains(classToBan))
            state.banned.Add(classToBan);

        if (state.pool != null && state.pool.Contains(classToBan))
            state.pool.Remove(classToBan);
    }


    /// <summary>
    /// Get a randomized subset of candidates (non-destructive).
    /// </summary>
    public static List<ClassData> GetRandomSubset(List<ClassData> list, int count)
    {
        List<ClassData> pool = new(list);
        List<ClassData> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }
}
