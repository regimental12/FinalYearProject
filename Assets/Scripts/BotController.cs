using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEditor;
using System.Linq;

public class BotController : MonoBehaviour
{
    private IBaseState activeState;
    
    [SerializeField]
    private int _health = 1000;

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if(_health <= 0)
            { 
                Destroy(this.gameObject);
            }
        }
    }

    public RoamState rs;
    public CollectHealthState cs;
    public ShootState ss;

    public List<GameObject> players = new List<GameObject>();
    public bool enemySighted = false;
    public Transform enemy;

    float tempTime = 5.0f;
    float tempTimeReset = 5.0f;

    public GameObject sender;

    void Awake()
    {
        // state references.
        rs = GetComponent<RoamState>();
        cs = GetComponent<CollectHealthState>();
        ss = GetComponent<ShootState>();

        // initialize states.
        rs.enabled = true;
        cs.enabled = false;
    }

	// Use this for initialization
	void Start ()
    {
        activeState = rs;

        players = GameObject.FindGameObjectsWithTag("Player").ToList();

        MeshRenderer mySkin = this.transform.GetComponent<MeshRenderer>();
        mySkin.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
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
        foreach (GameObject go in players)
        {
            if (go != null)
            {
                if (Vector3.Distance(transform.position, go.transform.position) < 50.0f)
                {
                    Debug.DrawLine(transform.position, go.transform.position, Color.red, 0.5f);
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
        if (enemy == null)
        {
            enemySighted = false;
            activeState.enabled = false;
            rs.enabled = true;
            activeState = rs;
        }
        
    }

    public void exitShootState()
    {
        enemy = null;
        enemySighted = false;
        activeState.enabled = false;
        rs.enabled = true;
        activeState = rs;
        players = GameObject.FindGameObjectsWithTag("Player").ToList();

    }

    public void UpdateHealth(object[] pars)
    { 
        // need to figure out a trigger for events.
        Health += (int)pars[1];
        sender = pars[0] as GameObject;
        Debug.Log("hit by someone" + sender.name);
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
            //bot.UpdateHealth(-10);
        }
    }
}
