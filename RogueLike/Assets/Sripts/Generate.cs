using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generate : MonoBehaviour
{
    public Tilemap backLayer;
    public Tile groundTile;
    public Tile wallTile;

    [SerializeField]
    Room[,] grid = new Room[2, 2];

    public int roomSize = 5;
    public int doorSize = 1;
    public int hallSize = 2;

    void Start()
    {
        

        //testing grid 1
        grid[0, 0] = new Room();
        grid[0, 0].states[0] = Room.RoomState.Open;
        grid[0, 0].states[1] = Room.RoomState.Connected;

        grid[1, 0] = new Room();
        grid[1, 0].states[0] = Room.RoomState.Connected;
        grid[1, 0].states[3] = Room.RoomState.Connected;

        grid[0, 1] = new Room();
        grid[0, 1].states[1] = Room.RoomState.Open;
        grid[0, 1].states[2] = Room.RoomState.Open;

        grid[1, 1] = new Room();
        grid[1, 1].states[2] = Room.RoomState.Connected;
        grid[1, 1].states[3] = Room.RoomState.Open;
        




        //testing grid 2
        //grid[0, 0] = new Room();
        //grid[0, 0].states[0] = Room.RoomState.Closed;
        //grid[0, 0].states[1] = Room.RoomState.Closed;
        //grid[0, 0].states[2] = Room.RoomState.Closed;
        //grid[0, 0].states[3] = Room.RoomState.Closed;


        PlaceTiles();


    }

    void PlaceTiles()
    {
        roomSize = Mathf.Max(7, roomSize + 4 + hallSize);
        

        for (int i = 0; i < grid.GetLength(0) * grid.GetLength(1); i++)
        {
            Room r = grid[i % grid.GetLength(0), i / grid.GetLength(1)];

            Vector3Int[] positions = new Vector3Int[roomSize * roomSize];
            TileBase[] tileArray = new TileBase[positions.Length];

            for (int index = 0; index < positions.Length; index++)
            {
                positions[index] = new Vector3Int(index % roomSize + i % grid.GetLength(0) * roomSize, index / roomSize + i / grid.GetLength(0) * roomSize, 0);

                //Default
                if (index / roomSize < roomSize - hallSize  && index % roomSize < roomSize - hallSize  &&
                    index / roomSize >= hallSize && index % roomSize >= hallSize)
                {
                    tileArray[index] = groundTile;
                }
            //Room Walls
                //Connected
                if ((index / roomSize == roomSize - hallSize - 1 && r.states[0] == Room.RoomState.Connected) ||
                        (index % roomSize == roomSize - hallSize - 1 && r.states[1] == Room.RoomState.Connected) ||
                        (index / roomSize == hallSize && r.states[2] == Room.RoomState.Connected) ||
                        (index % roomSize == hallSize && r.states[3] == Room.RoomState.Connected)
                    )
                {
                    if ((index / roomSize == hallSize || index / roomSize == roomSize - hallSize - 1) &&
                        (index % roomSize == roomSize - hallSize - 1 || index % roomSize == hallSize))
                    {
                        tileArray[index] = wallTile; //block on corner
                    }
                    else { tileArray[index] = groundTile; } //block not on corner 
                }
                //Closed
                if ((index / roomSize == roomSize - hallSize - 1 && r.states[0] == Room.RoomState.Closed) ||
                           (index % roomSize == roomSize - hallSize - 1 && r.states[1] == Room.RoomState.Closed) ||
                           (index / roomSize == hallSize && r.states[2] == Room.RoomState.Closed) ||
                           (index % roomSize == hallSize && r.states[3] == Room.RoomState.Closed)
                       )
                {
                    tileArray[index] = wallTile;                    
                }
                //Open
                    if ((index / roomSize == roomSize - hallSize - 1 && r.states[0] == Room.RoomState.Open) ||
                        (index % roomSize == roomSize - hallSize - 1 && r.states[1] == Room.RoomState.Open) ||
                        (index / roomSize == hallSize && r.states[2] == Room.RoomState.Open) ||
                        (index % roomSize == hallSize && r.states[3] == Room.RoomState.Open)
                    )
                {
                    if (Mathf.Abs(index / roomSize - roomSize / 2) <= doorSize / 2 || Mathf.Abs(index % roomSize - roomSize / 2) <= doorSize / 2)
                    {
                        tileArray[index] = groundTile; //block in middle
                    }
                    else
                    {
                        tileArray[index] = wallTile; //block not in middle
                    }
                }

                //Connections between rooms
                //Connected
                if ((index / roomSize >= roomSize - hallSize - 1 && r.states[0] == Room.RoomState.Connected) ||
                        (index % roomSize >= roomSize - hallSize - 1 && r.states[1] == Room.RoomState.Connected) ||
                        (index / roomSize <= hallSize && r.states[2] == Room.RoomState.Connected) ||
                        (index % roomSize <= hallSize && r.states[3] == Room.RoomState.Connected)
                    )
                {
                    if ((index / roomSize <= hallSize || index / roomSize >= roomSize - hallSize - 1) &&
                        (index % roomSize >= roomSize - hallSize - 1 || index % roomSize <= hallSize))
                    {
                        tileArray[index] = wallTile; //block on corner
                    }
                    else { tileArray[index] = groundTile; } //block not on corner 
                }
                //Closed
                if ((index / roomSize > roomSize - hallSize - 1 && r.states[0] == Room.RoomState.Closed) ||
                           (index % roomSize > roomSize - hallSize - 1 && r.states[1] == Room.RoomState.Closed) ||
                           (index / roomSize < hallSize && r.states[2] == Room.RoomState.Closed) ||
                           (index % roomSize < hallSize && r.states[3] == Room.RoomState.Closed)
                       )
                {
                    tileArray[index] = null;
                }
                //Open
                if ((index / roomSize > roomSize - hallSize - 1 && r.states[0] == Room.RoomState.Open) ||
                        (index % roomSize > roomSize - hallSize - 1 && r.states[1] == Room.RoomState.Open) ||
                        (index / roomSize < hallSize && r.states[2] == Room.RoomState.Open) ||
                        (index % roomSize < hallSize && r.states[3] == Room.RoomState.Open)
                    )
                {
                    if (Mathf.Abs(index / roomSize - roomSize / 2) <= doorSize / 2 || Mathf.Abs(index % roomSize - roomSize / 2) <= doorSize / 2)
                    {
                        tileArray[index] = groundTile; //path
                    }
                    else if (Mathf.Abs(index / roomSize - roomSize / 2) <= doorSize / 2 + 1 || Mathf.Abs(index % roomSize - roomSize / 2) <= doorSize / 2 + 1)
                    {
                        tileArray[index] = wallTile; //wall
                    }
                    else
                    {
                        tileArray[index] = null; //outside
                    }
                }

            }
            backLayer.SetTiles(positions, tileArray);
        }

    }


    public class Room
    {
        //4 directions: Up, Right, Down, Left
        //3 states: Closed, Open, Connected
        public enum RoomState { Closed, Open, Connected };
        public RoomState[] states = new RoomState[4];
    }
}