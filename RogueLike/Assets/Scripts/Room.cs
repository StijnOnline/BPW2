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
        //TODO: Room Chances

        type = GameManager.GM.randomRooms[Random.Range(0, GameManager.GM.randomRooms.Length)];
    }
}

//Start , Treasure, Boss 
//Abandoned, Enemy,
