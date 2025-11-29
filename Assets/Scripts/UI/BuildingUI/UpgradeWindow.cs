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
    [SerializeField] private ResourceDisplay[] resourceDisplays;

    private BuildingUpgrade currentBuilding;

    private void Awake()
    {
        Instance = this;
        upgradeWindow.SetActive(false);
        buildButton.onClick.AddListener(OnConfirmClicked);
    }

    private void OnResourceChanged(ResourcesSO res, float newAmount)
    {
        if (upgradeWindow.activeSelf && currentBuilding != null)
        {
            UpdateDisplayedCosts();
        }
    }

    private void OnEnable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged += OnResourceChanged;
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;
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

        if (!upgradeWindow.activeSelf)
            upgradeWindow.SetActive(true);

        // Clear old cost display
        foreach (Transform child in costContainer)
            Destroy(child.gameObject);

        // Create new cost display
        ShowResourceCost(building);
        // Don't call UpdateDisplayedCosts here - the text is already set in ShowResourceCost

        foreach (var rd in resourceDisplays)
        {
            rd.Refresh();
        }
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
            icon.sprite = cost.resource.icon;

            TextMeshProUGUI amountText = resourceSlot.GetComponentInChildren<TextMeshProUGUI>();
            float currentAmount = ResourceManager.Instance.GetAmount(cost.resource);
            bool enough = currentAmount >= cost.amount;
            string color = enough ? "#00FF00" : "#FF0000";
            amountText.text = $"<color={color}>{currentAmount} / {cost.amount}</color>";

            displayed++;
        }
    }   

    private void UpdateDisplayedCosts()
    {
        int index = 0;

        foreach (Transform child in costContainer)
        {
            if (index >= currentBuilding.data.cost.Length)
                break;

            var cost = currentBuilding.data.cost[index];
            float currentAmount = ResourceManager.Instance.GetAmount(cost.resource);
            float requiredAmount = cost.amount;

            TextMeshProUGUI amountText = child.GetComponentInChildren<TextMeshProUGUI>();

            string displayAmount = requiredAmount == 0 ? $"{currentAmount} / 0" : $"{currentAmount} / {requiredAmount}";

            bool enough = requiredAmount == 0 || currentAmount >= requiredAmount;
            string color = enough ? "#00FF00" : "#FF0000";

            amountText.text = $"<color={color}>{displayAmount}</color>";

            index++;
        }
    }

    public void HideUpgradeWindow()
    {
        currentBuilding = null;
        upgradeWindow.SetActive(false);
    }
}
