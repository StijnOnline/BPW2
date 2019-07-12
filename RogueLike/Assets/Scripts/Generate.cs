using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generate : MonoBehaviour
{
    //Local Room Generation Settings
    int roomSize = 5;
    int doorSize = 1;
    int hallSize = 2;

    int gridSize = 5;
    int minRooms = 4;
    int treasureRooms = 1;

    [Header("Tiles")]
    public Tilemap backLayer;
    public Tilemap frontLayer;
    [Space(10)]
    public Tile groundTile;
    public Tile wallTile;

    [SerializeField]
    Room[,] grid;

    

    HashSet<Vector2Int> locations = new HashSet<Vector2Int>();

    void Awake()
    {
        //get Room Generation Settings
        GameManager gm = GameManager.GM;
        roomSize = gm.roomSize;
        doorSize = gm.doorSize;
        hallSize = gm.hallSize;

        gridSize = gm.gridSize;
        minRooms = gm.minRooms;
        treasureRooms = gm.treasureRooms;

        grid = new Room[gridSize, gridSize];
        GenerateGrid();
        PlaceTiles();
        FillRooms();

    }

    void GenerateGrid()
    {
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
            r = Random.Range(0f, 1f); //chance to connect rooms

            newLocation = oldLocation + offSet;
            if (!locations.Contains(newLocation))
            {
                locations.Add(newLocation);
                grid[newLocation.x, newLocation.y] = new Room();
                r = 0; //always connect new rooms
            }
            if (r < 0.4f) //chance to connect rooms
            {
                //Choosing a roomstate
                Room.RoomState roomState = Room.RoomState.Closed;
                r = Random.Range(0f, 1f);
                if (r < 0.25f) { roomState = Room.RoomState.Connected; }
                else { roomState = Room.RoomState.Open; }
                //Set room states
                int state = 1 - offSet.y + (offSet.x != -1 ? 0 : 2);
                grid[oldLocation.x, oldLocation.y].states[state] = roomState;
                state = 1 + offSet.y + (offSet.x != 1 ? 0 : 2);
                grid[newLocation.x, newLocation.y].states[state] = roomState;
                //Roomtypes
                grid[newLocation.x, newLocation.y].Randomise();
            }
        }
        HashSet<Vector2Int> filtered = new HashSet<Vector2Int>(locations); //all generated rooms exept the following       

        //Random start room
        Vector2Int startR = filtered.ElementAt(Random.Range(0, filtered.Count));
        grid[startR.x, startR.y].type = GameManager.GM.startRoom;        
        filtered.Remove(startR);

        //Random boss room
        Vector2Int bossR = filtered.ElementAt(Random.Range(0, filtered.Count));
        grid[bossR.x, bossR.y].type = GameManager.GM.bossRoom;
        filtered.Remove(bossR);

        Vector2Int treasureR;
        //Random treasure rooms
        for (int i = 0; i < treasureRooms; i++)
        {
            treasureR = filtered.ElementAt(Random.Range(0, filtered.Count));
            grid[treasureR.x, treasureR.y].type = GameManager.GM.treasureRoom;
            filtered.Remove(treasureR);
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
                        if ((index / roomSize < hallSize || index / roomSize > roomSize - hallSize - 1) &&
                        (index % roomSize > roomSize - hallSize - 1 || index % roomSize < hallSize))
                        {
                            tileArray[index] = null;//blocks outside corner
                        }
                        else
                        {
                            tileArray[index] = wallTile; //blocks on corner
                        }
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

    void FillRooms()
    {
        foreach (Vector2Int location in locations)
        {
            Room room = grid[location.x, location.y];
            Vector2 roomCenter = location * roomSize + Vector2.one * roomSize / 2;

            if(room.type.name == "Start")
            {
                Instantiate(GameManager.GM.player, roomCenter, Quaternion.identity);
            }
            if (room.type.name == "Enemy")
            {
                Instantiate(GameManager.GM.enemies[0], roomCenter, Quaternion.identity);
            }
            if(room.type.name == "Treasure")
            {
                Tile tile = new Tile(); tile.name = "Chest";
                Chest[] chests = new Chest[2];

                HashSet<Vector2> chestLocations = new HashSet<Vector2>();
                while (chestLocations.Count < 2)
                {
                    chestLocations.Add(roomCenter + RandomV2(2, 4));
                }

                frontLayer.SetTile(Vector3Int.FloorToInt(chestLocations.First()), tile);
                chests[0] = Instantiate(GameManager.GM.chest, chestLocations.First(), Quaternion.identity).GetComponent<Chest>();
                frontLayer.SetTile(Vector3Int.FloorToInt(chestLocations.Last()), tile);
                chests[1] = Instantiate(GameManager.GM.chest, chestLocations.Last(), Quaternion.identity).GetComponent<Chest>();

                chests[0].otherChest = chests[1].gameObject;
                chests[1].otherChest = chests[0].gameObject;


                //while (chests.Count < 2)
                //{
                //    rPos = roomCenter + RandomV2(2, 4);
                //    if (frontLayer.GetTile(Vector3Int.FloorToInt(rPos)) == null)
                //    {
                //        frontLayer.SetTile(Vector3Int.FloorToInt(rPos), tile);
                //        chests.Add(Instantiate(GameManager.GM.chest, rPos, Quaternion.identity).GetComponent<Chest>());
                //    }
                //}
            }

        }

        Debug.Log("Filled Rooms");
    }


   

    ///Generates a Vector2 with min and max on absolute value
    public Vector2 RandomV2(int mindist, int maxdist)
    {
        return new Vector2(Random.Range(-1, 1) * Random.Range(mindist, maxdist), Random.Range(-1, 1) * Random.Range(mindist, maxdist));
    }
}