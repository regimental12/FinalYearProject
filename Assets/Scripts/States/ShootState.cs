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
    public float shootDistance = 40.0f;

    public float shootCoolDown = 0.5f;
    public float shootCoolDownReset = 0.5f;

    public float shootInnacuracy = 2f;

    public void MoveToShoot()
    {
        if (bot.enemy != null)
        {
            // Bot chasing and stop bots getting to close
            if (Vector3.Distance(bot.enemy.transform.position, transform.position) > 5.0f)
            {
                direction = bot.enemy.transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, bot.enemy.transform.position, moveSpeed * Time.deltaTime);
                Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
        else
        {
            bot.exitShootState();
        }

        Shoot();

    }

    void Shoot()
    {
        // Shooting cooldown
        if(shootCoolDown > 0)
        {
            shootCoolDown -= Time.deltaTime;
            return;
        }
        if (shootCoolDown < 0 && bot.AmmoAmount > 0)
        {
            if (Vector3.Distance(transform.position, bot.enemy.transform.position) < shootDistance)
            {
                // Inaccuracy factor
                Vector3 orig = transform.position;
                Vector3 dir = bot.enemy.transform.position - orig;
                dir = Quaternion.Euler(0, Random.Range(-shootInnacuracy, shootInnacuracy), 0) * dir;

                // Raycast shooting
                RaycastHit hit;
                Physics.Raycast(transform.position, dir, out hit, 40.0f);
                Debug.DrawRay(transform.position, dir, Color.green, 1.0f);

                // Send hit bot a health update message.
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Player")
                    {
                        // Need to change this so mesg still sent.
                        /*object[] pars = new object[2];
                        pars[0] = this.gameObject;
                        pars[1] = -10;
                        collider.SendMessage("UpdateHealth", +10);
                        Debug.Log("bot " + this.name +  " hit " + hit.collider.gameObject.name);*/
                        hit.collider.gameObject.GetComponent<BotController>().Health -= 10;
                    }
                }
                bot.AmmoAmount -= 1;
            }
            shootCoolDown = 0.5f;
        }
    }

    // Use this for initialization
    void Start()
    {
        bot = gameObject.GetComponent<BotController>();
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
