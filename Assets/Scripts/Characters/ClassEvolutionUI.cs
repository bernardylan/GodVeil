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
    [SerializeField] private GameObject charaPrefab;
    [SerializeField] private Button selectButton;
    [SerializeField] private Transform characterSlot;

    private ClassData currentClass;
    private Action<ClassData> onSelect;

    private void Clear(Transform parent)
    {
        foreach (Transform t in parent)
            Destroy(t.gameObject);
    }

    public void Initialize(ClassData classData, Action<ClassData> onSelectCallback)
    {
        currentClass = classData;
        onSelect = onSelectCallback;
        Clear(characterSlot);
        Clear(skillContainer);

        // classe name
        classNameText.text = LocalizationManager.GetLocalizedString(classData.className, "ClassNames");

        GameObject go = Instantiate(charaPrefab, characterSlot);
        Image icon = go.GetComponent<Image>();
        icon.sprite = classData.classIcon;

        // Proficiencies
        proficiencyText.text = "";
        foreach (var s in classData.baseStats.stats)
        {
            proficiencyText.text += $"{s.type}: {ProficiencyUtility.GetLetterGrade(s.proficiency)}\n";
        }

        // add passive
        if (classData.passive != null)
        {
            AddSkillOrPassiveIcon(
                classData.passive.icon,
                classData.passive.passiveNameKey,
                classData.passive.passiveDescriptionKey,
                "Passives"
            );
        }

        // add skills
        SkillData[] skills = { classData.autoAttack, classData.specialSkill, classData.ultimateSkill };
        foreach (var skill in skills)
        {
            if (skill == null) continue;

            AddSkillOrPassiveIcon(
                skill.icon,
                skill.skillNameKey,
                skill.skillDescriptionKey,
                "Skills"
            );
        }

        // Select button
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() => onSelect?.Invoke(currentClass));
    }

    private void AddSkillOrPassiveIcon(Sprite iconSprite, string nameKey, string descriptionKey, string tableName)
    {
        GameObject go = Instantiate(skillIconPrefab, skillContainer);
        Image icon = go.GetComponent<Image>();
        icon.sprite = iconSprite;

        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger) trigger = go.AddComponent<EventTrigger>();

        // PointerEnter -> show tooltip
        var entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((e) =>
        {
            string name = LocalizationManager.GetLocalizedString(nameKey, tableName);
            string desc = LocalizationManager.GetLocalizedString(descriptionKey, tableName);
            TooltipManager.Instance.Show($"{name}\n{desc}");
        });
        trigger.triggers.Add(entryEnter);

        // PointerExit -> hide tooltip
        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((e) => TooltipManager.Instance.Hide());
        trigger.triggers.Add(entryExit);
    }
}
