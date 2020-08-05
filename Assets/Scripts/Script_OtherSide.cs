using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_OtherSide : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public GameObject LineOtherSidePrefab;                                      // these prefabs 

    void Start()
    {
        InstantiateOtherSide();
    }

    void Update()
    {
        
    }
    void InstantiateOtherSide()
    {
        Vector3 InstantiationLinePos = new Vector3();                                   // initialize temporary Vector
        InstantiationLinePos.Set(0, Script_Road.RoadLanes + Script_MidGrass.GrassLanes + Script_Water.WaterLanes - 3, -1);      // Start counting from amount of Road Lines + Grass + Water.
        LineOtherSidePrefab = Instantiate(LineOtherSidePrefab);                               // instantiate prefabs to GameObject array
        LineOtherSidePrefab.transform.position = InstantiationLinePos;                    // pass into newly created object 
    }
}
