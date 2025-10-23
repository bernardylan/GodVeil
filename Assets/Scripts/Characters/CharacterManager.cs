using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    private void Awake() => Instance = this;

    [SerializeField] private ClassData currentClass;
    [SerializeField] private CharacterStats currentStats;

    private void Start()
    {
        InitializeCharacter(currentClass);
    }

    public void InitializeCharacter(ClassData classData)
    {
        currentClass = classData;
        currentStats = new CharacterStats(classData.baseStats);

        Debug.Log($"Initialized class : {classData.className}");
    }

    public void EquipSkill(SkillData skill) => Debug.Log($"Equipped {skill.skillName}");
    public void ApplyPassive(PassiveData passive) => Debug.Log($"Applied passive {passive.passiveName}");
}
