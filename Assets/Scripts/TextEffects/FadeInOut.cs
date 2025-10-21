using System.Collections;
using UnityEngine;
using TMPro;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private float blinkSpeed = 2f;

    private TMP_Text tmpText;
    private float timer;
    private Coroutine blinkTextCo;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
       // blinkTextCo = StartCoroutine(BlinkTextCo(1f));
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            this.gameObject.SetActive(false);
            if(blinkTextCo != null )
                StopCoroutine(blinkTextCo);
        }
    }

    //private IEnumerator BlinkTextCo(float duration)
    //{
    //    timer = 0f;

    //    while(timer < blinkSpeed)
    //    {
    //        timer += Time.deltaTime;

    //        //float alpha 
    //    }
    //}
}
