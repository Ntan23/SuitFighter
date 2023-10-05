using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUIAnimation : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject textGO;
    [SerializeField] private GameObject buttonGO;

    void Start() => StartCoroutine(StartAnimation());

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1.8f);
        LeanTween.scale(image, Vector3.one, 0.8f).setEaseSpring().setOnComplete(() => LeanTween.scale(textGO, Vector3.one, 0.8f).setEaseSpring().setOnComplete(() => LeanTween.scale(buttonGO, Vector3.one, 0.8f).setEaseSpring()));
    }
}
