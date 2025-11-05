using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
    [Header("Full Screen")]
    private Toggle fullscreenToggle;
    [SerializeField] private bool isFullscreen;

    [Header("Resolutions")]
    Resolution[] AllResolutions;
    [SerializeField] int selectedResolution;

    private void Start()
    {
        isFullscreen = true;
    }

    public void ChangeFullScreen()
    {
        isFullscreen = fullscreenToggle.isOn;

        if (isFullscreen)
        {
            Resolution res = Screen.currentResolution;
            Screen.SetResolution(res.width, res.height, true);
        }
        else
        {
            Screen.SetResolution(1280, 760, false);
        }
    }
}
