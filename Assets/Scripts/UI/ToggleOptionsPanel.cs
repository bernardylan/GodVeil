using System.Reflection;
using UnityEngine;

public class ToggleOptionsPanel : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject mainMenuUI;

    private CanvasGroup mainMenuCanvasGroup;

    private void Start()
    {
        if(mainMenuUI != null)
        {
            mainMenuCanvasGroup = mainMenuUI.GetComponent<CanvasGroup>();
            if(mainMenuCanvasGroup == null)
                mainMenuCanvasGroup = mainMenuUI.AddComponent<CanvasGroup>();
        }
    }

    public void ShowOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            
            if(mainMenuCanvasGroup != null)
            {
                mainMenuCanvasGroup.interactable = false;
                mainMenuCanvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void HideOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);

            if (mainMenuCanvasGroup != null)
            {
                mainMenuCanvasGroup.interactable = true;
                mainMenuCanvasGroup.blocksRaycasts = true;
            }
        }
    }
}
