using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using System.Linq;

public class CollectAmmoState : IBaseState
{
    public List<GameObject> AmmoItems = new List<GameObject>();
    public float moveSpeed = 10f;
    public float rotSpeed = 0.2f;
    Vector3 direction;
    public int nextItem = 0;
    public BotController bot;

    // Use this for initialization
    void Start ()
    {
        bot = GetComponent<BotController>();
        AmmoItems = GameObject.FindGameObjectsWithTag("AmmoItem").ToList();
        AmmoItems = AmmoItems.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void StateUpdate()
    {
        MoveToAmmo();
    }

    public void MoveToAmmo()
    {
        direction = AmmoItems[nextItem].transform.position - transform.position;
        if (transform.position != AmmoItems[nextItem].transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, AmmoItems[nextItem].transform.position, moveSpeed * Time.deltaTime);

            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
            return;
        }
        nextItem++;
        if (nextItem > AmmoItems.Count() - 1)
        {
            nextItem = 0;
        }
    }
}
