using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    public override void Move()
    {

        Vector3 playerPos = GameManager.GM.player.transform.position;
        Vector2 toPlayer = (Vector2)(playerPos - transform.position);

        if (toPlayer.magnitude < detectionRange)
        {
            rigidB.velocity = toPlayer.normalized * speed;
        }
    }
           
}
