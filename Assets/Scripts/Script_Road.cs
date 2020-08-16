using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Road : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    
    int CarAmountPerLane;
    static int MaxCarAmountPerLane = 8;

    public GameObject LineRoadPrefab;                                   // these prefabs 
    public GameObject CarPrefab;                                        // will be instantiated to arrays / lists

    public static GameObject[] LinesRoad;   // to store road lines in array

    List<GameObject> Cars = new List<GameObject>();                     // to store Cars in list

    private float v;

    void Start()
    {
        LinesRoad = new GameObject[(Settings.DefaultLanes[0] + Settings.ExtraLanes[0])];
        CarAmountPerLane = Mathf.Min(2 + Settings.Difficulty, MaxCarAmountPerLane);

        v = 0.012F * (1 + (Settings.Difficulty / 5F));           // +20% speed per difficulty (diminishing returns)

        InstantiateRoads();
        InstantiateCars();
    }

    void Update()
    {
        foreach (GameObject Car in Cars)
        {
            GameBoard.AI_Move(Car, v);                                          // Move obj C with velocity of v 
            if (Mathf.Abs(Car.transform.position.x) >= GameBoard.RespawnPoint)  // if goes too far on the board
            {
                GameBoard.RespawnObject(Car);
            }
        }
    }

    void InstantiateRoads()
    {
        Vector3 InstantiationLinePos = new Vector3();                   // initialize temporary Vector
        for (int i = 0; i < (Settings.DefaultLanes[0] + Settings.ExtraLanes[0]); i++)
        {
            InstantiationLinePos.Set(0, i - 3, 0);
            LinesRoad[i] = Instantiate(LineRoadPrefab);                 // instantiate prefabs to GameObject array
            LinesRoad[i].transform.position = InstantiationLinePos;     // pass position Vector into newly created object 
        }
    }

    void InstantiateCars()
    {
        Vector3 InstantiationLinePos = new Vector3();
        for (int i = 0; i < (Settings.DefaultLanes[0] + Settings.ExtraLanes[0]); i++)
        {
            float x_coord;                                          // will be used to roll x position
            for (int j = 0; j < CarAmountPerLane; j++)
            {
                x_coord = -GameBoard.RespawnPoint + (1 + j) * ((GameBoard.RespawnPoint * 2) / (CarAmountPerLane + 1)) + GameBoard.RollDeviation();
                InstantiationLinePos.Set(x_coord, i - 3, -2);
                GameObject NewCar = Instantiate(CarPrefab);
                NewCar.transform.position = InstantiationLinePos;

                if (i % 2 == 0)                                     // at each second lane car will be facing the other direction
                {
                    NewCar.GetComponent<SpriteRenderer>().flipX = true;
                }
                Cars.Add(NewCar);
            }

        }
    }
}
