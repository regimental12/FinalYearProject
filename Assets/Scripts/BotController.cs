using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

public class BotController : MonoBehaviour {

    public static BotController instance;

    private IBaseState activeState;

    public int health = 100;

    public RoamState rs;
    public CollectHealthState cs;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        rs = GetComponent<RoamState>();
        cs = GetComponent<CollectHealthState>();

        rs.enabled = true;
        cs.enabled = false;
    }

	// Use this for initialization
	void Start ()
    {
        activeState = rs;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(activeState != null)
        {
            activeState.StateUpdate();
        } 
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 100, 30), "Do Damage"))
        {
            UpdateHealth(-10);
        }
    }

    public void UpdateHealth(int amount)
    {
        health += amount;
        if (health < 49)
        {
            activeState.enabled = false;
            cs.enabled = true;
            activeState = cs;   
        }
        if (health > 50)
        {
            activeState.enabled = false;
            rs.enabled = true;
            activeState = rs;
        }
    }
}
