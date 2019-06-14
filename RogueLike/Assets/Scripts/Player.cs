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
        Vector2 input = new Vector2(0,0);
        input.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        input.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
        rigidB.velocity = input * moveSpeed;

        //rotate crossbow
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float AngleRad = Mathf.Atan2(mousePos.y - crossBow.position.y, mousePos.x - crossBow.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        crossBow.rotation = Quaternion.Euler(0, 0, AngleDeg);

        //mirror crossbow
        if (mousePos.x - crossBow.position.x < 0){crossBow.localScale = new Vector3(1,-1,1); }
        else { crossBow.localScale = new Vector3(1, 1, 1); }


        if (Input.GetMouseButtonDown(0) && shoot.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            shoot.SetBool("Shoot", true);
            Instantiate(arrow, crossBow.position, crossBow.rotation);
        }
        else { shoot.SetBool("Shoot", false); }
    }
}
