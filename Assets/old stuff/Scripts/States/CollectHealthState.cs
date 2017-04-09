using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System;

public class CollectHealthState : IBaseState
{
    public List<GameObject> healthItems = new List<GameObject>();
    public float moveSpeed = 10f;
    public float rotSpeed = 0.2f;
    Vector3 direction;
    public int nextItem = 0;
    public BotController bot;

    public void MoveToHealth()
    {
        if (nextItem > healthItems.Count() - 1)
        {
            Debug.Log(healthItems.Count());
            Debug.Log(nextItem);
            nextItem = 0;
        }
        direction = healthItems[nextItem].transform.position - transform.position;
        if (transform.position != healthItems[nextItem].transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, healthItems[nextItem].transform.position, moveSpeed * Time.deltaTime);

            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
            return;
        }
        
            buildHealthItemList();
        
        //nextItem++;
        
    }

    
    // Use this for initialization
    void Start()
    {
        bot = GetComponent<BotController>();
        buildHealthItemList();

    }

    void OnEnalbe()
    {
        buildHealthItemList();
    }

    private void buildHealthItemList()
    {
        healthItems = GameObject.FindGameObjectsWithTag("HealthItem").ToList();
        healthItems = healthItems.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(bot.health + " " + bot.name);
    }

    public override void StateUpdate()
    {
        MoveToHealth(); 
    }
}
