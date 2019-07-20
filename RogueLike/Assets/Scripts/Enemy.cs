using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;
    [HideInInspector] public float poison = 0;
    [HideInInspector] public float bleed = 0;
    [HideInInspector] public float fire = 0;    

    [Space(10)]
    public float speed;
    public float moveDelay = 0f;
    float lastMove = 0f;
    public float detectionRange = 4f;

    [Space(10)]
    public float damage;
    public float attackRange = 0.5f;
    public float attackDelay = 0f;
    float lastAttack = 0f;

    [Space(10)]
    public bool isPointingRight;


    [HideInInspector] public Rigidbody2D rigidB;
    [HideInInspector] public Vector2 toPlayer;
    [HideInInspector] public GameObject healthBar;
    SpriteRenderer[] statusEffects;

    
    

    private void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();

        health = maxHealth;

        healthBar = Instantiate(GameManager.GM.healthBar,transform);
        healthBar.transform.localPosition = new Vector3(0,0.4f,0);
        statusEffects = healthBar.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();

        InvokeRepeating("DamageTick", 0f,1f);
    }

    void Update()
    {
        Vector3 playerPos = GameManager.GM.player.transform.position;
        toPlayer = (Vector2)(playerPos - transform.position);

        if (Time.time > lastMove + moveDelay)
        {
            lastMove = Time.time;
            Move();
            //TODO: flip sprite as neccesary

            //float dir = Mathf.Sign(rigidB.velocity.x)
            //dir = (!isPointingRight && rigidB.velocity.x > 0) ? -1 : 1;
            //transform.localScale = new Vector3(dir, 1, 1);
        }
        
        if (Time.time > lastAttack + attackDelay && toPlayer.magnitude < attackRange)
        {
            lastAttack = Time.time;
            Attack();
        }

    }

    public void DamageTick()
    {
        if (poison > 0) {health -= PlayerStats.stats.poisonDamage; poison -= 1; } 
        if (bleed > 0) { health -= PlayerStats.stats.bleedDamage; bleed -= 1; } 
        if (fire > 0) { health -= PlayerStats.stats.fireDamage; fire -= 1; }
        if (health <= 0) { Die(); }
        UpdateHP();
    }

    public void UpdateHP()
    {
        healthBar.transform.GetChild(1).localPosition = new Vector3( (health / maxHealth / 2f - 0.5f) , 0, 0);
        healthBar.transform.GetChild(1).localScale = new Vector3( (health / maxHealth) , 0.2f, 0);
        int statusses = 0;
        if (poison > 0) { statusEffects[statusses].sprite = GameManager.GM.poisonSprite; statusses++; }
        if (bleed > 0) { statusEffects[statusses].sprite = GameManager.GM.bleedSprite; statusses++; }
        if (fire > 0) { statusEffects[statusses].sprite = GameManager.GM.fireSprite; statusses++; }
        for (int i = statusses; i < statusEffects.Length; i++) { statusEffects[i].sprite = null; }
    }
    
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        Debug.Log("Enemy moved");
    }

    public virtual void Attack()
    {
        Debug.Log("Enemy attacked");
        GameManager.GM.player.GetComponent<Player>().TakeDamage(damage);
    }


    //public void OnCollisionEnter2D(Collision2D coll)
    //{
        
    //    Debug.Log(coll.gameObject.name);
    //    GameObject other = coll.gameObject;
    //    if (other.tag == "Player")
    //    {
    //        other.GetComponent<Player>().TakeDamage(damage);
    //    }
    //}
}