using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats stats;

    void Awake()
    {
        stats = this;
    }

    //Player
    public float maxHealth;
    public float health;
    public float speed;
    public float poison = 0;
    public float bleed = 0;
    public float fire = 0;

    //Arrow
    public float damage = 1;

    public bool doPoison = false;
    public float poisonTime = 3f;
    public float poisonDamage = 2f;

    public bool doBleed = false;
    public float bleedTime = 5f;
    public float bleedDamage = 1f;

    public bool doFire = false;
    public float fireTime = 2f;
    public float fireDamage = 3f;


}
