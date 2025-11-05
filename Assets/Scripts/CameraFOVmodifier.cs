using UnityEngine;

public class CameraFOVmodifier : MonoBehaviour
{
    private Camera cam;
    private bool isZooming = false;
    private float startFov = 85f;
    private float targetFov = 35f;
    private float duration = 1.5f; // transition time
    private float t = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("No camera found");
            enabled = false;
            return;
        }

        cam.orthographic = false;
        cam.fieldOfView = startFov;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isZooming)
        {
            isZooming = true;
            t = 0f;
        }

        if (isZooming)
        {
            t += Time.deltaTime / duration;
            cam.fieldOfView = Mathf.Lerp(startFov, targetFov, Mathf.SmoothStep(0f, 1f, t));

            if (t >= 1f)
            {
                isZooming = false;
            }
        }
    }
}
