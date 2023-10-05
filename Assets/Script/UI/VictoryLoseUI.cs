using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryLoseUI : MonoBehaviour
{   
    [SerializeField] private Fade fade;
    [SerializeField] private Button[] buttons;

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;
    
    public void ShowUI()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        CanInteract(false);
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => CanInteract(true));
    }

    public void GoToNextLevel() => StartCoroutine(Animation("NextLevel"));
    public void GoToMainMenu() => StartCoroutine(Animation("MainMenu"));
    public void Retry() => StartCoroutine(Animation("Retry"));

    private void CanInteract(bool value)
    {
        foreach(Button btn in buttons)
        {
            btn.interactable = value;
        }
    }

    IEnumerator Animation(string value)
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1.0f);
        if(value == "NextLevel") SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        if(value == "MainMenu") SceneManager.LoadSceneAsync("MainMenu");
        if(value == "Retry") SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
