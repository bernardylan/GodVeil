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

        if (tier == 1)
        {
            state.chosenT1 = chosen;
            // build next pool from chosen.nextTierClasses (T2)
            var next = chosen.nextTierClasses != null ? chosen.nextTierClasses.ToList() : new List<ClassData>();
            state.pool = next.Where(c => !state.banned.Contains(c)).ToList();
        }
        else if (tier == 2)
        {
            state.chosenT2 = chosen;
            var next = chosen.nextTierClasses != null ? chosen.nextTierClasses.ToList() : new List<ClassData>();
            state.pool = next.Where(c => !state.banned.Contains(c)).ToList();
        }
        else if (tier == 3)
        {
            state.chosenT3 = chosen;
            state.pool.Clear(); // final tier
        }
    }

    /// <summary>
    /// Ban a class for a specific character (adds to banned list and removes from pool)
    /// </summary>
    public static void BanClassForCharacter(CharacterStats characterStats, ClassData classToBan)
    {
        if (characterStats == null || classToBan == null) return;
        var state = characterStats.classState ?? new CharacterClassState();
        if (!state.banned.Contains(classToBan))
            state.banned.Add(classToBan);

        if (state.pool.Contains(classToBan))
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
