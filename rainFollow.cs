using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//script to make the rain particle system follow the player object, and take all the rotations on too. 
//also turns the rain on and off again with input from the player. 

public class rainFollow : MonoBehaviour
{   
    private thirdPersonActionAsset playerActionsAsset; //gets the player input component
    private InputAction rainInput;     //and then specifically the part tracking rain input buttons (left control, triggers on the gamepad) 
    ParticleSystem rain; 
    public GameObject cloudToFollow;   //the third person player object 
    Vector3 cloudRotation;

    private void Awake()
    {
        playerActionsAsset = new thirdPersonActionAsset(); 
    }

    private void OnEnable()
    {
        rainInput = playerActionsAsset.Player.Rain;    //assign this to be the correct part of the component. 
        playerActionsAsset.Player.Enable();          //turn it on!! 
    }

    private void OnDisable() {
        playerActionsAsset.Player.Disable();           //destructor 
    }

    // Start is called before the first frame update
    void Start()
    {
        rain = GetComponent<ParticleSystem>();      //this script is attatched to the particle system 
        cloudRotation = cloudToFollow.transform.eulerAngles; //gets rotation of the cloud 
    }

    // Update is called once per frame
    void Update()
    {   
        //put the rain under the cloud, at the correct rotation.
        rain.transform.position = new Vector3(cloudToFollow.transform.position.x, cloudToFollow.transform.position.y -5, cloudToFollow.transform.position.z);
        rain.transform.rotation = cloudToFollow.transform.rotation;
        
        //checking for input 
        if (rainInput.triggered)
        {
            ToggleParticles();
        }
    }
    
    //turns rain on and off, also changes the tag of the object so that if it is raining, 
    //the humans can tell that there is rain above them, or if it is not raining they know the object above them is only tagged 'cloud' 
    void ToggleParticles()
    {
        if (rain.isPlaying)
        {
           rain.Stop(); 
           gameObject.tag = "cloud"; 
        }
        else
        {
            rain.Play();
            gameObject.tag = "rain"; 
        }
    }
}
