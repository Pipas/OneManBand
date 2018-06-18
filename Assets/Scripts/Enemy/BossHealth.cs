using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {
    public GameObject enemy;
    public KillEnemy killEnemy;
    
    public int startHearts;
    public int currHealth;

    private int healthPerHeart = 1;

    public Image[] healthImages;
    public Sprite[] healthSprites;

    void Start() {
        currHealth = startHearts * healthPerHeart;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        bool empty = false;
        int i = 0;

        foreach (Image image in healthImages)
        {
            if (empty)
            {
                image.sprite = healthSprites[0];
            }
            else
            {
                i++;
                if (currHealth >= i * healthPerHeart)
                {
                    image.sprite = healthSprites[healthSprites.Length - 1];
                }
                else
                {
                    int currentHeartHealth = (int)(healthPerHeart - (healthPerHeart * i - currHealth));
                    int healthPerImage = healthPerHeart / (healthSprites.Length - 1);
                    int imageIndex = currentHeartHealth / healthPerImage;

                    image.sprite = healthSprites[imageIndex];
                    image.GetComponent<Image>().color = new Color32(255, 90, 90, 100);
                    empty = true;
                }
            }
        }

        checkIfBossKilled();


        // nao mexas aqui :p
        /*if (currHealth != startHearts * healthPerHeart && currHealth % healthPerHeart == 0)
        {
            transform.Find("EvilPiano").GetComponent<BossMelody>().NextStage();
        }*/    
    }

    public void checkIfBossKilled()
    {
        if (currHealth == 0)
        {
            killEnemy.setEnemyDead(true);
            Destroy(enemy);
        }
    }
    public void TakeDamage(int amount)
    {
        if (currHealth > 0)
        {
            currHealth += amount;
            currHealth = Mathf.Clamp(currHealth, 0, startHearts * healthPerHeart);
            UpdateHearts();
        }
    }

}
