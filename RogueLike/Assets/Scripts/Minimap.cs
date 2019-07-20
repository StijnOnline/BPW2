using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    void Start()
    {
        float roomSize = Mathf.Max(7, GameManager.GM.roomSize + 4 + GameManager.GM.hallSize);
        float mapSize = roomSize * GameManager.GM.gridSize;
        transform.position = new Vector3(mapSize / 2, mapSize / 2);
    }
}
