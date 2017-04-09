using UnityEngine;
using System.Collections;

public class HealthItem : MonoBehaviour
{
    bool active = true;
    float timeOut = 5.0f;
    float timeOutReset = 5.0f;
    public GameObject cube;
    public Collider col;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            cube.SetActive(false);
            col.enabled = false;
            active = false;
            collider.gameObject.GetComponent<BotController2>().Health += 50;
            Debug.Log("Collect health");
        }
          
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            timeOut -= Time.deltaTime;
            if (timeOut <= 0)
            {
                cube.SetActive(true);
                col.enabled = true;
                active = true;
                timeOut = timeOutReset;
            }
        }
    }
}
