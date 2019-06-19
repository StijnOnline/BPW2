using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public enum UpgradeType { HealthUp, DamageUp, DoubleShot, DiagnalShot, PoisonArrow, BleedArrow, FireArrow };

    public GameObject player;
    public List<UpgradeType> collectedUpgrades = new List<UpgradeType>();

    public GameObject healthBar;
    public Sprite poisonSprite;
    public Sprite bleedSprite;
    public Sprite fireSprite;

    void Awake()
    {
        GM = this;
    }

    

}