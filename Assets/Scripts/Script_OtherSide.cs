using UnityEngine;

public class Script_OtherSide : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public GameObject LineOtherSidePrefab;                                      
    void Start()
    {
        InstantiateOtherSide();
    }

    void InstantiateOtherSide()
    {
        Vector3 InstantiationLinePos = new Vector3();                                   // initialize temporary Vector
        InstantiationLinePos.Set(0, Script_Road.RoadLanes + Script_MidGrass.GrassLanes + Script_Water.WaterLanes - 3, 0);      // Start counting from amount of Road Lines + Grass + Water.
        LineOtherSidePrefab = Instantiate(LineOtherSidePrefab);                               // instantiate prefabs to GameObject array
        LineOtherSidePrefab.transform.position = InstantiationLinePos;                    // pass into newly created object 
    }
}
