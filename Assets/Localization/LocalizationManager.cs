using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// Return the translation of a key (from localization package)
    /// </summary>
    public static string GetLocalizedString(string key)
    {
        // Search the key in the reference table of the package
        if (LocalizationSettings.StringDatabase != null)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(key);
        }

        Debug.LogWarning($"Didn't find the key : {key}");
        return key;
    }
}
