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

}