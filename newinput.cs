using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class newInput : MonoBehaviour
{

    //accesses the new input component. 
    private thirdPersonActionAsset playerActionsAsset;
    private InputAction move;
    private InputAction jump; 
    private InputAction down; 

    public CharacterController controller;  //characterController component
    public Transform cam;                   //third person camera 
    
    public float speed = 6f;                //speed of movement! 

    public float turnSmoothTime = 0.1f;      //smooth the rotation if the character
    float turnSmoothVelocity;                

    Vector3 height;                         //to adjust the height of the character so it can fly  

    private bool goingDown;
    private bool goingUp; 
    
    private void Awake()
    {
       //assigns variables as soon as the program awakes - prevents bugs, using just Start() here could cause issues. 
       playerActionsAsset = new thirdPersonActionAsset();  
       height = new Vector3(0f,0f,0f);

        goingDown = false;
        goingUp = false; 
    }

    private void OnEnable()
    {   
        //access all the separate parts of the input component
        move = playerActionsAsset.Player.Move;
        jump = playerActionsAsset.Player.Jump;
        down = playerActionsAsset.Player.Down;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable() {
        playerActionsAsset.Player.Disable();   //destructor! 
    }

    private void FixedUpdate() { //again better to use this here instead of Update() to prevent bugs - as update can slow down if the frame rate drops meaning the movement would feel more janky
        
        //get input values 
        float horizontal = move.ReadValue<Vector2>().x;
        float vertical = move.ReadValue<Vector2>().y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        //move the PlayerController component based upon input
        
        controller.Move(height * Time.deltaTime); 
        
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //add the toggles for going up and down
        if(jump.triggered){
            if(goingDown){
                goingDown = false; 
            } else if(goingUp){
                goingUp = false; 
            } else {
                goingUp = true;
            }
        } else if(down.triggered){
            if(goingUp){
                goingUp = false; 
            } else if(goingDown){
                goingDown = false; 
            } else {
                goingDown = true; 
            }
        } 

        if(goingUp){
            height = new Vector3(0,6,0);
        } else if(goingDown){
            height = new Vector3(0,-6,0);
        }

        if(!goingUp && !goingDown){
            height = new Vector3(0,0,0);
        }
    }
}
