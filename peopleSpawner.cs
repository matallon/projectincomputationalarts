using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
