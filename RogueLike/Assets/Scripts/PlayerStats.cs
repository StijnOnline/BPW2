using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats stats;

    void Awake()
    {
        if (stats == null)
        {
            stats = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    //Player
    public float maxHealth;
    public float health;
    public float moveSpeed = 1f;    

    [Header("Arrows")]
    public float reloadSpeed = 1f;
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
