using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayUI : MonoBehaviour
{
    [SerializeField] private GameObject[] howToPlay;
    [SerializeField] private Fade fade;

    public void Next(int index)
    {
        LeanTween.moveLocalX(howToPlay[index], -1830.0f, 0.5f).setEaseSpring();
        LeanTween.moveLocalX(howToPlay[index + 1], 0.0f, 0.5f).setEaseSpring();
    }

    public void Previous(int index)
    {
        LeanTween.moveLocalX(howToPlay[index], 1830.0f, 0.5f).setEaseSpring();
        LeanTween.moveLocalX(howToPlay[index - 1], 0.0f, 0.5f).setEaseSpring();
    }

    public void GoToNextScene() => StartCoroutine(ChangeSceneAnimation("NextScene"));

    public void GoToMainMenu() => StartCoroutine(ChangeSceneAnimation("Back"));
    
    IEnumerator ChangeSceneAnimation(string value)
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1.0f);
        if(value == "NextScene") SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + PlayerPrefs.GetInt("LevelReached", 1));
        if(value == "Back") SceneManager.LoadSceneAsync("MainMenu");
    }
}
