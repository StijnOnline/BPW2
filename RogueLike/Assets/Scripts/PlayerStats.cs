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

    [Header("Arrows")]
    public float damage = 1;
    public bool doubleShot = false;
    public bool diagonal = false;
    [Space(5)]
    public bool doPoison = false;
    public float poisonTime = 3f;
    public float poisonDamage = 2f;
    [Space(5)]
    public bool doBleed = false;
    public float bleedTime = 5f;
    public float bleedDamage = 1f;
    [Space(5)]
    public bool doFire = false;
    public float fireTime = 2f;
    public float fireDamage = 3f;


}
