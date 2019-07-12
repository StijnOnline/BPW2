using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public GameManager.UpgradeType type;
    public Sprite opened;
    public TextMeshPro text;

    public GameObject otherChest;
    public bool locked = false;

    void Start()
    {
        SelectType();
        text.SetText("Choose\n" + type.ToString());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !locked)
        {
            other.GetComponent<Player>().Upgrade(type);
            GetComponent<SpriteRenderer>().sprite = opened;
            text.SetText("Obtained\n"+type.ToString());
            locked = true;
            
            otherChest.transform.GetChild(0).gameObject.SetActive(false); //disable light
            otherChest.transform.GetChild(1).gameObject.SetActive(false); //disable text
            otherChest.GetComponent<Chest>().locked = true;
        }
    }

    void SelectType()
    {
        type = (GameManager.UpgradeType)Random.Range(0, System.Enum.GetValues(typeof(GameManager.UpgradeType)).Length);
        if (GameManager.GM.collectedUpgrades.Contains(type)) {SelectType(); }
    }

}

