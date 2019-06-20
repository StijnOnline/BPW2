using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float reloadSpeed = 1f;
    public Transform crossBow;
    public GameObject arrow;
    Animator shoot;

    private Rigidbody2D rigidB;
    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
        shoot = crossBow.GetComponent<Animator>();


    }

    void Update()
    {
        shoot.SetFloat("ReloadSpeed", reloadSpeed);


        //move player
        Vector2 input = new Vector2(0, 0);
        input.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        input.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        rigidB.velocity = input * moveSpeed;

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
        }
        if (type != GameManager.UpgradeType.HealthUp || type != GameManager.UpgradeType.DamageUp)
        {
            GameManager.GM.collectedUpgrades.Add(type);
        }
    }
}
