using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    [Header("Room Generation Settings")]
    public int roomSize = 5;
    public int doorSize = 1;
    public int hallSize = 2;

    public int gridSize = 5;
    public int minRooms = 4;
    public int treasureRooms = 1;
    public int roomObjects = 3;
    public int traps = 3;
    

    [Header("Rooms")]
    public RoomType startRoom;
    public RoomType treasureRoom;
    public RoomType bossRoom;
    public RoomType[] randomRooms;
    

    [Header("Upgrades")]
    public List<UpgradeType> collectedUpgrades = new List<UpgradeType>();
    public enum UpgradeType { HealthUp, DamageUp, AttackSpeedUp, DoubleShot, DiagnalShot, PoisonArrow, BleedArrow, FireArrow };
    

    [Header("GameObjects")]
    public GameObject player;
    public GameObject chest;
    public GameObject heal;    
    public GameObject trap;    
    [Space(5)]
    public GameObject[] enemies;

    [Header("Sprites / HUD")]
    public GameObject healthBar;
    public Sprite poisonSprite;
    public Sprite bleedSprite;
    public Sprite fireSprite; 

    void Awake()
    {
        GM = this;
        randomRooms = Resources.LoadAll<RoomType>("Rooms/Random");
    }

}