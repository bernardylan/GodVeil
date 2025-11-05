using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChangeButton : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumePercentage;
    private int volumeSteps = 10; // Number of "parts" in the slider (0 to 10, rather than smooth)

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();

        volumeSlider.minValue = 0; // No sound at 0%
        volumeSlider.maxValue = volumeSteps; // Max sound at 10
        volumeSlider.wholeNumbers = true; // Make the steps as integers
        volumeSlider.value = 5; // Default setting

        UpdateAudioVolume();
    }

    private void UpdateAudioVolume()
    {
        if (audioSource != null)
            audioSource.volume = volumeSlider.value / (float)volumeSteps; // Update the audio source volume
    }

    public void IncreaseVolume()
    {
        volumeSlider.value = Mathf.Min(volumeSlider.value + 1, volumeSteps); // Add 1 to the slider value, to increase volume
        UpdateAudioVolume();
        UpdateVolumeText();
    }

    public void DecreaseVolume()
    {
        volumeSlider.value = Mathf.Max(volumeSlider.value - 1, 0); // Remove 1 to the slider value
        UpdateAudioVolume();
        UpdateVolumeText();
    }

    private void UpdateVolumeText()
    {
        // Calculate percentage based on steps
        int percentage = Mathf.RoundToInt(volumeSlider.value / volumeSteps * 100f);
        volumePercentage.text = $"{percentage}%";
    }
}
