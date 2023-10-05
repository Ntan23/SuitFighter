using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCountUI : MonoBehaviour
{
    private enum Type{
        Player, Enemy
    }

    [SerializeField] private Type healthType;

    private int decreasedLive;
    [SerializeField] private Image[] lives;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.instance;

        if(healthType == Type.Player) UpdateLivesCount();
    }

    public void DecreaseHealth()
    {
        if(healthType == Type.Player)
        {
            if(gm.GetHealth() == 0) return;

            gm.DecreaseHealth(1);
            lives[gm.GetHealth()].gameObject.SetActive(false);
        }

        if(healthType == Type.Enemy)
        {
            if(gm.GetEnemyHealth() == 0) return;

            gm.DecreaseEnemyHealth(1);
            lives[gm.GetEnemyHealth()].gameObject.SetActive(false);
        }
    }

    public void IncreaseHealth()
    {
        if(gm.GetHealth() == 0 || gm.GetHealth() == 6) return;

        lives[gm.GetHealth()].gameObject.SetActive(true);
        gm.IncreaseHealth(1);
    }

    public void UpdateLivesCount()
    {
        decreasedLive = 6 - gm.GetHealth();

        for(int i = 0; i < decreasedLive; i++)
        {
            lives[6 - (i+1)].gameObject.SetActive(false);
        }
    }
}
