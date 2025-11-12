using UnityEngine;
using UnityEngine.EventSystems;

// Make a button bigger or smaller based on mouse for hover effect
public class ShowActiveChoiceMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Size")]
    [SerializeField] private Vector2 inactiveScale = new Vector2(1, 1);
    [SerializeField] private Vector2 activeScale = new Vector2(1.2f, 1.2f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActiveButtonChoice();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InactiveButtonChoice();
    }
    public void OnSelect(BaseEventData eventData)
    {
        ActiveButtonChoice();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        InactiveButtonChoice();
    }

    private void ActiveButtonChoice()
    {
        this.transform.localScale = activeScale;
    }

    private void InactiveButtonChoice()
    {
        this.transform.localScale = inactiveScale;
    }
}
