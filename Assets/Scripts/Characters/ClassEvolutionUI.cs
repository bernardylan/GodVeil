using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClassEvolutionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private TextMeshProUGUI proficiencyText;
    [SerializeField] private Transform skillContainer;
    [SerializeField] private GameObject skillIconPrefab;
    [SerializeField] private Button selectButton;

    private ClassData currentClass;
    private Action<ClassData> onSelect;

    public void Initialize(ClassData classData, Action<ClassData> onSelectCallback)
    {
        currentClass = classData;
        onSelect = onSelectCallback;

        // Nom de la classe
        classNameText.text = LocalizationManager.GetLocalizedString(classData.className);

        // Proficiencies
        proficiencyText.text = "";
        foreach (var s in classData.baseStats.stats)
        {
            proficiencyText.text += $"{s.type}: {s.proficiency}\n";
        }

        // Clear previous icons
        foreach (Transform t in skillContainer) Destroy(t.gameObject);

        // Passif comme un skill
        if (classData.passive != null)
        {
            AddSkillOrPassiveIcon(classData.passive.passiveNameKey, classData.passive.icon);
        }

        // Skills
        SkillData[] skills = { classData.autoAttack, classData.specialSkill, classData.ultimateSkill };
        foreach (var skill in skills)
        {
            if (skill == null) continue;
            AddSkillOrPassiveIcon(skill.skillNameKey, skill.icon);
        }

        // Bouton de sélection
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => onSelect?.Invoke(currentClass));
    }

    private void AddSkillOrPassiveIcon(string nameKey, Sprite iconSprite)
    {
        GameObject go = Instantiate(skillIconPrefab, skillContainer);
        Image icon = go.GetComponent<Image>();
        icon.sprite = iconSprite;

        // EventTrigger pour tooltip
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger) trigger = go.AddComponent<EventTrigger>();

        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((e) =>
        {
            string localizedText = LocalizationManager.GetLocalizedString(nameKey);
            TooltipManager.Instance.Show(localizedText, Input.mousePosition);
        });
        trigger.triggers.Add(entryEnter);

        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((e) => TooltipManager.Instance.Hide());
        trigger.triggers.Add(entryExit);
    }
}