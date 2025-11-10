using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneShortcut
{
    [MenuItem("Scenes/1. Main Title %#1")] // Ctrl+Shift+1
    private static void OpenMainMenu()
    {
        // Save the scene
        EditorSceneManager.SaveOpenScenes();
        // Load the scene
        EditorSceneManager.OpenScene("Assets/Scenes/MainTitle.unity");
    }

    [MenuItem("Scenes/2. Village Hug Scene %#2")] // Ctrl+Shift+2
    private static void VillageHubScene()
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene("Assets/Scenes/VillageHub_Scene.unity");
    }
}
