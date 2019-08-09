using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    

    //4 directions: Up, Right, Down, Left
    //3 states: Closed, Open, Connected
    public enum RoomState { Closed, Open, Connected };
    public RoomState[] states = new RoomState[4];

    public RoomType type;

    public void Randomise()
    {
        //float r = Random.Range(0f, 1f);
        //if (r < 0.9) { type = RoomType.Enemy; return; }
        //if (r < 1) { type = RoomType.Abandoned; return; }

        //pick random from list, throw dice, test, repeat
        type = GameManager.GM.randomRooms[Random.Range(0, GameManager.GM.randomRooms.Length)];

        Debug.Log(type);
    }
}

//Start , Treasure, Boss 
//Abandoned, Enemy,
[CreateAssetMenu(fileName = "RoomType", menuName = "Room", order = 1)]
public class RoomType : ScriptableObject
{
    public float roomChance;
    public Tile[] randomTiles;
    [Space(5)]
    public int enemies;
    public GameObject[] randomEnemies;
}