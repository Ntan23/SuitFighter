using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHintUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToChange;
	private GameManager gm;

	void Start() => gm = GameManager.instance;

    public void ShowDialogue(string text) => LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseSpring().setOnComplete(() => StartCoroutine(TypeSentence(text)));
		
    public void HideDialogue() 
	{
		gm.ChangeIsDialogueShowedValue();
		LeanTween.scale(gameObject, Vector3.zero, 0.6f).setEaseSpring().setOnComplete(() => textToChange.text = null);
	}

    IEnumerator TypeSentence(string sentence)
	{
		textToChange.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			textToChange.text += letter;
			yield return new WaitForSeconds(0.02f);
		}
	}
}
