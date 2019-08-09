using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Boss boss;    
    [Space(5)]
    public GameObject[] enemies;

    [Header("Sprites / HUD")]
    public GameObject healthBar;
    public Sprite poisonSprite;
    public Sprite bleedSprite;
    public Sprite fireSprite;
    public TMPro.TextMeshProUGUI bossTimerText;
    public Image healthImage;
    public Sprite[] healthSprites;

    [Header("Other")]
    public float bossTimer = 300;

    void Awake()
    {
        GM = this;
        randomRooms = Resources.LoadAll<RoomType>("Rooms/Random");

    }

    public void Update()
    {
        if (bossTimerText != null) {bossTimerText.SetText("Boss arriving in " + Mathf.FloorToInt(bossTimer - Time.time));}
        if (bossTimer != 0 && Time.time > bossTimer)
        {
            SceneManager.LoadScene("Boss");
        }
    }

}