using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFX : MonoBehaviour
{
    private GameManager gm;

    void Start() => gm = GameManager.instance;

    public void CardHoverFX(Sprite sprite)
    {
        if((gm != null && !gm.isBattling && !gm.isEnded && gm.canPlay) || gm == null) 
        {
            LeanTween.scale(gameObject,  new Vector3(1.2f, 1.2f, 1.2f), 0.3f);
            GetComponent<Image>().sprite = sprite;
        }
    }
    
    public void CardUnhoverFX(Sprite sprite)
    {
        if((gm != null && !gm.isBattling && !gm.isEnded && gm.canPlay) || gm == null) 
        {
            LeanTween.scale(gameObject, Vector3.one, 0.3f);
            GetComponent<Image>().sprite = sprite;
        }
    }
    
    public void OtherHoverFX() 
    {
        if((gm != null && !gm.isBattling) || gm == null) LeanTween.scale(gameObject,  new Vector3(1.2f, 1.2f, 1.2f), 0.3f);
    }
    
    public void OtherUnhoverFX() 
    {
        if((gm != null && !gm.isBattling) || gm == null) LeanTween.scale(gameObject, Vector3.one, 0.3f);
    }
           
}
