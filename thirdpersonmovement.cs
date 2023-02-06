using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersonmovement : MonoBehaviour
{
    public CharacterController controller;  //characterController component
    public Transform cam;                   //third person camera 
    
    public float speed = 6f;                //speed of movement! 

    public float turnSmoothTime = 0.1f;      //smooth the rotation if the character
    float turnSmoothVelocity;                

    Vector3 height;                         //to adjust the height of the character so it can fly  
    bool moving = false; 

    bool upStop = false;
    bool downStop = false;

    void Start(){
        //locks cursor to the game sceen
        Cursor.lockState = CursorLockMode.Locked;
        height = new Vector3(0f,0f,0f);
    }

    void Update()
    {
          float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //up and space bar 
        if(Input.GetButtonDown("Jump")){
           height.y += 5f; 
        } else if(Input.GetButtonDown("down")){
           height.y -= 5f;  
        } 

        print(height);
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
}
 