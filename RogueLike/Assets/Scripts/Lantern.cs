using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Lantern : MonoBehaviour
{
    public bool lit = false;
    Light2D light;

    void Start()
    {
        light = GetComponentInChildren<Light2D>();
    }

    public void Light()
    {
        if (!lit) {
            light.intensity = 1.5f;
            light.pointLightOuterRadius = 5f;
            lit = true;
            GameManager.GM.boss.lanterns -= 1;
        }

    }
}
