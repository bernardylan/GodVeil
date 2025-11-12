using UnityEngine;

public class CursorTrail : MonoBehaviour
{

    [SerializeField] private Transform mouseEffectPos;
    [SerializeField] private ParticleSystem clickEffectPos;

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseEffectPos.position = cursorPos;

        if(Input.GetMouseButtonDown(0))
        {
            clickEffectPos.transform.position = cursorPos;
            clickEffectPos.Play();
        }
    }
}
