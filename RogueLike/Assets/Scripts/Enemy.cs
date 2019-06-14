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
    SpriteRenderer[] statusEffects;

    private void Start()
    {
        health = maxHealth;

        healthBar = Instantiate(GameManager.GM.healthBar,transform);
        healthBar.transform.localPosition = new Vector3(0,0.4f,0);
        statusEffects = healthBar.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();

        InvokeRepeating("Tick",0f,1f);
    }

    public void Tick()
    {
        if (poison > 0) {health -= PlayerStats.stats.poisonDamage; poison -= 1; } 
        if (bleed > 0) { health -= PlayerStats.stats.bleedDamage; bleed -= 1; } 
        if (fire > 0) { health -= PlayerStats.stats.fireDamage; fire -= 1; }
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        healthBar.transform.GetChild(1).localPosition = new Vector3(-(maxHealth - health) / maxHealth, 0, 0);
        int statusses = 0;
        if (poison > 0) { statusEffects[statusses].sprite = GameManager.GM.poisonSprite; statusses++; }
        if (bleed > 0) { statusEffects[statusses].sprite = GameManager.GM.bleedSprite; statusses++; }
        if (fire > 0) { statusEffects[statusses].sprite = GameManager.GM.fireSprite; statusses++; }
        for (int i = statusses; i < statusEffects.Length; i++) { statusEffects[i].sprite = null; }

    }
}