using UnityEngine;
using System.Collections.Generic;

public class CombatUIManager : MonoBehaviour
{
    [SerializeField] private Transform playerPanelParent;
    [SerializeField] private PlayerPanel playerPanelPrefab;

    private readonly List<PlayerPanel> activePanels = new();

    public void Initialize(List<PlayerUnit> playerUnits)
    {
        Clear();

        foreach (var unit in playerUnits)
        {
            var panel = Instantiate(playerPanelPrefab, playerPanelParent);
            panel.Bind(unit);
            activePanels.Add(panel);
        }
    }

    public void Clear()
    {
        foreach (var p in activePanels)
            Destroy(p.gameObject);
        activePanels.Clear();
    }
}
