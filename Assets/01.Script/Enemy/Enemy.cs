using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy _instance;
    public float time;
    public float speed = 1;

    private void Start()
    {
        _instance = this;
    }

    private void Update()
    {
        speed += Time.deltaTime;
        transform.Translate(-1 * Time.deltaTime * speed,0,0);
    }

    public void GoBack(float dist)
    {
        transform.Translate(1 * dist,0,0);
    }
    
    
}
