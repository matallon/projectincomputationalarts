using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//means the terrain details, such as grass, crops and flowers can be seen from a greater distance
//than the unity UI typically allows. 

public class terrainDetailDistance : MonoBehaviour
{
    public Terrain terrain1; 
    public Terrain terrain2;

    // Update is called once per frame
    void Update()
    {
        terrain1.detailObjectDistance = 700;
        terrain2.detailObjectDistance = 700;
    }
}
