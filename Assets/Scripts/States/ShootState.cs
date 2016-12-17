using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using System.Linq;


public class ShootState : IBaseState
{
    public BotController bot;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 10.0f;
    Vector3 direction;

    public void MoveToShoot()
    {
        if (bot.enemy != null)
        {
            if (Vector3.Distance(bot.enemy.transform.position, transform.position) > 5.0f)
            {
                direction = bot.enemy.transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, bot.enemy.transform.position, moveSpeed * Time.deltaTime);
                Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
                transform.rotation = Quaternion.LookRotation(newDir);
                Shoot();
                return;
            }
        }
        else
        {
            bot.exitShootState();
        }
        
    }

    void Shoot()
    {
        //needs cooldown
        if (Vector3.Distance(transform.position, bot.enemy.transform.position) < 40.0f)
        {
            //Ray ray;
            RaycastHit hit;
            Physics.Raycast(transform.position,transform.forward, out hit, 40.0f);
            Debug.DrawRay(transform.position, transform.forward, Color.green, 1.0f);
            if(hit.transform.tag == "Player")
            {
                object[] pars = new object[2];
                pars[0] = this.gameObject;
                pars[1] = -10;
                //Debug.Log("bot " + this.name +  " hit " + hit.collider.gameObject.name);
                hit.collider.SendMessage("UpdateHealth", pars);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        bot = gameObject.GetComponent<BotController>();
        //Debug.Log("enter shoot state" + bot.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StateUpdate()
    {
        MoveToShoot();
    }
}
