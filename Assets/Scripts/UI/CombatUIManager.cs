using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    [Header("Prefab & Container")]
    public CharacterPanel characterPanelPrefab;
    public Transform panelContainer; // content transform inside the canvas

    private readonly List<CharacterPanel> spawnedPanels = new();

    /// <summary>
    /// Initialize UI panels for players list.
    /// Call this from CombatManager after spawning player units.
    /// </summary>
    public void Initialize(List<PlayerUnit> playerUnits)
    {
        Clear();

        if (characterPanelPrefab == null || panelContainer == null)
        {
            Debug.LogWarning("[CombatUIManager] Prefab or container missing.");
            return;
        }

        foreach (var pu in playerUnits)
        {
            var panel = Instantiate(characterPanelPrefab, panelContainer);
            panel.Bind(pu);
            spawnedPanels.Add(panel);

            LayoutRebuilder.ForceRebuildLayoutImmediate(panelContainer as RectTransform);
        }
    }

    public void Clear()
    {
        for (int i = spawnedPanels.Count - 1; i >= 0; i--)
        {
            if (spawnedPanels[i] != null)
                Destroy(spawnedPanels[i].gameObject);
        }
        spawnedPanels.Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }
}
