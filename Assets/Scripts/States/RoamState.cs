using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using System.Linq;


public class RoamState : IBaseState
{
    private BotController bot;

    public List<GameObject> WayPoints = new List<GameObject>();
    public int wayPointCounter = 0;
    public float moveSpeed = 10f;
    public float rotSpeed = 0.2f;
    Vector3 direction;

    public void Move()
    {
        direction = WayPoints[wayPointCounter].transform.position - bot.transform.position;

        if (direction.magnitude > Random.Range(1.0f, 5.0f))
        {
            bot.transform.position = Vector3.MoveTowards(bot.transform.position, WayPoints[wayPointCounter].transform.position, moveSpeed * Time.deltaTime);
            Vector3 newDir = Vector3.RotateTowards(bot.transform.forward, direction, rotSpeed, 0.0F);
            bot.transform.rotation = Quaternion.LookRotation(newDir);
            return;
        }
        wayPointCounter = Random.Range(0, WayPoints.Count());
    }

    /*public void UpdateHealth(int amount)
    {
        bot.health += amount;
    }*/

    // Use this for initialization
    void Start()
    {
        bot = BotController.instance;
        WayPoints = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        wayPointCounter = Random.Range(0, WayPoints.Count());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StateUpdate()
    {
        Move();
    }
}
