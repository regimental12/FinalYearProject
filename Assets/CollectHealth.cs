using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class CollectHealth : IBaseState
{
    public float moveSpeed = 10f;
    public float rotSpeed = 0.2f;

    public BotController2 BC;

    public GameObject healthItem;

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

    void MoveTOHealthItem()
    {
        GetNearestHealthItem();
        Vector3 direction = healthItem.transform.position - transform.position;
        if (transform.position != healthItem.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position , healthItem.transform.position , moveSpeed * Time.deltaTime);
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public void GetNearestHealthItem()
    {
        healthItem = GameManager2.Instance._lhealthItemList.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();   
    }

    public override void StateUpdate()
    {
        MoveTOHealthItem();
    }
}
