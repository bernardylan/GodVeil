using System.Collections;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    private Animator anim;          // Main text animation
    private Animator childAnim;     // Mirror text animation

    [SerializeField] private GameObject mainMenuUI;

    // Main text strings
    [SerializeField] private string fadeOutAnim;
    [SerializeField] private string fadeInAnim;

    // Mirror text strings
    [SerializeField] private string lowAlphaOutAnim;
    [SerializeField] private string lowAlphaInAnim;

    // Caching for optimization
    private WaitForSeconds waitTime = new WaitForSeconds(1.5f);

    private bool active = false;

    private Coroutine fadeInOutCo;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (mainMenuUI != null)
            mainMenuUI.SetActive(false);

        foreach (Animator a in GetComponentsInChildren<Animator>(true)) // Removes main text Animator, ensures only the mirror text animator is called
        {
            if (a != anim)
            {
                childAnim = a;
                break;
            }
        }

        fadeInOutCo = StartCoroutine(FadeInOutCo()); // Starts fade anim
    }

    private void Update()
    {
        FadeOnButtonPress();
    }

    // Stop all coroutines, plays fade out one last time before disabling both texts
    private void FadeOnButtonPress()
    {
        if (Input.GetKeyDown(KeyCode.H) && active == true)
        {
            //SoundManager.Instance.PlaySFX("FirstScreen");

            active = false;
            if (fadeInOutCo != null)
                StopCoroutine(fadeInOutCo);

            if (anim != null)
                anim.Play(fadeOutAnim, 0, 0f);

            if (childAnim != null)
                childAnim.Play(lowAlphaOutAnim, 0, 0f);

            StartCoroutine(DisableAfterFadeCo());
            SoundManager.Instance.PlayMusicWithFade("MainMenuTheme");
        }
    }

    // Ensures the animation plays before the texts get disabled
    private IEnumerator DisableAfterFadeCo()
    {
        yield return waitTime;

        gameObject.SetActive(false);

        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);
    }

    // Plays the animations as long as the texts exist (before pressing any key)
    private IEnumerator FadeInOutCo()
    {
        active = true;
        while (true)
        {
            PlayBothAnimations(fadeOutAnim, lowAlphaOutAnim);
            yield return waitTime;

            PlayBothAnimations(fadeInAnim, lowAlphaInAnim);
            yield return waitTime;
        }
    }

    // Helper function to ensure both animations are sync'd
    private void PlayBothAnimations(string mainAnim, string childAnim)
    {
        if (anim != null)
            anim.Play(mainAnim, 0, 0f);
        if (this.childAnim != null)
            this.childAnim.Play(childAnim, 0, 0f);
    }

    public void PlayFirstButtonSound()
    {
        SoundManager.Instance.PlaySFX("FirstScreen");
    }

    public void PlayMainMenuMusic()
    {
        SoundManager.Instance.PlayMusicWithFade("MainMenuTheme");
    }
}
