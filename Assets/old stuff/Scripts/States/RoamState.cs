using UnityEngine;
using System.Collections.Generic;

using System.Linq;


public class RoamState : IBaseState
{
    public List<GameObject> WayPoints = new List<GameObject>();
    public int wayPointCounter = 0;
    public float moveSpeed = 10f;
    public float rotSpeed = 0.2f;
    Vector3 direction;
    public BotController bot;

    public void Move()
    {
        direction = WayPoints[wayPointCounter].transform.position - transform.position;

        if (direction.magnitude > Random.Range(1.0f, 5.0f))
        {
            transform.position = Vector3.MoveTowards(transform.position, WayPoints[wayPointCounter].transform.position, moveSpeed * Time.deltaTime);
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
            return;
        }
        wayPointCounter = Random.Range(0, WayPoints.Count());
    }


    // Use this for initialization
    void Start()
    {
        bot = GetComponent<BotController>();
        WayPoints = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        wayPointCounter = Random.Range(0, WayPoints.Count());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(bot.health + " " + bot.name);
    }

    public override void StateUpdate()
    {
        Move();
    }
}
