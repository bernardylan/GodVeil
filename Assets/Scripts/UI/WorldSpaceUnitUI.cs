using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class WorldSpaceUnitUI : MonoBehaviour
{
    [SerializeField] public HpComponent hpComponent;
    [SerializeField] private Image fillImage;

    public Vector3 offset;
    public Transform target;

    private void Awake()
    {
        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                canvas.worldCamera = mainCam;
        }
    }

    private void Start()
    {
            hpComponent.OnHPChanged += UpdateBar;
    }

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
    }

    private void OnDestroy()
    {
        if (hpComponent != null)
            hpComponent.OnHPChanged -= UpdateBar;
    }

    private void UpdateBar(float currentHP)
    {
        if (hpComponent.MaxHP <= 0) return;
        fillImage.fillAmount = currentHP / hpComponent.MaxHP;
    }
}
