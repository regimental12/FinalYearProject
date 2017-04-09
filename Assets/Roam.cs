using UnityEngine;
using System.Collections;

public class Roam : IBaseState {

    BotController2 BC;

    private float moveSpeed = 10f;
    private float rotSpeed = 0.2f;
    Vector3 direction;

    [SerializeField]
    private int _wayPointCounter = 0;
    [SerializeField]
    private int _wayPointListLength;

	// Use this for initialization
	void Start ()
    {
        BC = GetComponent<BotController2>();
        _wayPointListLength = GameManager2.Instance._lwayPointList.Count;
        _wayPointCounter = UnityEngine.Random.Range(0, _wayPointListLength);
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void CheckEnemyRange()
    {
        foreach (GameObject GO in GameManager2.Instance._lBotList)
        {
            if (Vector3.Distance(transform.position, GO.transform.position) < 50.0f)
            {
                Vector3 targetDir = GO.transform.position - transform.position;
                Vector3 forward = transform.forward;
                float angle = Vector3.Angle(targetDir, forward);
                if (angle < 20.0F)
                {
                    BC.EnemyInRange = true;
                    BC.goEnGameObject = GO;
                }
            }
        }
    }

    private void Move()
    {
        Vector3 destinVector3 = GameManager2.Instance._lwayPointList[_wayPointCounter].transform.position;
        direction = destinVector3 - transform.position;

        if (direction.magnitude >= 5)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinVector3, moveSpeed * Time.deltaTime);
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotSpeed, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
            return;
        }
        else
        {
            _wayPointCounter = Random.Range(0, _wayPointListLength);
            //Debug.Log(_wayPointCounter);
        }
    }


    public override void StateUpdate()
    {
        CheckEnemyRange();
        Move();
    }

}
