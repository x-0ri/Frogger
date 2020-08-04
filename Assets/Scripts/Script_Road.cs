using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Script_Road : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public GameObject Line_Prefab;                                      // these prefabs 
    public GameObject Car_Prefab;                                       // will be copied to arrays

    static int Lanes = 4;
    GameObject[] Lines_Road = new GameObject[Lanes];                    // to store road lines in array
    List<GameObject> Cars = new List<GameObject>();                     // to store Cars in list

    readonly Vector3[] AI_move_vector = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };            // AI_Move[0] - to right        AI_Move[1] - to left
    private float v = 0.01F;

    void Start()
    {
        InstantiateRoads();
        InstantiateCars();
    }

    // Update is called once per frame
    void Update()
    {         
        foreach (GameObject C in Cars)
        {
            int dir;
            if (C.GetComponent<SpriteRenderer>().flipX == true) dir = 1;
            else dir = 0;

            C.transform.position += (AI_move_vector[dir] * v);
        }        
    }

    void InstantiateRoads() 
    {
        Vector3 InstantiationLinePos = new Vector3();                                   // initialize temporary Vector
        for (int i = 0; i < Lanes; i++)
        {
            InstantiationLinePos.Set(0, i - 3, -1);                     // set spawn coordinates
            Lines_Road[i] = Instantiate(Line_Prefab);                   // instantiate prefabs to GameObject array
            Lines_Road[i].transform.position = InstantiationLinePos;    // pass into newly created object  
        }    
    }

    void InstantiateCars() 
    {
        Vector3 InstantiationLinePos = new Vector3();
        for (int i = 0; i < Lanes; i++)
        {
            InstantiationLinePos.Set(0, i - 3, -1);
            GameObject NewCar = Instantiate(Car_Prefab);
            NewCar.transform.position = InstantiationLinePos;
            
            if (i%2 == 0)                   // each second lane car will be facing the other direction
            {
                NewCar.GetComponent<SpriteRenderer>().flipX = true;
            }

            Cars.Add(NewCar);
        }
    }
}
