using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using System.Collections;

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
    /// Get the translation from a key
    /// </summary>
    /// <param name="key">key in the table (ex : "Villager_AutoAttack_Name")</param>
    /// <param name="tableName">Name of the table (ex : "Skills", "Classes", "Passives")</param>
    /// <returns>translation text</returns>
    public static string GetLocalizedString(string key, string tableName)
    {
        if (LocalizationSettings.StringDatabase == null)
        {
            Debug.LogWarning("Localization StringDatabase is null");
            return key;
        }

        string result = LocalizationSettings.StringDatabase.GetLocalizedString(tableName, key);

        if (string.IsNullOrEmpty(result))
        {
            Debug.LogWarning($"No translation found for '{key}' in table '{tableName}'");
            return key;
        }
        return result;
    }
}
