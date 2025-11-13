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

        if (data.requiredBuilding != null)
        {
            foreach (var required in data.requiredBuilding)
            {
                if (required == null)
                {
                    Debug.LogWarning($"{data.displayName} has a null required building entry!");
                    continue; // skip null entries
                }

                if (!TownManager.Instance.IsBuilt(required))
                {
                    Debug.Log($"Cannot build {data.displayName}, missing {required.displayName}");
                    return;
                }
            }
        }

        // If yes, build
        for(int i = 0; i < Mathf.Min(4, data.cost.Length); i++)
        {
            var cost = data.cost[i];
            ResourceManager.Instance.TrySpend(cost.resource, cost.amount);
        }

        if(!TownManager.Instance.builtBuildings.Contains(this))
            TownManager.Instance.builtBuildings.Add(this);

        isBuilt = true;
        UpdateVisual();

        Debug.Log($"{data.displayName} built!");
    }

    public bool IsBuilt => isBuilt;

    // Change the asset depending on destroyed or built
    private void UpdateVisual()
    {
        if (!sr) return;

        Vector3 bottomWorld = sr.bounds.min;
        sr.sprite = isBuilt ? builtSprite : destroyedSprite;

        Vector3 newBottomWorld = sr.bounds.min;
        Vector3 diff = bottomWorld - newBottomWorld;
        transform.position += diff;
    }
}
