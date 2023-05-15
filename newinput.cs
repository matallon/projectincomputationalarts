using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class newInput : MonoBehaviour
{
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
       playerActionsAsset = new thirdPersonActionAsset(); 
       height = new Vector3(0f,0f,0f);

        goingDown = false;
        goingUp = false; 
    }

    private void OnEnable()
    {
        move = playerActionsAsset.Player.Move;
        jump = playerActionsAsset.Player.Jump;
        down = playerActionsAsset.Player.Down;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable() {
        playerActionsAsset.Player.Disable();
    }

    private void FixedUpdate() {
    
        float horizontal = move.ReadValue<Vector2>().x;
        float vertical = move.ReadValue<Vector2>().y;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

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
