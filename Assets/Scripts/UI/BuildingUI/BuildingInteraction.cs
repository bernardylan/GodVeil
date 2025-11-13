using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private SpriteRenderer childSr;
    [SerializeField] private Color hoverColor = new Color(1f, 1f, 1f, 0.8f); // light white tint

    private Color baseColor;
    private BuildingUpgrade upgrade;

    private void Awake()
    {
        childSr = GetComponentInChildren<SpriteRenderer>();
        if (childSr == null)
            Debug.LogError("No SpriteRenderer found in children!");

        upgrade = GetComponent<BuildingUpgrade>();
        if (upgrade == null)
            upgrade = GetComponentInChildren<BuildingUpgrade>();

        if (upgrade == null)
            Debug.LogError("No BuildingUpgrade found on parent or children!");

        baseColor = childSr.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        childSr.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        childSr.color = baseColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (upgrade == null) return;

        if (!upgrade.IsBuilt)
            upgrade.TryBuild();
        else
            Debug.Log($"{upgrade.data.displayName} already built.");
    }
}
