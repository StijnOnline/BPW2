using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerStats.stats.health = PlayerStats.stats.maxHealth;
            GameManager.GM.player.GetComponent<Player>().SetLights();
        }
    }
}
