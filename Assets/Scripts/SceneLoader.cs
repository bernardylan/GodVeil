using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SceneLoad(string sceneName)
    {
        Debug.Log($"Let's go to {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
