using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DL : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject target;
    public Vector3 destination;
    public float stoppingDistance;
    public float speed; 
    // Start is called before the first frame update
    public GameManager gm;
    public bool playStarted;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        agent.stoppingDistance = stoppingDistance;
        agent.speed = speed;
        //agent.destination = target.transform.position;

    }

        void startPlay ()
        {
            //Debug.Log("startPlay DL");
            agent.destination = target.transform.position;  
    }

    // Update is called once per frame
    void Update()
    {

        if(gm.playStarted )
        {
             if(!playStarted)
            {
                    playStarted = true;
                    startPlay();
            }
   destination = target.transform.position;
        agent.destination = destination;

        }

// if(gm.playStarted){
//    agent.destination = target.transform.position;  
// }
        // destination = target.transform.position;
        // agent.destination = destination;

    }
}
