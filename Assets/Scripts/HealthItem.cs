﻿using UnityEngine;
using System.Collections;

public class HealthItem : MonoBehaviour
{
    bool active = true;
    float timeOut = 5.0f;
    float timeOutReset = 5.0f;
    public GameObject cube;
    public Collider col;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            cube.SetActive(false);
            col.enabled = false;
            active = false;
            //collider.SendMessage("UpdateHealth", +50);
            collider.gameObject.GetComponent<BotController>().Health += 50;
        }
          
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            timeOut -= Time.deltaTime;
            if (timeOut <= 0)
            {
                cube.SetActive(true);
                col.enabled = true;
                active = true;
                timeOut = timeOutReset;
            }
        }
    }
}
