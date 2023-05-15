using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class rainFollow : MonoBehaviour
{   
    private thirdPersonActionAsset playerActionsAsset;
    private InputAction rainInput;
    ParticleSystem rain; 
    public GameObject cloudToFollow; 
    Vector3 cloudRotation;

    private void Awake()
    {
        playerActionsAsset = new thirdPersonActionAsset(); 
    }

    private void OnEnable()
    {
        rainInput = playerActionsAsset.Player.Rain;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable() {
        playerActionsAsset.Player.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rain = GetComponent<ParticleSystem>(); 
        cloudRotation = cloudToFollow.transform.eulerAngles; 
    }

    // Update is called once per frame
    void Update()
    {   
        rain.transform.position = new Vector3(cloudToFollow.transform.position.x, cloudToFollow.transform.position.y -5, cloudToFollow.transform.position.z);
        rain.transform.rotation = cloudToFollow.transform.rotation;

        if (rainInput.triggered)
        {
            ToggleParticles();
        }
    }

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
