using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public float fireBallSpeed;
    public float rotateSpeed;
    public float dmg = 5;
    Rigidbody2D rigidB;

    public void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.GM.player.GetComponent<Player>().TakeDamage(dmg);
        }
        if (collision.gameObject.tag == "Lantern")
        {
            collision.gameObject.GetComponent<Lantern>().Light();
        }


        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {  
        Vector3 vectorToTarget = GameManager.GM.player.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

        rigidB.velocity = fireBallSpeed * transform.right;
    }
}
