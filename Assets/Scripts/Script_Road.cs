using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Road : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public GameObject Line_Prefab;                                      // these prefabs 
    public GameObject Car_Prefab;                                       // will be copied to arrays

    GameObject[] Lines_Road = new GameObject[4];                        // to store road lines in array
    List<GameObject> Cars = new List<GameObject>();                     // to store Cars in list
    void Start()
    {
        InstantiateRoads();
        //InstantiateCars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateRoads() 
    {
        Vector3 InstantiationLinePos;                                   // initialize temporary Vector
        for (int i = 0; i < Lines_Road.Length; i++)
        {
            InstantiationLinePos = new Vector3(0, i - 3, -1);           // set spawn coordinates
            Lines_Road[i] = Instantiate(Line_Prefab);                   // instantiate prefabs to GameObject array
            Lines_Road[i].transform.position = InstantiationLinePos;    // pass into newly created object  
        }    
    }

    void InstantiateCars() 
    {

    
    }
}
