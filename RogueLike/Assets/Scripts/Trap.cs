using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float dmg = 10f;
    public float triggerDelay = 0.3f;
    float lastTrigger = 0;
    Animator an;

    void Start()
    {
        an = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && Time.time > lastTrigger + triggerDelay)
        {
            GameManager.GM.player.GetComponent<Player>().TakeDamage(dmg);
            an.SetTrigger("Trigger");
            lastTrigger = Time.time;
        }
    } 
    
}
