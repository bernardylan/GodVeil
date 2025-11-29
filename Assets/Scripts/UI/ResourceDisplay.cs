using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    public ResourcesSO resource;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        // Update resources
        if (ResourceManager.Instance == null)
        {
            Debug.LogError("ResourceManager not found!");
            return;
        }

        ResourceManager.Instance.OnResourceChanged += OnResourceChanged;
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;
    }

    private void OnResourceChanged(ResourcesSO changedResource, float newAmount)
    {
        if (changedResource == resource)
            text.text = newAmount.ToString();
    }

    public void Refresh()
    {
        float current = ResourceManager.Instance.GetAmount(resource);
        text.text = current.ToString();
    }
}
