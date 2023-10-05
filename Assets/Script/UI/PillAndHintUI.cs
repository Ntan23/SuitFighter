using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PillAndHintUI : MonoBehaviour
{
    private bool canUsePill;
    private bool canUseHint = true;
    [SerializeField] private Button pillButton;
    [SerializeField] private Button hintButton;
    [SerializeField] private LivesCountUI[] livesCountUIs;
    [SerializeField] private DialogueHintUI[] dialogueHintUIs;
    private GameManager gm;
    private AudioManager am;
    
    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;
    }

    public void PillEffect()
    {
        if(canUsePill && gm.GetHealth() > 0)
        {
            am.PlayGulpSFX();
            Debug.Log("Decrese Enemy Health Instantly !");
            livesCountUIs[1].DecreaseHealth();
            canUsePill = false;
            pillButton.interactable = false;
        }
    }

    public void HintEffect()
    {
        if(canUseHint && gm.GetHealth() > 1)
        {
            am.PlayHintSFX();
            canUseHint = false;
            Debug.Log("Show Hint");
            StopAllCoroutines();
            livesCountUIs[0].DecreaseHealth();

            if(gm.GetIsDialogueShowed()) HideDialogueAndShowHint();
            else if(!gm.GetIsDialogueShowed()) dialogueHintUIs[1].ShowDialogue(gm.GetHint());
            
            hintButton.interactable = false;
        }
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && canUsePill && gm.canPlay && gm.GetHealth() > 0) 
        {
            PillEffect();
        }

        if((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && canUseHint && gm.canPlay && gm.GetHealth() > 1)
        {
            HintEffect();
        }
    }

    void HideDialogueAndShowHint()
    {
        gm.ChangeIsDialogueShowedValue();
        dialogueHintUIs[0].HideDialogue();
        dialogueHintUIs[1].ShowDialogue(gm.GetHint());
    }

    public void ChangeCanUsePill() 
    {
        canUsePill = true;
        pillButton.interactable = true;
    }

    public void ChangeBackCanUseHint() 
    {
        hintButton.interactable = true;
        canUseHint = true;
    }
}
