using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rabbit : MonoBehaviour
{
    public NavMeshAgent agent; 
    public GameObject rabbitObject; 
    public float reachPoint; 

    //for checking if its in the same spot
    private float timeElapsed = 0.0f;
    private float timeThreshold = 5.0f;
    private Vector3 lastPosition;

    public float amountSpawned; 

    Vector3 navPosition; 

    private void Start() {
        //first 4 lines found from a unity forum, found here, has been editted and altered: https://forum.unity.com/threads/failed-to-create-agent-because-it-is-not-close-enough-to-the-navmesh.125593/
        //places the navagent onto the navmesh 
        Vector3 sourcePostion = rabbitObject.transform.position;//The position you want to place your agent
        NavMeshHit closestHit;
        if( NavMesh.SamplePosition(sourcePostion, out closestHit, 500, 1 ) ){
            rabbitObject.transform.position = closestHit.position;
        } 
        
        //gives the agent an intial first position to go to 
        navPosition =  RandomNavmeshLocation(reachPoint);

        lastPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //make the agent go to the first place 
        agent.SetDestination(navPosition);

        //change the location the agent is going to when it gets close to the old one
        if(Vector3.Distance(rabbitObject.transform.position, navPosition) < amountSpawned){
            navPosition = RandomNavmeshLocation(reachPoint);
        }

        if (Vector3.Distance(transform.position, lastPosition) < 5)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeThreshold)
            {
                navPosition = RandomNavmeshLocation(reachPoint);
            }
        }
        else
        {
            timeElapsed = 0.0f;
            lastPosition = transform.position;
        }
    }

    //finds a random position on the entire navmesh 
    //function sourced from Unity forums found here: https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html 
    public Vector3 RandomNavmeshLocation(float radius) {
         Vector3 randomDirection = Random.insideUnitSphere * radius;
         randomDirection += transform.position;
         NavMeshHit ya;
         Vector3 finalPosition = Vector3.zero;
         if (NavMesh.SamplePosition(randomDirection, out ya, radius, 1)) {
             finalPosition = ya.position;            
         }
         return finalPosition;
     }
}
