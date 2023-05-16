using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//instantiates 10 rabbits onto the navmesh, rabbits are assigned in the inspector

public class rabbitSpawnerScript : MonoBehaviour
{
    public GameObject rabbit; 

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; i++){
            Instantiate(rabbit, transform.position, transform.rotation);
        }
    }
}
