using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//adds more movement to objects in the scene if the player collides with them. Most notibly is on a football object. 

public class football : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody physic;

    private void Start()
    {
        physic = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.CompareTag("cloud"))
        {
            Vector3 direction = transform.position - collision.transform.position;
            physic.AddForce(direction.normalized * speed, ForceMode.Impulse);
        }
    }
}
