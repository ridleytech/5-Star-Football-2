using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointTest : MonoBehaviour
{
        public GameObject QB;
        public GameObject playerAssignment;
        public GameObject blockDestination;
        public bool isPass;
        Rigidbody rb;
        public UnityEngine.AI.NavMeshAgent agent;
        public Vector3 destination;
        public float stoppingDistance;
        public float speed;
        public float pushForce;
        public GameManager gm;
        public bool playStarted;

        void Start()
        {
                gm = GameObject.Find("GameManager").GetComponent<GameManager>();

                //agent.stoppingDistance = stoppingDistance;
                agent.speed = speed;
                agent.updateRotation = false;

                //agent.destination = playerAssignment.transform.position;
        }

        void startPlay (){
                //Debug.Log("startPlay OL");

                agent.destination = playerAssignment.transform.position;
        }

        void OnTriggerStay(Collider other){

                if(other.gameObject.name == "DL")
                {
                        //Debug.Log("collided2");

                        Vector3 dir = other.transform.position - transform.position;
                        dir = -dir.normalized;

                        rb = other.gameObject.GetComponent<Rigidbody>();
                        rb.AddForce(Vector3.forward * pushForce);
                }
        }

        // Update is called once per frame
        void Update()
        {
                if(gm.playStarted ){

                        if(!playStarted) 
                        {
                                playStarted = true;
                                startPlay();
                        }

                // print("z: "+transform.position.z);


                        //destination = playerAssignment.transform.position + (QB.transform.position - playerAssignment.transform.position) / 2;

                        if(isPass)
                        {
                                destination = playerAssignment.transform.position + (QB.transform.position - playerAssignment.transform.position) / 2;

                        }
                        else
                        {
                                destination = Vector3.Lerp(transform.position,playerAssignment.transform.position,0.5f);

                        }

                        transform.LookAt(playerAssignment.transform);

                        agent.destination = destination; 
                }
        }

        // Update is called once per frame
        //     void Update1()
        //     {
        //         // print("z: "+transform.position.z);


        //            //destination = playerAssignment.transform.position + (QB.transform.position - playerAssignment.transform.position) / 2;

        //         destination = Vector3.Lerp(transform.position,playerAssignment.transform.position,0.5f);

        //         agent.destination = destination;



        //         return;



        //         // destination = playerAssignment.transform.position;
        //         // agent.destination = destination;


        //         Vector3 direction;




        //         if(isPass)
        //         {
        //                 direction = playerAssignment.transform.position + (QB.transform.position - playerAssignment.transform.position) / 2;
        //         }
        //         else
        //         {
        //                 //direction = blockDestination.transform.position;
        //                 //direction = blockDestination.transform.position + (playerAssignment.transform.position - blockDestination.transform.position) / 2;

        //                 direction = playerAssignment.transform.position;
        //         }

        //         transform.position = direction;
        //     }
}
