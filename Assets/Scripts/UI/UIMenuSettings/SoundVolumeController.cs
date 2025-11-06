using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeController : MonoBehaviour
{
    public Slider MusicSlider, SfxSlider;

    public void ToggleMusic() => SoundManager.Instance?.ToggleMusic();

    public void MusicVolume() => SoundManager.Instance?.MusicVolume(MusicSlider.value);

    public void ToggleSfx() => SoundManager.Instance?.ToggleSFX();

    public void SFXVolume() => SoundManager.Instance?.SFXVolume(SfxSlider.value);
}
