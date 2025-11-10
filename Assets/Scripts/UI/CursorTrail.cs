using UnityEngine;

public class CursorTrail : MonoBehaviour
{

    [SerializeField] private Transform mouseEffectPos;

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseEffectPos.position = cursorPos;
    }
}
