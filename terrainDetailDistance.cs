using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
