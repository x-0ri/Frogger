using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Water : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public static int WaterLanes = 3;
    int LogAmountPerLane = 3;
    int LilyAmountPerLane = 2;

    public static float v_log = 0.01F;        // velocity of Log
    public static float v_lily = 0.01F;       // velocity of Lily

    public GameObject LineWaterPrefab;                                      // these prefabs 
    public GameObject LogPrefab;
    public GameObject LilyPrefab;

    public static GameObject[] LinesWater = new GameObject[WaterLanes];     // to store lines in array
    List<GameObject> Logs = new List<GameObject>();
    List<GameObject> Lilys = new List<GameObject>();

    void Start()
    {
        InstantiateWater();
        InstantiateLogs();
        InstatiateLilys();
    }

    void Update()
    {
        foreach (GameObject Log in Logs)
        {
            GameBoard.AI_Move(Log, v_log);         // Move obj C with velocity of v   
            if (Mathf.Abs(Log.transform.position.x) >= GameBoard.RespawnPoint)
            {
                GameBoard.RespawnObject(Log);
            }
        }
    }
    void InstantiateWater()
    {
        Vector3 InstantiationLinePos = new Vector3();                                   // initialize temporary Vector
        for (int i = 0; i < WaterLanes; i++)
        {    
            InstantiationLinePos.Set(0, Script_Road.RoadLanes + Script_MidGrass.GrassLanes + i - 3, -1);     // Start counting from amount of Road Lines + Grass.
            LinesWater[i] = Instantiate(LineWaterPrefab);                               // instantiate prefabs to GameObject array
            LinesWater[i].transform.position = InstantiationLinePos;                    // pass into newly created object 
        }
    }

    void InstantiateLogs()
    {
        Vector3 InstantiationLinePos = new Vector3();
        for (int i = 0; i < WaterLanes; i++)
        {
            float x_coord;
            for (int j = 0; j < LogAmountPerLane; j++)
            {
                x_coord = -GameBoard.RespawnPoint + (1 + j) * ((GameBoard.RespawnPoint * 2) / (LogAmountPerLane + 1)) + GameBoard.RollDeviation();                
                InstantiationLinePos.Set(x_coord, Script_Road.LinesRoad.Length + Script_MidGrass.LinesGrass.Length + i - 3, -1);
                GameObject NewLog = Instantiate(LogPrefab);
                NewLog.transform.position = InstantiationLinePos;
                if (i % 2 == 0)                                     // at each second lane log will be facing the other direction
                {
                    NewLog.GetComponent<SpriteRenderer>().flipX = true;
                }
                Logs.Add(NewLog);
            }
        }
    }

    void InstatiateLilys()
    { 
    
    }
}
