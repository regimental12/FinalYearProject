using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using System.Linq;


public class ShootState : IBaseState
{
    public BotController bot;

    public Transform Enemy;

    // Use this for initialization
    void Start()
    {
        bot = gameObject.GetComponent<BotController>();
        Debug.Log("enter shoot state" + bot.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StateUpdate()
    {

    }
}
