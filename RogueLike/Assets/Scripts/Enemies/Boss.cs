using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    public GameObject rat;
    public GameObject fireBall;
    public float fireBallSpeed;
    public Transform fireBallSpawn;
    public int lanterns = 5;
    public int phase = 0;
    

    public override void Update()
    {
        base.Update();

        Vector3 toPlayer = (GameManager.GM.player.transform.position - transform.position);

        float dir = transform.localScale.x;
        if (isPointingRight && toPlayer.x > 0) { dir = 4; }
        if (isPointingRight && toPlayer.x < 0) { dir = -4; }
        if (!isPointingRight && toPlayer.x > 0) { dir = -4; }
        if (!isPointingRight && toPlayer.x < 0) { dir = 4; }
        transform.localScale = new Vector3(dir, 4, 1);

        if(phase == 0 && lanterns == 0)
        {
            phase += 1;
            health -= 300;
            attackDelay *= 0.6f;
        }
    }

    public override void Attack()
    {
        if (phase == 0)
        {
            //fireball
            Vector3 vectorToTarget = GameManager.GM.player.transform.position - fireBallSpawn.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            GameObject go = Instantiate(fireBall, fireBallSpawn.position, Quaternion.identity);
            go.GetComponent<Transform>().rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            float r = Random.Range(0f, 1f);
            if (r > 0.6)
            {
                Rat rt = Instantiate(rat, transform.position + new Vector3(0, 2f), Quaternion.identity).GetComponent<Rat>();
                rt.detectionRange = 100f;
            }
        }else if(phase == 1)
        {
            float r = Random.Range(0f, 1f);
            if (r > 0.7)
            {
                Rat rt = Instantiate(rat, transform.position + new Vector3(2f, 0), Quaternion.identity).GetComponent<Rat>();
                rt.detectionRange = 100f;
                rt = Instantiate(rat, transform.position + new Vector3(-2f, 0), Quaternion.identity).GetComponent<Rat>();
                rt.detectionRange = 100f;
            }
        }
    }
}
