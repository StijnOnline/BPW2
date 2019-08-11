using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float dmg = 10f;
    public float triggerDelay = 0.3f;
    float lastTrigger = 0;
    Animator an;
    AudioSource audioSource;
    public AudioClip activateAudio;
    void Start()
    {
        an = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            lastTrigger = Time.time - triggerDelay/2f; //Trap waits a bit before first tick
            audioSource.PlayOneShot(activateAudio);
        }
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
