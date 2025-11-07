using UnityEngine;

public class CameraMapSettings : MonoBehaviour
{
    private Camera cam;

    [Header("FOV")]
    private bool isZooming = false;
    private float startFov = 85f;
    private float targetFov = 35f;
    private float duration = 1.5f; // transition time
    private float t = 0f;

    [Header("Headbob")]
    public float headbobAmplitude = 0.05f;
    public float tiltAmplitude = 0.05f;
    public float bobFrequency = 8f;
    private Vector3 originalPos;

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
        originalPos = cam.transform.localPosition;
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
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            float sinus = Mathf.Sin(t * bobFrequency * Mathf.PI);

            //FOV
            cam.fieldOfView = Mathf.Lerp(startFov, targetFov, easedT);

            //Headbob
            float bob = sinus * headbobAmplitude;
            cam.transform.localPosition = originalPos + new Vector3(0, bob, 0);

            //tilt
            float tilt = sinus * tiltAmplitude;
            cam.transform.localRotation = Quaternion.Euler(0, 0, tilt);

            if (t >= 1f)
            {
                isZooming = false;
                cam.transform.localPosition = originalPos;
                cam.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
