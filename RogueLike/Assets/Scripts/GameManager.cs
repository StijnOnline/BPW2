using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

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
    public TextMeshProUGUI bossTimerText;
    public Image healthImage;
    public Sprite[] healthSprites;
    public GameObject transition;
    public TextMeshProUGUI transitionText;

    [Header("Other")]
    public float bossTimer = 300;
    public AudioClip LanternActivateAudio;
    public AudioClip PedestalActivateAudio;
    public AudioClip HealActivateAudio;

    void Awake()
    {
        GM = this;
        //startRoom = Resources.Load<RoomType>("Start");
        //treasureRoom = Resources.Load<RoomType>("Treasure");
        //bossRoom = Resources.Load<RoomType>("Boss");
        //randomRooms = Resources.LoadAll<RoomType>("Random");
    }

    public void Update()
    {
        if (bossTimerText != null) {bossTimerText.SetText("Boss arriving in\n" + Mathf.FloorToInt(bossTimer - Time.time));}
        if (bossTimer != 0 && Time.time > bossTimer)
        {
            StartCoroutine(TriggerBossFight());            
        }
    }

    public IEnumerator TriggerBossFight()
    {
        transition.SetActive(true);
        transitionText.SetText("Boss Fight");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Boss");
    }

}