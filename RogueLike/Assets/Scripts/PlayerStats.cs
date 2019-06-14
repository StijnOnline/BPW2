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
    public bool doBleed = false;
    public bool doFire = false;

}
