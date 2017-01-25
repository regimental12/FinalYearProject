using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {


    public List<GameObject> bots = new List<GameObject>();
    public List<Camera> cameras = new List<Camera>();

    public Camera currentCam;
    public int currentCamNo = 0;


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
        bots = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    // Use this for initialization
    void Start ()
    {
	    foreach(GameObject go in bots)
        {
            cameras.Add(go.GetComponentInChildren<Camera>());
        }
        foreach(Camera cam in cameras)
        {
            cam.enabled = false;
        }
        cameras.Add(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>()) ;
        currentCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	      
	}

    void FixedUpdate()
    {
        foreach(GameObject go in bots)
        {
            if(go.activeSelf == false)
            {
                bots.Remove(go);
            }
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Switch Camera"))
        {
            // TODO siwtch cameras.
            // need curent cam and next cam.
            currentCam.enabled = false;
            currentCam = cameras[currentCamNo];
            currentCam.enabled = true;
            currentCamNo++;
            if(currentCamNo >= cameras.Count())
            {
                currentCamNo = 0;
            }
        }
    }
}
