using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider atbBar;
    [SerializeField] private Slider energyBar;

    private PlayerUnit unit;
    private HpComponent hp;

    public void Bind(PlayerUnit boundUnit)
    {
        unit = boundUnit;
        hp = unit.GetComponent<HpComponent>();
        nameText.text = unit.characterStats.CurrentClass.className;
    }

    private void Update()
    {
        if (unit == null || hp == null) return;

        hpBar.value = hp.HP / hp.MaxHP;
        atbBar.value = unit.ATB / 100f;
        energyBar.value = unit.energy / unit.maxEnergy;
    }
}
