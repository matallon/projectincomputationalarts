using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//instantiates people onto the navmesh by pulling from the public array, which is filled in the inspector. 
//fills with 4 different people prefabs. 
public class peopleSpawner : MonoBehaviour
{
    public GameObject[] people;

    void Start()
    {
        for (int i = 0; i < people.Length; i++)
        {
            Instantiate(people[i], transform.position, Quaternion.identity);
        }
    }
}
