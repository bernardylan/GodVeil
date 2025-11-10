using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private Renderer rend;
    private Color baseColor;
    private Color highlightColor = new Color(0.35f, 0.07f, 0.05f);

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
    }

    void OnMouseEnter() => Highlight(true);
    void OnMouseExit() => Highlight(false);

    void Highlight(bool active)
    {
        rend.material.color = active ? highlightColor : baseColor;
    }


    private void OnMouseDown()
    {
        SceneLoader.Instance.SceneLoad(sceneToLoad);
    }
}
