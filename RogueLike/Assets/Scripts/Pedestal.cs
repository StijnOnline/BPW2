using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pedestal : MonoBehaviour
{
    public GameManager.UpgradeType type;
    public TextMeshPro text;

    public GameObject otherChest;
    public bool locked = false;

    AudioSource audioSource;

    
    void Start() 
    {
        SelectType();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 toPlayer = GameManager.GM.player.transform.position - transform.position;
        if (!locked)
        {
            if (toPlayer.magnitude < 6f)
            {
                if (GameManager.GM.collectedUpgrades.Contains(type)) { SelectType(); } // double check if player has upgrade
                transform.GetChild(0).gameObject.SetActive(true); //enable light
                transform.GetChild(1).gameObject.SetActive(true); //enable text
                otherChest.transform.GetChild(0).gameObject.SetActive(true); //enable light
                otherChest.transform.GetChild(1).gameObject.SetActive(true); //enable text
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false); //disable light
                transform.GetChild(1).gameObject.SetActive(false); //disable text
                otherChest.transform.GetChild(0).gameObject.SetActive(false); //disable light
                otherChest.transform.GetChild(1).gameObject.SetActive(false); //disable text
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !locked)
        {
            audioSource.PlayOneShot(GameManager.GM.PedestalActivateAudio);

            other.GetComponent<Player>().Upgrade(type);
            text.SetText("Obtained\n"+type.ToString());
            locked = true;
            GetComponent<Animator>().SetBool("Broken", true);

            otherChest.transform.GetChild(0).gameObject.SetActive(false); //disable light
            otherChest.transform.GetChild(1).gameObject.SetActive(false); //disable text
            otherChest.GetComponent<Pedestal>().locked = true;
            otherChest.GetComponent<Animator>().SetBool("Broken",true);
        }
    }

    void SelectType()
    {
        type = (GameManager.UpgradeType)Random.Range(0, System.Enum.GetValues(typeof(GameManager.UpgradeType)).Length);
        if (GameManager.GM.collectedUpgrades.Contains(type)) {SelectType(); }
        else { text.SetText("Choose\n" + type.ToString());}
    }

}

