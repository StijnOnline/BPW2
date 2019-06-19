using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameManager.UpgradeType type;
     
    

    void SelectType()
    {
        while (GameManager.GM.collectedUpgrades.Contains(type))
        {
            type = (GameManager.UpgradeType)Random.Range(0, 7);
        }         
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }
    
}

