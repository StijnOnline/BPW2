using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    void Awake()
    {
        GM = this;
    }

    public GameObject healthBar;
    public Sprite poisonSprite;
    public Sprite bleedSprite;
    public Sprite fireSprite;

}