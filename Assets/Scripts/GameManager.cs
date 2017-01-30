using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // public int for no. of bots
    // fill list of bots
    // spawn bots at random points
    // update list on bots death (set inactive)
    // start timer for bot respwan 
    // reset bot.
    // reactivatebot at random point on timer completion.

    public int numOfBots = 5;
    public GameObject bot;
    public List<GameObject> botsList = new List<GameObject>();
    public List<Camera> cameraList = new List<Camera>();
    public List<GameObject> WayPoints = new List<GameObject>();
    public float respawnTimer = 5.0f;

    public Camera currentCam;
    public int currentCamNo = 1;

    public GameObject goHealthItem;
    public int _healthITems = 5;
    //public List<HealthItem> LHealthItems = new List<HealthItem>();

    public GameObject goAmmoItem;
    public int _ammoItem = 5;
    //public List<AmmoItem> LAmmoItem = new List<AmmoItem>();


    public List<BotData> BotListSaveData = new List<BotData>();

    // Singleton declaration.
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get { return instance; }
    }

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

    // Use this for initialization
    void Start ()
    {
        // Move into seperate functions.

        // implement a main menu system. new scene?

        // include no. of bots , no. of ammo/health items , timer for sim to run.

        // Fill list botslList and setup camera switching.
        
    }

    void OnLevelWasLoaded()
    {
        cameraList.Add(Camera.main);
        WayPoints = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        for (int i = 0; i < numOfBots; i++)
        {
            GameObject go = Instantiate(bot);
            go.SetActive(false);
            go.name = "BOT " + i + "";
            botsList.Add(go);

            cameraList.Add(go.GetComponentInChildren<Camera>());
        }

        for (int i = 0; i < _healthITems; i++)
        {
            Random.seed = (int)Time.time + (int)Random.Range(0, 101);
            int wayPointCounter = Random.Range(0, WayPoints.Count());

            GameObject go = Instantiate(goHealthItem);
            go.transform.position = WayPoints[wayPointCounter].transform.position + new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10));
            //go.transform.rotation = WayPoints[wayPointCounter].transform.rotation;

            Random.seed = (int)Time.time + (int) Random.Range(0, 101);
            wayPointCounter = Random.Range(0, WayPoints.Count());
            go = Instantiate(goAmmoItem);
            go.transform.position = WayPoints[wayPointCounter].transform.position + new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10));
            //go.transform.rotation = WayPoints[wayPointCounter].transform.rotation;
        }

        setBotsActive();
        currentCam.enabled = true;
    }

    public void setBotsActive()
    {
        foreach(GameObject go in botsList)
        {
            if (go.gameObject.activeSelf == false)
            {
                int wayPointCounter = Random.Range(0, WayPoints.Count());
                go.SetActive(true);
                go.GetComponentInChildren<Camera>().enabled = false;
                go.transform.position = WayPoints[wayPointCounter].transform.position;
                go.transform.rotation = WayPoints[wayPointCounter].transform.rotation;
                BotListSaveData.Add(new BotData(go.name));
            }
            
        }
    }

    
	
	// Update is called once per frame
	void Update ()
    {
        foreach(GameObject go in botsList)
        {
            if(go.activeSelf == false)
            {
                StartCoroutine(respawn(go));
            }
        }
	}

    IEnumerator respawn(GameObject bot)
    {
        if(respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;
        }
        if(respawnTimer < 0)
        {
            bot.SetActive(true);
            respawnTimer = 5.0f;
        }
        yield return null;
    }

    void OnApplicationQuit()
    {
        SaveData.SaveBotData(BotListSaveData);
    }

    void FixedUpdate()
    {
        
    }

    void OnGUI()
    {
        /*if (GUI.Button(new Rect(10, 70, 200, 30), "Switch Camera"))
        {
            // TODO sort out blank cameras.
            if (cameraList[currentCamNo].gameObject.activeSelf == false)
            {
                currentCamNo++;
                return;
            }
            else if(cameraList[currentCamNo].gameObject.activeSelf == true)
            {
                currentCam.enabled = false;
                currentCam = cameraList[currentCamNo];
                currentCam.enabled = true;
                currentCamNo++;
                if (currentCamNo >= cameraList.Count())
                {
                    currentCamNo = 0;
                }
            }
        }*/

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (GUI.Button(new Rect(10, 140, 200, 30), "launch game"))
            {
                SceneManager.LoadScene(1);

                //SetUp();
            }
        }

        if (GUI.Button(new Rect(10, 35, 200, 30), "increase Speed"))
        {
            Time.timeScale = 2.0f;
        }

    }
}

public class BotData
{
    private string _botName;
    public string BotName { get; set; }

    private int _kills = 0;
    public int Kills { get; set; }

    private int _deaths = 0;
    public int Deaths{ get; set; }

    public BotData(string name)
    {
        BotName = name;
    }

    
}
