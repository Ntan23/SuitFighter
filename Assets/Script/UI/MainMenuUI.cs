using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private GameObject gameIcon;
    [SerializeField] private GameObject mainMenuButtonParent;
    [SerializeField] private Button[] buttons;
    
    void Start() => StartCoroutine(StartAnimation());

    public void GoToNextScene() => StartCoroutine(Animation("NextScene"));

    public void Quit() 
    {
        Debug.Log("Quit");
        StartCoroutine(Animation("Quit"));
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.moveLocalY(gameIcon, 317.0f, 0.5f).setEaseSpring().setOnComplete(() =>
        {
            LeanTween.moveLocalY(mainMenuButtonParent, -157.0f, 0.5f).setEaseSpring().setOnComplete(() =>
            {
                LeanTween.scale(gameIcon, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setLoopPingPong();

                foreach(Button btn in buttons)
                {
                    btn.interactable = true;
                }
            });
        });
        // LeanTween.moveLocalY(buttons[0].gameObject, 477.0f, 0.5f).setEaseSpring().setOnComplete(() => 
        // {
            
        // });
    }

    IEnumerator Animation(string value)
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1.0f);
        if(value == "NextScene") SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        if(value == "Quit") Application.Quit();
    }
}
