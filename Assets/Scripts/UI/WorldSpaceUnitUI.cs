using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WorldSpaceUnitUI : MonoBehaviour
{
    [Header("Bindings")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider atbBar;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Vector3 offset = new(0, 2f, 0);

    private CombatUnit unit;
    private HpComponent hp;
    private Camera mainCam;
    private bool fading = false;

    public void Bind(CombatUnit u)
    {
        unit = u;
        hp = unit.GetComponent<HpComponent>();

        nameText.text = unit switch
        {
            PlayerUnit p => p.characterStats.CurrentClass.className,
            EnemyUnit e => e.enemyData.enemyNameKey,
            _ => "???"
        };

        hp.OnHPChanged += UpdateHP;
        hp.OnDied += OnDeath;

        mainCam = Camera.main;
        canvasGroup.alpha = 0f;
        StartCoroutine(Fade(0f, 1f, 0.4f));
    }

    private void LateUpdate()
    {
        if (unit == null || mainCam == null) return;

        transform.position = unit.transform.position + offset;
        transform.LookAt(mainCam.transform);
        hpBar.value = hp.GetHPPercent();
        atbBar.value = unit.ATB / 100f;
    }

    private void UpdateHP(float newHP)
    {
        hpBar.value = newHP / hp.MaxHP;
    }

    private void OnDeath(GameObject killer)
    {
        if (fading) return;
        fading = true;
        StartCoroutine(Fade(1f, 0f, 0.5f, () => Destroy(gameObject)));
    }

    private IEnumerator Fade(float from, float to, float duration, System.Action onEnd = null)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
        onEnd?.Invoke();
    }
}
