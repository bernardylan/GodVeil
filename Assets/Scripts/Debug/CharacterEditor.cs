using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterManager))]
public class CharacterEditor : Editor
{
    bool[] showPools;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterManager cm = (CharacterManager)target;

        if (cm.Characters.Count == 0) return;

        if (showPools == null || showPools.Length != cm.Characters.Count)
            showPools = new bool[cm.Characters.Count];

        for (int i = 0; i < cm.Characters.Count; i++)
        {
            var character = cm.Characters[i];
            if (character.classState == null) continue;

            showPools[i] = EditorGUILayout.Foldout(showPools[i], $"Character {i + 1}: {character.CurrentClass.className}");
            if (showPools[i])
            {
                EditorGUI.indentLevel++;

                // Pool
                EditorGUILayout.LabelField("Pool");
                EditorGUI.indentLevel++;
                foreach (var c in character.classState.pool)
                    EditorGUILayout.ObjectField(c.className, c, typeof(ClassData), false);
                EditorGUI.indentLevel--;

                // Banned
                EditorGUILayout.LabelField("Banned");
                EditorGUI.indentLevel++;
                foreach (var c in character.classState.banned)
                    EditorGUILayout.ObjectField(c.className, c, typeof(ClassData), false);
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
            }
        }
    }
}
