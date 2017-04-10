using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CollectAmmo : IBaseState {


    public float moveSpeed = 10f;
    public float rotSpeed = 0.2f;

    public BotController2 BC;

    public GameObject ammoItem;

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

    void MoveToAmmoItem()
    {
        GetNearestAmmoItem();
        Vector3 direction = ammoItem.transform.position - transform.position;
        if (transform.position != ammoItem.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, ammoItem.transform.position, moveSpeed * Time.deltaTime);
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public void GetNearestAmmoItem()
    {
        ammoItem = GameManager2.Instance._lammoItemList.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
        if (ammoItem.gameObject.GetComponent<AmmoItem>().active == false)
        {
            ammoItem = GameManager2.Instance._lammoItemList.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).Skip(1).First();
        }
    }

    public override void StateUpdate()
    {
        MoveToAmmoItem();
    }
}
