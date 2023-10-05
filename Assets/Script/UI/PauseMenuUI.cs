using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    private bool isOpen;
    private bool canPause = true;
    private GameManager gm;
    [SerializeField] private Fade fade;
    [SerializeField] private Button[] buttons;

    void Start() => gm = GameManager.instance;

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenPauseMenu() 
    {
        if(canPause)
        {
            buttons[2].interactable = false;
            canPause = false;
            CanInteract(false);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.3f).setOnComplete(() =>
            {
                gm.canPlay = false;
                Time.timeScale = 0;
                isOpen = true;
                canPause = true;
                CanInteract(true);
            });
        }
    }

    public void ClosePauseMenu() 
    {
        if(canPause)
        {
            CanInteract(false);
            Time.timeScale = 1.0f;
            canPause = false;
            LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.3f).setOnComplete(() => 
            {
                gm.canPlay = true;
                isOpen = false;
                canPause = true;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                buttons[2].interactable = true;
            });
        }
    }
  
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isOpen && gm.canPlay && !gm.isEnded && canPause) OpenPauseMenu();
        if(Input.GetKeyDown(KeyCode.Escape) && isOpen && canPause) ClosePauseMenu();
    }

    private void CanInteract(bool value)
    {
        foreach(Button btn in buttons)
        {
            btn.interactable = value;
        }
    }

    public void GoToMainMenu()
    {
        StartCoroutine(GoToMainMenuAnimation());
    }

    IEnumerator GoToMainMenuAnimation()
    {
        Time.timeScale = 1.0f;
        fade.FadeOut();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
