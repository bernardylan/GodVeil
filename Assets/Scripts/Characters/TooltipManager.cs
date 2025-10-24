using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private GameObject tooltipPanel; // Panel for the text
    [SerializeField] private TextMeshProUGUI tooltipText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Hide();
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// Show the tooltip with text at mouse position
    /// </summary>
    public void Show(string text, Vector3 position)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = text;
        tooltipPanel.transform.position = position;
    }

    /// <summary>
    /// Hide the tooltip
    /// </summary>
    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
