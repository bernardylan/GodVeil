using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    public static LocaleSelector Instance;

    private bool active = false;
    [SerializeField] private TMP_Dropdown languageDropdown;

    public List<string> localeNames = new List<string> { "English", "Français", "日本語" };

    //Singleton pattern
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Load localization tables
    void Start()
    {
        languageDropdown.ClearOptions();
        languageDropdown.AddOptions(localeNames);

        languageDropdown.onValueChanged.AddListener(ChangeLocale);

        languageDropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
    }

    // Allows the changing of the Locale
    public void ChangeLocale (int _localeID)
    {
        if (active == true)
            return;
        StartCoroutine(SetLocale(_localeID));
    }

    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false;
    }
}
