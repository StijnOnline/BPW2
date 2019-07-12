using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public int size = 2;

    public override void Die()
    {
        if (size > 1)
        {
            List<GameObject> newSlimes = new List<GameObject>();
            newSlimes.Add(Instantiate(gameObject, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Quaternion.identity));
            newSlimes.Add(Instantiate(gameObject, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Quaternion.identity));
            foreach (GameObject newslime in newSlimes)
            {
                newslime.transform.localScale = transform.localScale / 2;
                Destroy(newslime.transform.GetChild(0).gameObject); //Destroy old healthbar

                Slime slime = newslime.GetComponent<Slime>();
                slime.maxHealth = maxHealth / 2;
                slime.size = size - 1;

            }
            
        }

        Destroy(gameObject);
    }
}
