using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private ResourcesSO resource;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        // Update resources
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged += UpdateDisplay;
        else
            Debug.LogError("ResourceManager.Instance is null! Make sure a ResourceManager exists in the scene.");
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(ResourcesSO res, float amount)
    {
        if (text == null) return;
        if (res == null) return;
        if (res == resource)
            text.text = $"{amount}";
    }
}
