using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class BotController2 : MonoBehaviour {


    // States
    IBaseState activeState;
    Roam rStateRoam;
    Shoot sShootState;
    CollectHealth csCollectHealth;
    // CollectAmmo

    [SerializeField]
    private float _health;
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (Health < 0)
            {
                this.gameObject.SetActive(false);
            }
            stateSwitcher();
        }
    }

    [SerializeField]
    private float _ammo;
    public float Ammo
    {
        get { return _ammo; }
        set { _ammo = value; stateSwitcher(); }
    }

    [SerializeField]
    private bool _enemyInRange = false;
    public bool EnemyInRange
    {
        get { return _enemyInRange; }
        set { _enemyInRange = value; stateSwitcher(); }
    }

    public GameObject goEnGameObject;
    public GameObject sender;


    private void stateSwitcher()
    {
        if (Health <= 50 && Health > 0)
        {
            activeState.enabled = false;
            activeState = csCollectHealth;
            activeState.enabled = true;
        }
        else if (Ammo <= 0)
        {
            // Set Collect Ammo Active.
            activeState.enabled = false;
            activeState = rStateRoam;
            activeState.enabled = true;
        }
        else if (EnemyInRange)
        {
            activeState.enabled = false;
            activeState = sShootState;
            activeState.enabled = true;
        }
        else if (Health > 50 || Ammo > 0 || !EnemyInRange)
        {
            activeState.enabled = false;
            activeState = rStateRoam;
            activeState.enabled = true;
        }
        else if (Health < 0)
        {
            
        }

    }

    void Awake()
    {
        rStateRoam = gameObject.GetComponent<Roam>();
        sShootState = gameObject.GetComponent<Shoot>();
        csCollectHealth = gameObject.GetComponent<CollectHealth>();
        activeState = rStateRoam;
        activeState.enabled = true;
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        activeState.StateUpdate();
	}
}
[CustomEditor(typeof(BotController2))]
public class TakeDamage2 : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        BotController2 bot = (BotController2)target;

        if (GUILayout.Button("Take Damage"))
        {
            bot.Health -= 60;
        }

        /*if (GUILayout.Button("Drain Ammo"))
        {
            bot.AmmoAmount -= 10;
        }*/
    }
}