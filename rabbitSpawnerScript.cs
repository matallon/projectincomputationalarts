using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
