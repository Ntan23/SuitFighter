using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintBoxUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToChange;
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI nextButtonText;
    [SerializeField] private Color inactiveColor;


    public void ShowHintBoxAndUpdate(string text)
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseSpring().setOnComplete(() => StartCoroutine(TypeSentence(text)));
    }

    IEnumerator TypeSentence (string sentence)
	{
        nextButton.interactable = false;
        nextButtonText.color = inactiveColor;

		textToChange.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			textToChange.text += letter;
			yield return new WaitForSeconds(0.02f);
		}

        nextButton.interactable = true;
        nextButtonText.color = Color.white;
	}

    public void HideHintBox() => LeanTween.scale(gameObject, Vector3.zero, 1.0f).setEaseSpring();
}
