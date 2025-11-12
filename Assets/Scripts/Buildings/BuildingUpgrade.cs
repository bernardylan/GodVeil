using Unity.VisualScripting;
using UnityEngine;

public class BuildingUpgrade : MonoBehaviour
{
    public BuildingSO data;

    [SerializeField] private bool isBuilt;

    private SpriteRenderer sr;
    [SerializeField] private Sprite builtSprite;
    [SerializeField] private Sprite destroyedSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update the visual on game start based on unlocked buildings
    private void Start()
    {
        UpdateVisual();
    }

    [ContextMenu("Upgrade")]
    public void TryBuild()
    {
        // Check if there's enough resources to build
        foreach (var cost in data.cost)
        {
            if (ResourceManager.Instance.GetAmount(cost.resource) < cost.amount)
            {
                Debug.Log($"Not enough {cost.resource.displayName}");
                return;
            }

            
        }

        // If yes, build
        foreach (var cost in data.cost)
        {
            ResourceManager.Instance.TrySpend(cost.resource, cost.amount);
        }

        isBuilt = true;
        UpdateVisual();

        Debug.Log($"{data.displayName} built!");
    }

    // Change the asset depending on destroyed or built
    private void UpdateVisual()
    {
        if (!sr) return;
        sr.sprite = isBuilt ? builtSprite : destroyedSprite;
    }
}
