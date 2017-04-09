using System;
using UnityEngine;
using System.Collections;

public class Shoot : IBaseState
{
    BotController2 BC;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 10.0f;

    public float shootDistance = 40.0f;

    public float shootCoolDown = 0.5f;
    public float shootCoolDownReset = 0.5f;

    public float shootInnacuracy = 2f;

    // Use this for initialization
    void Start ()
    {
        BC = gameObject.GetComponent<BotController2>();
        if (BC == null)
        {
            throw new NullReferenceException("bc is null");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void MoveToShoot()
    {
        if (BC != null && BC.goEnGameObject != null)
        {
            if (Vector3.Distance(BC.goEnGameObject.transform.position, transform.position) > 5.0f)
            {
                Vector3 direction = BC.goEnGameObject.transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, BC.goEnGameObject.transform.position, moveSpeed * Time.deltaTime);
                Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
        
        if (BC.goEnGameObject.activeSelf == false)
        {
            BC.EnemyInRange = false;
        }

        ShootGun();

    }

    void ShootGun()
    {
        if (shootCoolDown < 0)
        {
            Debug.Log("Shooting!");
            if (Vector3.Distance(transform.position, BC.goEnGameObject.transform.position) < shootDistance)
            {
                // Inaccuracy factor
                Vector3 orig = transform.position;
                Vector3 dir = BC.goEnGameObject.transform.position - orig;
                dir = Quaternion.Euler(0, UnityEngine.Random.Range(-shootInnacuracy, shootInnacuracy), 0) * dir;

                // Raycast shooting
                RaycastHit hit;
                Physics.Raycast(transform.position, dir, out hit, 40.0f);
                Debug.DrawRay(transform.position, dir, Color.green, 1.0f);

                // Send hit bot a health update message.
                if (hit.transform != null)
                {
                    if (hit.transform.tag == "Player")
                    {
                       hit.collider.gameObject.GetComponent<BotController2>().Health -= 10;
                       hit.collider.gameObject.GetComponent<BotController2>().sender = this.gameObject;
                    }
                }
                BC.Ammo -= 1;
            }
            shootCoolDown = 0.5f;
        }
        else
        {
            shootCoolDown -= Time.deltaTime;
            return;
        }
    }

    public override void StateUpdate()
    {
        MoveToShoot();
    }
}
