using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEditor;
using System.Linq;
using System.Collections;

public class BotController : MonoBehaviour
{ 
    // States.
    private IBaseState activeState;
    public RoamState rs;
    public CollectHealthState cs;
    public ShootState ss;
    public CollectAmmoState ams;

    public bool enemySighted = false;
    public Transform enemy;

    float tempTime = 5.0f;
    float tempTimeReset = 5.0f;

    public GameObject sender;

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if(_health <= 0)
            {
                //Destroy(this.gameObject);
                this.gameObject.SetActive(false);
            }
            if(_health <= 50)
            {
                activeState.enabled = false;
                cs.enabled = true;
                activeState = cs;
            }
            if(_health >= 51)
            {
                activeState.enabled = false;
                rs.enabled = true;
                activeState = rs;
            }
            exitShootState();
        }
    }

    [SerializeField]
    private int _ammoAmount = 10;
    public int AmmoAmount
    {
        get { return _ammoAmount; }
        set
        {
            _ammoAmount = value;
            if(_ammoAmount <= 0)
            {
                // set seek ammo state.
                activeState.enabled = false;
                ams.enabled = true;
                activeState = ams;
            }
            if (_ammoAmount > 5)
            {
                activeState.enabled = false;
                rs.enabled = true;
                activeState = rs;
            }
            exitShootState();
        }
    }


    

    void Awake()
    {
        // state references.
        rs = GetComponent<RoamState>();
        cs = GetComponent<CollectHealthState>();
        ss = GetComponent<ShootState>();
        ams = GetComponent<CollectAmmoState>();

        // initialize states.
        rs.enabled = true;
        cs.enabled = false;
    }

	// Use this for initialization
	void Start ()
    {
        // Set intitial state.
        activeState = rs;
        // Set random colour 
        MeshRenderer mySkin = this.transform.GetComponent<MeshRenderer>();
        mySkin.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    void OnEnable()
    {
        Health = 100;
        AmmoAmount = 10;
    }

    // Update is called once per frame
    void Update()
    {
        // needs sorting out
        // need to see if roam state and then get closest enemy.
        
        // Run state specific code
        if (activeState != null)
        {
            activeState.StateUpdate();
        }

        // try changeing this to a list in a game manger class that keeps track of bots
        // bots can notify game manager of death.
        // need a way for each bot to register a kill.
        // maybe by accessing the health variable of the hit collider in shoot.
        foreach (GameObject go in GameManager.Instance.botsList)
        {
            if (go != null && AmmoAmount > 0)
            {
                // Check distance to enemy.
                if (Vector3.Distance(transform.position, go.transform.position) < 50.0f)
                {
                    // Check angle to enemy.
                    //Debug.DrawLine(transform.position, go.transform.position, Color.red, 0.5f);
                    Vector3 targetDir = go.transform.position - transform.position;
                    Vector3 forward = transform.forward;
                    float angle = Vector3.Angle(targetDir, forward);
                    if (angle < 20.0F)
                    {
                        //Debug.Log("Enemy Sighted " + this.name);
                        enemySighted = true;
                        enemy = go.transform;
                        activeState.enabled = false;
                        ss.enabled = true;
                        activeState = ss;
                    }
                }
            }
        }
        if (enemy != null)
        {
            if (enemy.gameObject.activeSelf == false)
            {
                enemySighted = false;
                activeState.enabled = false;
                rs.enabled = true;
                activeState = rs;
            }
        }
        
    }

    public void exitShootState()
    {
        // change to check for health and ammo.
        enemy = null;
        enemySighted = false;
        if(Health < 50)
        {
            activeState.enabled = false;
            cs.enabled = true;
            activeState = cs;
            return;
        }
        else if (AmmoAmount <= 0)
        {
            activeState.enabled = false;
            ams.enabled = true;
            activeState = ams;
            return;
        }
        else
        {
            activeState.enabled = false;
            rs.enabled = true;
            activeState = rs;
        }

    }

   

    /*public void UpdateHealth(object[] pars)
    { 
        // Move this to health prop .
        Health += (int)pars[1];
        sender = pars[0] as GameObject;
        Debug.Log(this.gameObject.name + "hit by " + sender.name);
        if (Health < 50)
        {
            activeState.enabled = false;
            cs.enabled = true;
            activeState = cs;   
        }
        else if (Health >= 50 && activeState == cs)
        {
            activeState.enabled = false;
            rs.enabled = true;
            activeState = rs;
        }
    }
    
    public void UpdateAmmo(object[] pars)
    {

    } */  
}

[CustomEditor(typeof(BotController))]
public class TakeDamage : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        BotController bot = (BotController)target;

        if(GUILayout.Button("Take Damage"))
        {
            bot.Health -= 100;
        }

        if (GUILayout.Button("Drain Ammo"))
        {
            bot.AmmoAmount -= 10;
        }
    }
}

/*[CustomEditor(typeof(BotController))]
public class DrainAmmo : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        BotController bot = (BotController)target;

        
    }
}*/


