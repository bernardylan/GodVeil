using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // Refactor the array into a dictionary eventually
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    // Singleton pattern
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Helper function to find a clip and play
    private void PlaySound(Sound[] soundArray, AudioSource source, string name, bool useOneShot = false)
    {

        if(source == null)
        {
            Debug.LogWarning("Music Source is not found!");
            return;
        }

        Sound sound = Array.Find(soundArray, s => s.soundName == name);

        if(sound == null)
        {
            Debug.LogWarning($"Sound '{name}' not found!");
            return;
        }

        if (useOneShot)
            source.PlayOneShot(sound.clip);
        else
        {
            source.clip = sound.clip;
            source.Play();
        }
    }


    //==================
    // RELATED TO MUSICS
    //==================

    //Mute or unmute the music source
    public void ToggleMusic() => musicSource.mute = !musicSource.mute;

    public void MusicVolume(float volume) => musicSource.volume = volume;

    // Play music without any effects(fade etc.)
    public void PlayMusic(string name) => PlaySound(musicSounds, musicSource, name);

    public void PlayMusicWithFade(string name, float fadeDuration = 1f)
    {
        Sound musicSound = Array.Find(musicSounds, s => s.soundName == name);
        
        if(musicSound == null)
        {
            Debug.LogWarning($"Music {name} does not exist");
            return;
        }

        StartCoroutine(FadeMusicCo(musicSound.clip, fadeDuration));
    }

    private IEnumerator FadeMusicCo(AudioClip newClip, float duration)
    {
        float targetVolume = musicSource.volume; // the volume you want at the end
        float currentVolume = targetVolume;

        // Fade out current music (if something is already playing
        if (musicSource.isPlaying)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(currentVolume, 0f, t / duration);
                yield return null;
            }
        }

        musicSource.clip = newClip;
        musicSource.volume = 0f;
        musicSource.Play();

        // Fade in
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }


    //=========================
    // RELATED TO SOUND EFFECTS
    //=========================

    // Mute or unmute the SFX source
    public void ToggleSFX() => sfxSource.mute = !sfxSource.mute;

    public void SFXVolume(float volume) => sfxSource.volume = volume;

    // Play sounds for the SFX source
    public void PlaySFX(string name, float pitch = 1f)
    {
        Sound sfxSound = Array.Find(sfxSounds, s => s.soundName == name);

        if(sfxSound == null)
        {
            Debug.LogWarning($"Sound '{name}' not found!");
            return;
        }

        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(sfxSound.clip);
        sfxSource.pitch = 1f;
    }
}
