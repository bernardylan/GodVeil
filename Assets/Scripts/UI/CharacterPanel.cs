using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterPanel : MonoBehaviour
{
    [Header("UI References")]
    public Image hpFill;           // Image type = Filled
    public Image atbFill;          // Image type = Filled
    public Image energyFill;       // Image type = Filled (ultimate)
    public Image portrait;
    public TMP_Text nameText;

    // Optional: skill icons, buff container...
    // public Image[] skillIcons;
    // public Transform buffContainer;

    private PlayerUnit boundUnit;
    private HpComponent boundHp;

    /// <summary>
    /// Bind a PlayerUnit to this panel. Panel will subscribe to hp events and update ATB/energy each frame.
    /// </summary>
    public void Bind(PlayerUnit unit)
    {
        if (unit == null) return;

        Unbind(); // safety

        boundUnit = unit;
        boundHp = unit.GetComponent<HpComponent>();

        // Initialize UI values
        nameText.text = LocalizationManager.GetLocalizedString(unit.characterStats.CurrentClass.className, "ClassNames");

        // portrait from class icon
        if (portrait != null && unit.characterStats?.CurrentClass?.classIcon != null)
            portrait.sprite = unit.characterStats.CurrentClass.classIcon;

        // Subscribe to HP changes
        if (boundHp != null)
            boundHp.OnHPChanged += OnHpChanged;

        // initial fill
        if (boundHp != null && hpFill != null)
            hpFill.fillAmount = boundHp.GetHPPercent();

        if (atbFill != null)
            atbFill.fillAmount = Mathf.Clamp01(unit.ATB / 100f);

        if (energyFill != null)
            energyFill.fillAmount = Mathf.Clamp01(unit.energy / unit.maxEnergy);
    }

    /// <summary>
    /// Unbind safely (called automatically on destroy).
    /// </summary>
    public void Unbind()
    {
        if (boundHp != null)
            boundHp.OnHPChanged -= OnHpChanged;

        boundUnit = null;
        boundHp = null;
    }

    private void OnDestroy()
    {
        Unbind();
    }

    // ------ Event handlers & update ------

    private void OnHpChanged(float newHp)
    {
        if (hpFill == null || boundHp == null) return;
        hpFill.fillAmount = boundHp.GetHPPercent();
    }

    private void Update()
    {
        // ATB & Energy updated every frame
        if (boundUnit != null)
        {
            if (atbFill != null)
                atbFill.fillAmount = Mathf.Clamp01(boundUnit.ATB / 100f);

            if (energyFill != null)
                energyFill.fillAmount = Mathf.Clamp01(boundUnit.energy / boundUnit.maxEnergy);
        }
    }
}
