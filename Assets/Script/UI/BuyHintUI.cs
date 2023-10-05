using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHintUI : MonoBehaviour
{
    [SerializeField] private GameObject cards;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject enemyHealth;
    [SerializeField] private GameObject round;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject startUI;
    [SerializeField] private Button[] buttons;
    private GameManager gm;

    void Start() => gm = GameManager.instance;

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void HideBuyHintUI() 
    {
        foreach(Button btn in buttons)
        {
            btn.interactable = false;
        }

        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() => 
        {
            ShowHealth();
            ShowRound();
            ShowEnemyHealth();
        });
    }

    private void ShowCard() => LeanTween.moveLocalY(cards.gameObject, -377.0f, 0.8f).setEaseSpring().setOnComplete(() => gm.SaveCardInitialPosition());
    private void ShowHealth() => LeanTween.moveLocalY(health.gameObject, -443.0f, 0.5f).setEaseSpring();

    private void ShowEnemyHealth() => LeanTween.moveLocalY(enemyHealth.gameObject, 474.0f, 0.8f).setEaseSpring().setOnComplete(() =>
    {
        ShowTimer();
        ShowCard();
    });

    private void ShowRound() => LeanTween.moveLocalY(round.gameObject, 490.0f, 0.8f).setEaseSpring();
    private void ShowTimer() => LeanTween.moveLocalY(timer.gameObject, 456.0f, 0.8f).setEaseSpring().setOnComplete(() => StartCoroutine(Delay()));

    IEnumerator Delay()
    {
        LeanTween.scale(startUI, Vector3.one, 0.5f);
        yield return new WaitForSeconds(1.0f);
        LeanTween.scale(startUI, Vector3.zero, 0.5f).setOnComplete(() => 
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            gm.canPlay = true;
        });
    }
    
}
