using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class BotController2 : MonoBehaviour {


    // States
    public IBaseState activeState;
    public Roam rStateRoam;
    public Shoot sShootState;
    public CollectHealth csCollectHealth;
    public CollectAmmo caCollectAmmo;

    // Bot Health.
    [SerializeField]
    private float _health;
    public virtual float Health
    {
        get { return _health; }
        set { _health = value; stateSwitcher(); }
    }

    // Bot Ammo.
    [SerializeField]
    private float _ammo;
    public virtual float Ammo
    {
        get { return _ammo; }
        set { _ammo = value; stateSwitcher(); }
    }

    // Bot Enemy Seen.
    [SerializeField]
    private bool _enemyInRange = false;
    public virtual bool EnemyInRange
    {
        get { return _enemyInRange; }
        set { _enemyInRange = value; stateSwitcher(); }
    }

    // Bot Target Enemy.
    public GameObject goEnGameObject;
    
    // Who Hit Me.
    private GameObject _sender;
    public virtual GameObject Sender
    {
        get { return _sender; }
        set { _sender = value; }
    }

    // Range for Fuzzy Logic
    private float _range;
    public virtual float Range
    {
        get { return _range; }
        set { _range = value; stateSwitcher();}
    }

    // Set Up Bot States And Set Roam State Active.
    public virtual void Awake()
    {
        rStateRoam = gameObject.GetComponent<Roam>();
        sShootState = gameObject.GetComponent<Shoot>();
        csCollectHealth = gameObject.GetComponent<CollectHealth>();
        caCollectAmmo = gameObject.GetComponent<CollectAmmo>();
        activeState = rStateRoam;
        activeState.enabled = true;
    }

    public virtual void stateSwitcher()
    {
        
        if (Health <= 50 && Health > 0) // Collect Health State
        {
            activeState.enabled = false;
            activeState = csCollectHealth;
            activeState.enabled = true;
            
        }
        else if (Ammo <= 0) // Collect Ammo State
        {
            activeState.enabled = false;
            activeState = caCollectAmmo;
            activeState.enabled = true;
            
        }
        else if (EnemyInRange) // Shoot State.
        {
            activeState.enabled = false;
            activeState = sShootState;
            activeState.enabled = true;
        }
        else if (Health > 50 || Ammo > 0 || !EnemyInRange) // Roam State.
        {
            activeState.enabled = false;
            activeState = rStateRoam;
            activeState.enabled = true;
            
        }
        if (Health <= 0) // Death.
        {
            this.gameObject.SetActive(false);
            GameManager2.Instance.UpdateDeaths(this.gameObject);
            Sender.SendMessage("IncreaseKillCount");
        }
    }

    public virtual void ResetEnemy()
    {
        Sender = null;
        goEnGameObject = null;
        EnemyInRange = false;
        Range = Mathf.Infinity;
    }

    public virtual void SetSender(GameObject temp)
    {
        Sender = temp;
    }

    public virtual void OnDisable()
    {
        Health = 100;
        Ammo = 10;
    }

    public virtual void IncreaseKillCount()
    {
        GameManager2.Instance.UpdateKills(this.gameObject);
        ResetEnemy();
    }



    // Use this for initialization
    public virtual void Start ()
    {
        
	}

    // Update is called once per frame
    public virtual void Update ()
    {
        activeState.StateUpdate();  
	}
}



[CustomEditor(typeof(BotController2))]
public class TakeDamage2 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BotController2 bot = (BotController2)target;

        if (GUILayout.Button("Take Damage"))
        {
            bot.Health -= 60;
        }

        if (GUILayout.Button("Drain Ammo"))
        {
            bot.Ammo -= 10;
        }
    }
}