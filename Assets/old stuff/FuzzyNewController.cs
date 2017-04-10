using UnityEngine;
using System.Collections;

using UnityEditor;

public class FuzzyNewController : MonoBehaviour {

    // Class level variables
    #region

    public FuzzyLogic fLogic = new FuzzyLogic();
    public IBaseState activeState;

    [SerializeField]
    private float _health;
    public float Health
    {
        get{ return _health; }
        set{ _health = value; stateSwitcher(); }
    }

    [SerializeField]
    private float _ammo;
    public float Ammo
    {
        get { return _ammo; }
        set { _ammo = value; stateSwitcher(); }
    }

    private float _range;
    public float Range
    {
        get { return _range; }
        set { _range = value; stateSwitcher(); }
    }

    public RoamState rsState;
    public CollectHealthState chState;
    public CollectAmmoState caState;
    public ShootState sState;

    #endregion

    // State Switcher
    private void stateSwitcher()
    {
        float result = 0;

        //result = fLogic.getValues(Health, Ammo, Range);
        Debug.Log(result);

        if (result <= -2)
        {
            activeState.enabled = false;
            chState.enabled = true;
            activeState = chState;
            Debug.Log(this.name + result);
        }
        else if (result < 0 && result > -2)
        {
            activeState.enabled = false;
            caState.enabled = true;
            activeState = caState;
            Debug.Log(this.name + result);
        }
        else if (result > 0 && result < 1)
        {
            activeState.enabled = false;
            rsState.enabled = true;
            activeState = rsState;
            Debug.Log(this.name + result);
        }
        else if (result >= 1)
        {
            activeState.enabled = false;
            sState.enabled = true;
            activeState = sState;
            Debug.Log(this.name + result);
        }

    }

    void Awake()
    {
        // state references.
        rsState = GetComponent<RoamState>();
        chState = GetComponent<CollectHealthState>();
        sState = GetComponent<ShootState>();
        caState = GetComponent<CollectAmmoState>();

        // initialize states.
        rsState.enabled = true;
        activeState = rsState;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        activeState.StateUpdate();

        foreach (var bot in GameManager.Instance.botsList)
        {
            if (Vector3.Distance(bot.transform.position, transform.position) < 100)
            {
                Range = Vector3.Distance(bot.transform.position, transform.position);
            }
        }
    }

    public GameObject sender;
    public void increaseKillCount()
    {
        foreach (BotData bot in GameManager.Instance.BotListSaveData)
        {
            if (bot.BotName == sender.name)
            {
                bot.Kills++;
            }

        }
    }
}

[CustomEditor(typeof(FuzzyNewController))]
public class TakeDamage : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        FuzzyNewController bot = (FuzzyNewController)target;

        if (GUILayout.Button("Take Damage"))
        {
            bot.Health -= 10;
        }

        if (GUILayout.Button("Drain Ammo"))
        {
            bot.Ammo -= 1;
        }
    }
}
