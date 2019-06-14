using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            enemy.health -= PlayerStats.stats.damage;            
            if (PlayerStats.stats.doFire) { enemy.fire = PlayerStats.stats.fireTime; }
            if (PlayerStats.stats.doBleed) { enemy.bleed = PlayerStats.stats.bleedTime; }
            if (PlayerStats.stats.doPoison) { enemy.poison = PlayerStats.stats.poisonTime; }
            enemy.UpdateHUD();
            
        }

        Destroy(gameObject);
    }
}
