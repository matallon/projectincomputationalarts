using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class human : MonoBehaviour
{
    NavMeshAgent agent; 
    Animator animator; 

    //public GameObject humanObject; 
    public float reachPoint; 

    //for checking if its in the same spot
    private float timeElapsed = 0.0f;
    private float timeThreshold = 30.0f;
    private Vector3 lastPosition;

    Vector3 navPosition; 
    string collidedTag; 

    // time ==========================================
    [SerializeField]
    private DateTime currentTime; 

    private TimeSpan sunriseTime = TimeSpan.FromHours(7); 
    private TimeSpan sunsetTime = TimeSpan.FromHours(21);

    [SerializeField]
    private bool dayTime = true; 

    [SerializeField]
    private int startTime = 12;
    [SerializeField]
    private int timeMultiplier = 100; 

    public GameObject firePit;
    
    // ============================================== states 
    //for state machine 
    enum AgentState
    {
        Idle,
        Walking,
        Farming, 
        Eating, 
        RainedOn, 
        inBed,
        fireTurnOn,
    }

    //inialise the enum 
    [SerializeField]
    AgentState currentState;

    //counters for changing states 
    [SerializeField]
    float hunger = 1f; 
    [SerializeField]
    float tired = 1f; 

    // ==============================================

    private void Start() {
        //get all the components 
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        firePit = GameObject.FindGameObjectWithTag("fire");

        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startTime);
        
        //first 4 lines found from a unity forum, found here, has been editted and altered: https://forum.unity.com/threads/failed-to-create-agent-because-it-is-not-close-enough-to-the-navmesh.125593/
        //places the navagent onto the navmesh 
        Vector3 sourcePostion = transform.position;//The position you want to place your agent
        NavMeshHit closestHit;
        if( NavMesh.SamplePosition(sourcePostion, out closestHit, 500, 1 ) ){
            transform.position = closestHit.position;
        } 
        
        //gives the agent an intial first position to go to 
        navPosition =  RandomNavmeshLocation(reachPoint);
        lastPosition = transform.position;

        currentState = AgentState.Walking;
    }

    // Update is called once per frame
    void Update(){

          //================================================== switch the states 
        switch(currentState)
        {
            case AgentState.Walking:
                goRandomPositions(); 
                break;
            case AgentState.inBed:
                goToBed();
                break;
            case AgentState.fireTurnOn:
                turnOnFire();
                break; 
            case AgentState.Eating:
                doEating();
                break;
            case AgentState.RainedOn:
                Debug.Log("beingRainedOn");
                break;
        } 
        // ===============================================

        //update time 
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
    
        // =================================== change the states 
        //bedtime 
        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime){
            dayTime = true; 
        } else {
            dayTime = false; 
        } 

        if(!dayTime){
            currentState = AgentState.inBed;
        } else {
            checkForRain();

            if(!firePit.activeSelf){
                currentState = AgentState.fireTurnOn;
            } else {
                currentState = AgentState.Walking;
            }
        }
        // =============================================
        //see if the fire is out
    }

    //============== WALKING STATE========================
    public void goRandomPositions(){
        //stop them from going too high up 
        if(navPosition.y > 116){
            navPosition = RandomNavmeshLocation(reachPoint);
        }

        agent.isStopped = false; 
        animator.SetBool("walking", true); 
        //animator.SetBool("digging", false);
        animator.SetBool("eating", false);
        agent.SetDestination(navPosition);
        
        //change the location the agent is going to when it gets close to the old one
        if(Vector3.Distance(transform.position, navPosition) < 10){
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

         Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
         randomDirection += transform.position;
         NavMeshHit ya;
         Vector3 finalPosition = Vector3.zero;
         if (NavMesh.SamplePosition(randomDirection, out ya, radius, 1)) {
             finalPosition = ya.position;            
         }
         return finalPosition;
    }   

    //============== EATING STATE ========================
    void doEating(){
        //agent.isStopped = true; 
        GameObject eatingPlace = GameObject.FindGameObjectWithTag("eatingArea");

        if (eatingPlace != null){
            Transform eatingPlaceTransform = eatingPlace.transform; 
            agent.SetDestination(eatingPlaceTransform.position);
            if(Vector3.Distance(transform.position, eatingPlaceTransform.position) < 10){
                agent.isStopped = true; 
                animator.SetBool("walking",false);
                animator.SetBool("eating", true); 
                hunger -= UnityEngine.Random.Range(0.001f, 0.0005f);
            }
        } else{
            Debug.Log("couldn't find an eating place");
        }
    }

    //================ FARMING STATE ==========
    void doFarming(){
        animator.SetBool("walking", false); 
        //animator.SetBool("digging", true); 

        tired += UnityEngine.Random.Range(0.001f, 0.005f);
    }

    //================ BEDTIME!!!!
    private void goToBed(){
        //agent.isStopped = true;
        GameObject bedPlace; 

        if(gameObject.name == "megan(Clone)"){
            bedPlace = GameObject.FindGameObjectWithTag("dave");
        } else if (gameObject.name == "dave(Clone)"){
            bedPlace = GameObject.FindGameObjectWithTag("dave");
        } else if(gameObject.name == "anna(Clone)"){
            bedPlace = GameObject.FindGameObjectWithTag("dave");
        } else if (gameObject.name == "blake(Clone)"){
            bedPlace = GameObject.FindGameObjectWithTag("dave");
        } else {
            bedPlace = GameObject.FindGameObjectWithTag("dave");
        }

        if (bedPlace != null){
            Transform bedPlaceTransform = bedPlace.transform; 
            agent.SetDestination(bedPlaceTransform.position);
            if(Vector3.Distance(transform.position, bedPlaceTransform.position) < 3){
                agent.isStopped = true; 
            }
        }
    }

    private void turnOnFire(){
         if (firePit != null){
            Transform firePitTransform = firePit.transform; 
            agent.SetDestination(firePitTransform.position);
            if(Vector3.Distance(transform.position, firePitTransform.position) < 8){
                firePit.SetActive(true);
            }
        }
    }

    public void checkForRain(){
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit;

        // Check if the ray hits an object with the 'rain' tag
        if (Physics.Raycast(ray, out hit, 1000f) && hit.collider.CompareTag("rain"))
        {
            animator.SetBool("walking",false);
            animator.SetBool("eating", false); 
            animator.SetBool("rain", true); 
        } else {
            animator.SetBool("walking",true);
            animator.SetBool("eating", false); 
            animator.SetBool("rain", false); 
        }
    }
}
