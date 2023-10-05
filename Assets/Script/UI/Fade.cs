using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    void Start() => StartCoroutine(FadeInAnimation());

    public void FadeOut() 
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 1.0f);
    }

    IEnumerator FadeInAnimation()
    {
        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 1.5f);
        yield return new WaitForSeconds(2.0f);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
