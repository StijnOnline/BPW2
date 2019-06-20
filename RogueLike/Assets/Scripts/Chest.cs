using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public GameManager.UpgradeType type;
    public Sprite opened;
    public TextMeshPro text;

    void Start()
    {
        SelectType();
    }

    void SelectType()
    {
        if (GameManager.GM.collectedUpgrades.Count != System.Enum.GetValues(typeof(GameManager.UpgradeType)).Length) { 
            while (GameManager.GM.collectedUpgrades.Contains(type))
            {
                type = (GameManager.UpgradeType)Random.Range(0, System.Enum.GetValues(typeof(GameManager.UpgradeType)).Length);
            }
        }
        else
        {
            Debug.Log("Collected all upgrades");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().Upgrade(type);
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().sprite = opened;
            text.SetText("Obtained\n"+type.ToString());
            text.gameObject.SetActive(true);
        }
    }
    
}

