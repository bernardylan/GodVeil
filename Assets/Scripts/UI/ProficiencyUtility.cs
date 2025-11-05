using UnityEngine;

public static class ProficiencyUtility
{
    public static string GetLetterGrade(float proficiency)
    {
        return proficiency switch
        {
            >= 1.0f => "S+",
            >= 0.9f => "S",
            >= 0.8f => "A+",
            >= 0.7f => "A",
            >= 0.6f => "B+",
            >= 0.5f => "B",
            >= 0.4f => "C+",
            >= 0.3f => "C",
            >= 0.2f => "D+",
            >= 0.1f => "D",
            _ => "-"
        };
    }
}
