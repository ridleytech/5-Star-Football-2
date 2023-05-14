using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WRRouteAgent : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    public Animator animator;
    public Transform[] routePoints;
    public int routeIndex = 0;
    private bool isCoroutineExecuting = false;
    public bool runningRoute = false;

    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(target.position);
        agent.isStopped = true;
        //agent.speed = 0;
    }

    public void StartRoute(){

        agent.isStopped = false;
        runningRoute = true;
        //agent.speed = 1;
        //agent.SetDestination(target.position);
    }

//      IEnumerator ExecuteAfterTime(float time, Action task)
//  {
//      if (isCoroutineExecuting)
//          yield break;
//      isCoroutineExecuting = true;
//      yield return new WaitForSeconds(time);
//      task();
//      isCoroutineExecuting = false;
//  }

    // Update is called once per frame
    void FixedUpdate()
    {
         if(runningRoute){
            animator.SetFloat("ForwardMovement",5f);
         }

        Transform currentRoutePoint = routePoints[routeIndex];

        //Debug.Log("remainingDistance: "+agent.remainingDistance);

        if(runningRoute && agent.remainingDistance <= 1 && routeIndex < routePoints.Length-1){

            Debug.Log("update route index");

            routeIndex++;

            agent.SetDestination(routePoints[routeIndex].position);
        }
    }
}
