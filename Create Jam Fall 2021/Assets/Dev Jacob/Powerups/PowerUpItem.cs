using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpItem : MonoBehaviour
{
    public GameObject popUpWindow;
    public Image popUpImage;
    public TMP_Text description;
    public TMP_Text puName;

    public PowerUp[] powerUps;
    private PowerUp powerUp;

    SpriteRenderer sr;

    PlayerController pc;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        int rndNum = (int)Random.Range(0, powerUps.Length);
        powerUp = powerUps[rndNum];

        sr.sprite = powerUp.art;
        popUpImage.sprite = powerUp.art;
        description.text = powerUp.description;
        puName.text = powerUp.name;

        popUpWindow.SetActive(false);
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (popUpWindow.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            switch (powerUp.name)
            {
                case "Power Rune":
                    pc.damage += 0.5f;
                    break;
                case "Haste Rune":
                    pc.timeBeforeHammerBack *= .8f ;
                    break;
                case "Movement Rune":
                    pc.moveSpeed += 2.5f;
                    break;
                case "Range Rune":
                    pc.shootDistance += 1f;
                    break;
                case "Health Rune":
                    pc.startHealth += 25f;
                    pc.health += 25f;
                    pc.healthText.text = "Health: " + pc.health;
                    break;
                case "Big Hammer Rune":
                    pc.hammerRadius += .35f;
                    break;

            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            popUpWindow.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            popUpWindow.SetActive(false);
        }
    }
}
