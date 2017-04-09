using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;


public class GameManager2 : MonoBehaviour {

     
    #region Singleton declaration.
    private static GameManager2 instance = null;

    public static GameManager2 Instance
    {
        get { return instance; }
    }

    public object SceneManagment { get; private set; }

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Class level Variables

    // Setup option varialbes
    public int _noOfBots;
    public int _noOfHealthItems;
    public int _noOfAmmoItems;

    string bots = "5";
    string hItems = "5";
    string aItems = "5";

    public GameObject _goHealthItem;
    public GameObject _goAmmoitem;
    public GameObject _goWaypoints;
    public GameObject _goBot;

    public List<GameObject> _lwayPointList = new List<GameObject>();
    public List<GameObject> _lhealthItemList = new List<GameObject>();
    public List<GameObject> _lammoItemList = new List<GameObject>();

    public List<GameObject> _lBotList = new List<GameObject>();

    #endregion


    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnLevelWasLoaded(int level)
    {
        Instantiate(_goWaypoints);
        _lwayPointList = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        InstantiateBots();
        SetUpItems(_noOfHealthItems , _lhealthItemList , _goHealthItem);
        SetUpItems(_noOfAmmoItems , _lammoItemList , _goAmmoitem);
        SetBotsActive();
    }

    private void SetUpItems(int count , List<GameObject> list , GameObject objectToCreate)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go =
                Instantiate(objectToCreate,
                    _lwayPointList[UnityEngine.Random.Range(0, _lwayPointList.Count - 1)].transform.position,
                    transform.rotation) as GameObject;
            go.transform.position += new Vector3(UnityEngine.Random.Range(0, 10), 0, UnityEngine.Random.Range(0, 10));
            list.Add(go);
        }
    }

    private void InstantiateBots()
    {
        for (int i = 0; i < _noOfBots; i++)
        {
            GameObject go = Instantiate(_goBot, _lwayPointList[UnityEngine.Random.Range(0, _lwayPointList.Count - 1)].transform.position , transform.rotation) as GameObject;
            go.name = "BOT " + i;
            go.SetActive(false);
            _lBotList.Add(go);
        }
    }

    void SetBotsActive()
    {
        foreach (GameObject go in _lBotList)
        {
            go.SetActive(true);
        }
    }

    void OnGUI()
    {
        // Game Setup Options
        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            

            GUILayout.TextArea("Enter Number Of bots");
            bots = GUILayout.TextField(bots, 20);

            GUILayout.TextArea("Enter Number of Health Items");
            hItems = GUILayout.TextField(hItems, 20);

            GUILayout.TextArea("Enter Number of Ammo Items");
            aItems = GUILayout.TextField(aItems, 20);

            if (GUI.Button(new Rect(0, 175, 200, 30), "launch game"))
            {
                SceneManager.LoadScene(1);
                _noOfBots = Convert.ToInt32(bots);
                _noOfHealthItems = Convert.ToInt32(hItems);
                _noOfAmmoItems = Convert.ToInt32(aItems);
            }
        }
        // In Game Options
        else
        {
            if (GUI.Button(new Rect(10, 35, 200, 30), "Increase Speed"))
            {
                Time.timeScale = 2.0f;
            }

            // camera switcher.
            if (GUI.Button(new Rect(10, 70, 200, 30), "Switch Camera"))
            {
            }
        }
    }
}
