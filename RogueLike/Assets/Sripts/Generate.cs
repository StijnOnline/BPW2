using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generate : MonoBehaviour
{
    public Tilemap backLayer;
    public Tile groundTile;
    public Tile wallTile;

    [SerializeField]
    Room[,] grid;

    public int roomSize = 5;
    public int doorSize = 1;
    public int hallSize = 2;

    public int gridSize = 5;
    public int minRooms = 4;

    void Start()
    {
        grid = new Room[gridSize, gridSize];
        GenerateGrid();
        PlaceTiles();
    }

    void GenerateGrid()
    {

        HashSet<Vector2Int> locations = new HashSet<Vector2Int>();
        Vector2Int startLocacation = new Vector2Int(gridSize / 2, gridSize / 2);
        locations.Add(startLocacation);
        grid[startLocacation.x, startLocacation.y] = new Room();

        while (locations.Count < minRooms)
        {
            Vector2Int oldLocation = locations.ElementAt(Random.Range(0, locations.Count));
            Vector2Int newLocation = new Vector2Int();
            Vector2Int offSet = new Vector2Int();
            //Choosing new direction to spawn room
            float r = Random.Range(0f, 1f);
            if (r < .25f && oldLocation.x > 0) { offSet = new Vector2Int(-1, 0); }
            else if (r < .50f && oldLocation.x < gridSize - 1) { offSet = new Vector2Int(1, 0); }
            else if (r < .75f && oldLocation.y > 0) { offSet = new Vector2Int(0, -1); }
            else if (r < 1f && oldLocation.y < gridSize - 1) { offSet = new Vector2Int(0, 1); }
            else { continue; }
            //Adding Location
            newLocation = oldLocation + offSet;
            if (locations.Contains(newLocation)) { continue; }
            locations.Add(newLocation);
            grid[newLocation.x, newLocation.y] = new Room();
            //Choosing a roomstate
            Room.RoomState roomState = Room.RoomState.Closed;
            r = Random.Range(0f, 1f);
            if (r < 0.25f) { roomState = Room.RoomState.Connected; }
            else{ roomState = Room.RoomState.Open; }            
            //Set room states
            int state = 1 - offSet.y + (offSet.x != -1 ? 0 : 2);
            grid[oldLocation.x, oldLocation.y].states[state] = roomState;            
            state = 1 + offSet.y + (offSet.x != 1 ? 0 : 2);
            grid[newLocation.x, newLocation.y].states[state] = roomState;

        }

        Debug.Log("Generated Level");

    }

    void PlaceTiles()
    {
        roomSize = Mathf.Max(7, roomSize + 4 + hallSize);


        for (int i = 0; i < grid.GetLength(0) * grid.GetLength(1); i++)
        {
            Room r = grid[i % grid.GetLength(0), i / grid.GetLength(1)];

            if (r == null)
            {
                continue;
            }

            Vector3Int[] positions = new Vector3Int[roomSize * roomSize];
            TileBase[] tileArray = new TileBase[positions.Length];

            for (int index = 0; index < positions.Length; index++)
            {
                positions[index] = new Vector3Int(index % roomSize + i % grid.GetLength(0) * roomSize, index / roomSize + i / grid.GetLength(0) * roomSize, 0);

                //Default
                if (index / roomSize < roomSize - hallSize && index % roomSize < roomSize - hallSize &&
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
        Debug.Log("Placed Tiles");
    }


    public class Room
    {
        //4 directions: Up, Right, Down, Left
        //3 states: Closed, Open, Connected
        public enum RoomState { Closed, Open, Connected };
        public RoomState[] states = new RoomState[4];
    }
}