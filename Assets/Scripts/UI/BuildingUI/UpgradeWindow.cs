using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    public static UpgradeWindow Instance { get; private set; }

    [SerializeField] private GameObject upgradeWindow;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private GameObject resourceSlotPrefab;
    [SerializeField] private Transform costContainer;
    [SerializeField] private Button buildButton;

    private BuildingUpgrade currentBuilding;

    private void Awake()
    {
        Instance = this;
        upgradeWindow.SetActive(false);
        buildButton.onClick.AddListener(OnConfirmClicked);
    }

    private void OnConfirmClicked()
    {
        if(currentBuilding == null)
        {
            Debug.LogWarning("No building selected!");
            return;
        }

        currentBuilding.TryBuild();
        upgradeWindow.SetActive(false);
    }

    public void ShowUpgradeWindow(BuildingUpgrade building)
    {
        currentBuilding = building;
        buildingName.text = building.data.displayName;

        foreach(Transform child in costContainer)
            Destroy(child.gameObject);

        ShowResourceCost(building);

        upgradeWindow.SetActive(true);
    }

    private void ShowResourceCost(BuildingUpgrade building)
    {
        int displayed = 0;

        foreach (var cost in building.data.cost)
        {
            if (displayed >= 4)
                break;

            GameObject resourceSlot = Instantiate(resourceSlotPrefab, costContainer);
            Image icon = resourceSlot.GetComponentInChildren<Image>();
            TextMeshProUGUI amountText = resourceSlot.GetComponentInChildren<TextMeshProUGUI>();

            RectTransform rt = resourceSlot.GetComponent<RectTransform>();
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;

            icon.sprite = cost.resource.icon;
            amountText.text = cost.amount.ToString();

            displayed++;
        }
    }

    public void HideUpgradeWindow()
    {
        upgradeWindow.SetActive(false);
        currentBuilding = null;
    }

    private string GetCostString(BuildingUpgrade building)
    {
        string result = "";
        foreach(var cost in building.data.cost)
        {
            if (cost.resource == null)
                continue;
            result += $"{cost.resource.displayName} : {cost.amount}\n";
        }

        return result.TrimEnd('\n');
    }
}
