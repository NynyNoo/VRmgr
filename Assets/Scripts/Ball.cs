using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    bool toDestroy = false;
    float timer = 0;
    float seconds = 0;
    private void Update()
    {
        if(toDestroy)
        {
            timer += Time.deltaTime;
            seconds = Convert.ToInt32(timer % 60);
            if (seconds > 3)
            {
                Destroy(gameObject);
            }
        }
    }
    public void RemoveBall()
    {
        toDestroy = true;
    }
}
