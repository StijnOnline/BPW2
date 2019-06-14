using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector] public float health;
    public float damage;
    public float speed;
    public float poison = 0;
    public float bleed = 0;
    public float fire = 0;

    public GameObject healthBar;

    private void Start()
    {
        health = maxHealth;

        healthBar = Instantiate(GameManager.GM.healthBar,transform);
        healthBar.transform.localPosition = new Vector3(0,0.4f,0);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        healthBar.transform.GetChild(0).localPosition = new Vector3( -(maxHealth-health)/maxHealth , 0, 0);
    }
}