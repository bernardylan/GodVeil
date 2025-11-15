using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [Header("References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform tooltipPanel;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private Vector2 offset = new Vector2(20f, -20f);

    [Header("Settings")]
    [SerializeField] private float hideDelay = 0.15f;
    [SerializeField] private bool clampToScreen = true;

    private Coroutine hideCoroutine;
    private RectTransform canvasRect;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            canvasRect = canvas.GetComponent<RectTransform>();
            HideInstant();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (tooltipPanel.gameObject.activeSelf)
            FollowMouse();
    }

    public void Show(string content)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        tooltipText.text = content;
        layoutElement.enabled = (tooltipText.preferredWidth > layoutElement.preferredWidth);

        tooltipPanel.gameObject.SetActive(true);
        tooltipPanel.SetAsLastSibling();

        FollowMouse();
    }

    public void Hide()
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    public void HideInstant()
    {
        tooltipPanel.gameObject.SetActive(false);
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        tooltipPanel.gameObject.SetActive(false);
    }

    private void FollowMouse()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, canvas.worldCamera, out mousePos);

        Vector2 anchoredPos = mousePos + offset;

        if (clampToScreen)
        {
            Vector2 size = tooltipPanel.sizeDelta;
            Vector2 canvasSize = canvasRect.sizeDelta;

            anchoredPos.x = Mathf.Clamp(anchoredPos.x, -canvasSize.x / 2 + size.x / 2, canvasSize.x / 2 - size.x / 2);
            anchoredPos.y = Mathf.Clamp(anchoredPos.y, -canvasSize.y / 2 + size.y / 2, canvasSize.y / 2 - size.y / 2);
        }

        tooltipPanel.anchoredPosition = anchoredPos;
    }
}
