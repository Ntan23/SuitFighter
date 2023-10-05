using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditText;
    [SerializeField] private Button backButton;
    [SerializeField] private float timeToReachTheEnd;

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;
    
    public void OpenCredits()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
        {
            LeanTween.moveLocalY(creditText, 1076.0f, timeToReachTheEnd).setLoopClamp();
            backButton.interactable = true;
        });
    }

    public void CloseCredits() 
    {
        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
        {
            LeanTween.cancel(creditText); 
            creditText.transform.localPosition = new Vector3(0.0f, -1070.0f, 0.0f);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            backButton.interactable = false;
        });
    }
}
