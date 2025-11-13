using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CombatUnit), true)]
public class CombatUnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Default inspectr
        DrawDefaultInspector();

        CombatUnit unit = (CombatUnit)target;

        // separation
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("=== DEBUG AGGRO TABLE ===", EditorStyles.boldLabel);

        if (unit.aggroTable == null || unit.aggroTable.Count == 0)
        {
            EditorGUILayout.HelpBox("No aggro for this unit", MessageType.Info);
            return;
        }

        // du plus haut au plus bas (tri)
        var sorted = unit.aggroTable.OrderByDescending(kvp => kvp.Value);

        foreach (var kvp in sorted)
        {
            if (kvp.Key == null) continue;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"→ {kvp.Key.name}", EditorStyles.boldLabel);
            Color barColor = Color.Lerp(Color.green, Color.red, kvp.Value / 100f);
            GUI.color = barColor;
            EditorGUILayout.Slider("Aggro", kvp.Value, 0f, 100f);
            GUI.color = Color.white;
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("=== DEBUG DPS TABLE ===", EditorStyles.boldLabel);
        foreach (var kvp in unit.damageDealt)
        {
            if (kvp.Key != null)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"→ {kvp.Key.name}: {kvp.Value:F2}", EditorStyles.boldLabel);
                EditorGUILayout.EndVertical();

            }
        }

        // Refresh constant en mode Play
        if (Application.isPlaying)
        {
            EditorUtility.SetDirty(target);
        }
    }
}