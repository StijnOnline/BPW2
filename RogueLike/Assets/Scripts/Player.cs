﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    
    public Transform crossBow;
    public GameObject arrow;
    Animator shoot;

    private Rigidbody2D rigidB;

    public Light2D playerLight;
    public Light2D bowLight;


    AudioSource audioSource;
    [Header("Audio")]
    public AudioClip hurtAudio;
    public AudioClip bowAudio;

    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
        shoot = crossBow.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        shoot.SetFloat("ReloadSpeed", PlayerStats.stats.reloadSpeed);

        //move player
        Vector2 input = new Vector2(0, 0);
        input.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        input.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        rigidB.velocity = input * PlayerStats.stats.moveSpeed;

        //rotate crossbow
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float AngleRad = Mathf.Atan2(mousePos.y - crossBow.position.y, mousePos.x - crossBow.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        crossBow.rotation = Quaternion.Euler(0, 0, AngleDeg);

        //mirror crossbow
        if (mousePos.x - crossBow.position.x < 0) {
            crossBow.localScale = new Vector3(1, -1, 1);
            crossBow.GetChild(0).localRotation = Quaternion.Euler(0,0,90);
        }
        else {
            crossBow.localScale = new Vector3(1, 1, 1);
            crossBow.GetChild(0).localRotation = Quaternion.Euler(0, 0, -90);
        }


        if (Input.GetMouseButtonDown(0) && shoot.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            shoot.SetBool("Shoot", true);
            Shoot();
            if (PlayerStats.stats.doubleShot) { Invoke("Shoot", 0.1f); }
        }
        else { shoot.SetBool("Shoot", false); }

        


    }

    void Shoot()
    {
        Instantiate(arrow, crossBow.position, crossBow.rotation);
        if (PlayerStats.stats.diagonal)
        {
            Instantiate(arrow, crossBow.position + crossBow.up * 0.15f, crossBow.rotation * Quaternion.Euler(0, 0, 5));
            Instantiate(arrow, crossBow.position - crossBow.up * 0.15f, crossBow.rotation * Quaternion.Euler(0, 0, -5));
        }
        audioSource.PlayOneShot(bowAudio,0.1f);
        
    }

    public void TakeDamage(float dmg)
    {
        PlayerStats.stats.health -= dmg;
        if (PlayerStats.stats.health <= 0) { StartCoroutine(Die()); }
        else { StartCoroutine(ChangeLights());}
        audioSource.PlayOneShot(hurtAudio);        
    }

    public IEnumerator ChangeLights() //weird name but ok
    {
        playerLight.pointLightOuterRadius = 8;
        playerLight.intensity = 0.5f;
        playerLight.color = new Color(1, 0.5f, 0.5f);

        yield return new WaitForSeconds(0.1f);

        playerLight.pointLightOuterRadius = 8;
        playerLight.intensity = .5f;
        playerLight.color = Color.white;     

        yield return new WaitForSeconds(0.1f);

        SetLights();
    }
    public void SetLights() //weird name but ok
    {
        playerLight.color = new Color(100/255f, 180/255f, 1);
        float pHealth = PlayerStats.stats.health / PlayerStats.stats.maxHealth;
        playerLight.pointLightOuterRadius = 4 * pHealth + 3; // 3 - 7
        playerLight.intensity = 0.3f * pHealth + 0.2f; // 0.2 - 0.5
        bowLight.pointLightOuterAngle = 90 * pHealth + 30; //30 - 120
        bowLight.pointLightOuterRadius = 2 * pHealth + 4; // 4 - 6
        bowLight.intensity = 0.3f * pHealth + 0.2f; // 0.2 - 0.5

        int healthsprite = Mathf.Min(Mathf.FloorToInt(PlayerStats.stats.health / PlayerStats.stats.maxHealth * 4f), 3);
        GameManager.GM.healthImage.sprite = GameManager.GM.healthSprites[healthsprite];
    }

    public IEnumerator Die()
    {
        GameManager.GM.transition.SetActive(true);
        GameManager.GM.transitionText.SetText("Game Over");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }

    public void Upgrade(GameManager.UpgradeType type)
    {
        switch (type)
        {
            case GameManager.UpgradeType.PoisonArrow:
                PlayerStats.stats.doPoison = true;
                break;
            case GameManager.UpgradeType.BleedArrow:
                PlayerStats.stats.doBleed = true;
                break;
            case GameManager.UpgradeType.FireArrow:
                PlayerStats.stats.doFire = true;
                break;
            case GameManager.UpgradeType.DoubleShot:
                PlayerStats.stats.doubleShot = true;
                break;
            case GameManager.UpgradeType.DiagnalShot:
                PlayerStats.stats.diagonal = true;
                break;
            case GameManager.UpgradeType.HealthUp:
                PlayerStats.stats.health += PlayerStats.stats.maxHealth * 0.2f;
                PlayerStats.stats.maxHealth *= 1.2f;
                break;
            case GameManager.UpgradeType.DamageUp:
                PlayerStats.stats.damage *= 1.5f;
                break;
            case GameManager.UpgradeType.AttackSpeedUp:
                PlayerStats.stats.damage *= 1.5f;
                break;
        }
        if (type != GameManager.UpgradeType.HealthUp || type != GameManager.UpgradeType.DamageUp || type != GameManager.UpgradeType.AttackSpeedUp)
        {
            GameManager.GM.collectedUpgrades.Add(type);
        }
    }
}
