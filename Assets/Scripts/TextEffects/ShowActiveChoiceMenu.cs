using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowActiveChoiceMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Image activeTextDesign;

    private void Awake()
    {
        activeTextDesign = GetComponentInChildren<Image>(true);

        if(activeTextDesign != null)
            activeTextDesign.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowImage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideImage();
    }
    public void OnSelect(BaseEventData eventData)
    {
        ShowImage();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HideImage();
    }

    private void ShowImage()
    {
        if(activeTextDesign != null)
            activeTextDesign.gameObject.SetActive(true);
    }

    private void HideImage()
    {
        if(activeTextDesign != null)
            activeTextDesign.gameObject.SetActive(false);
    }
}
