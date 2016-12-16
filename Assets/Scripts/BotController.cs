using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEditor;
using System.Linq;

public class BotController : MonoBehaviour
{

    public BotController instance;

    private IBaseState activeState;

    public int health = 100;

    public RoamState rs;
    public CollectHealthState cs;
    public ShootState ss;

    public List<GameObject> players = new List<GameObject>();
    public bool enemySighted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        rs = GetComponent<RoamState>();
        cs = GetComponent<CollectHealthState>();
        ss = GetComponent<ShootState>();

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
        if (activeState != null)
        {
            activeState.StateUpdate();
        }
        if (!enemySighted)
        {
            foreach (GameObject go in players)
            {
                if (Vector3.Distance(transform.position, go.transform.position) < 20.0f)
                {
                    Vector3 targetDir = go.transform.position - transform.position;
                    Vector3 forward = transform.forward;
                    float angle = Vector3.Angle(targetDir, forward);
                    if (angle < 15.0F)
                    {
                        Debug.Log("Enemy Sighted " + this.name);
                        enemySighted = true;
                        activeState.enabled = false;
                        ss.enabled = true;
                        activeState = ss;

                    }
                }
            }
        }
    }

    

    public void UpdateHealth(int amount)
    {
        health += amount;
        if (health <= 49)
        {
            activeState.enabled = false;
            cs.enabled = true;
            activeState = cs;   
        }
        if (health >= 50)
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
            bot.UpdateHealth(-10);
        }
    }
}
