using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "RoomType", menuName = "Room", order = 51)]
public class RoomType : ScriptableObject
{
    public float roomChance;
    public Tile[] randomTiles;
    [Space(5)]
    public int enemies;
    public GameObject[] randomEnemies;
}