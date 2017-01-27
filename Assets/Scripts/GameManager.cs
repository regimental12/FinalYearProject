using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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

    public int inactive = 0;


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
        // Fill list botslList and setup camera switching.
        cameraList.Add(currentCam);
        WayPoints = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        for (int i = 0; i < numOfBots; i++ )
        {
            GameObject go = Instantiate(bot);
            go.SetActive(false);
            go.name = "BOT " + i  + "";
            botsList.Add(go);
            
            cameraList.Add(go.GetComponentInChildren<Camera>());
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

    

    void FixedUpdate()
    {
        
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Switch Camera"))
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
        }
    }
}
