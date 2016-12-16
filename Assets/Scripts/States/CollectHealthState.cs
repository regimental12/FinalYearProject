using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;
using System.Collections.Generic;
using System.Linq;

public class CollectHealthState : IBaseState
{

    private BotController bot;

    public List<GameObject> healthItems = new List<GameObject>();
    public float moveSpeed = 10f;
    public float rotSpeed = 0.2f;
    Vector3 direction;
    public int nextItem = 0;

    public void MoveToHealth()
    {
        //healthItems = healthItems.OrderBy(x => Vector3.Distance(x.transform.position, bot.transform.position)).ToList();
        if (bot.transform.position != healthItems[nextItem].transform.position)
        {
            bot.transform.position = Vector3.MoveTowards(bot.transform.position, healthItems[nextItem].transform.position, moveSpeed * Time.deltaTime);

            Vector3 newDir = Vector3.RotateTowards(bot.transform.forward, direction, rotSpeed, 0.0F);
            bot.transform.rotation = Quaternion.LookRotation(newDir);
            return;
        }
        nextItem++;
        if(nextItem > healthItems.Count() -1)
        {
            nextItem = 0;
        }
    }

    // Use this for initialization
    void Start()
    {
        bot = BotController.instance;
        healthItems = GameObject.FindGameObjectsWithTag("HealthItem").ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StateUpdate()
    {
        MoveToHealth();
    }
}
