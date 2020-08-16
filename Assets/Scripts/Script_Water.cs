using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Water : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    int LogAmountPerLane = 3;
    int LilyAmountPerLane = 7;

    public static float v_log;      // velocity of Log
    public static float v_lily;     // velocity of Lily

    public GameObject LineWaterPrefab;                      
    public GameObject LogPrefab;
    public GameObject LilyPrefab;

    public static GameObject[] LinesWater;     // to store lines in array
    List<GameObject> Logs = new List<GameObject>();
    List<GameObject> Lilys = new List<GameObject>();

    int AmountOfSinkingElements;
    int MaxAmountOfSinkingElements;

    void Start()
    {
        LinesWater = new GameObject[Settings.DefaultLanes[2] + Settings.ExtraLanes[2]];

        InstantiateWater();
        InstantiateLogs();
        InstatiateLilys();

        v_log = 0.015F * (1 + (Settings.Difficulty / 5F));    // 20% faster per lvl       
        v_lily = 0.01F / (1 + (Settings.Difficulty / 5F));   // 20% slower per lvl

        AmountOfSinkingElements = 0; // starting with zero
        MaxAmountOfSinkingElements = 1 + (Settings.Difficulty / 2);
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

        foreach (GameObject Lily in Lilys)
        {
            if (AmountOfSinkingElements < MaxAmountOfSinkingElements)
            {
                AmountOfSinkingElements++;
                StartCoroutine(SinkingSequence(Lily));
            }
            GameBoard.AI_Move(Lily, v_lily);         // Move obj C with velocity of v   
            if (Mathf.Abs(Lily.transform.position.x) >= GameBoard.RespawnPoint)
            {
                GameBoard.RespawnObject(Lily);
            }
        }
    }
    void InstantiateWater()
    {
        Vector3 InstantiationLinePos = new Vector3();                                   // initialize temporary Vector
        for (int i = 0; i < Settings.DefaultLanes[2] + Settings.ExtraLanes[2]; i++)
        {    
            InstantiationLinePos.Set(0, Settings.DefaultLanes[0] + Settings.ExtraLanes[0] + Settings.DefaultLanes[1] + Settings.ExtraLanes[1] + i - 3, 0);     // Start counting from amount of Road Lines + Grass.
            LinesWater[i] = Instantiate(LineWaterPrefab);                               // instantiate prefabs to GameObject array
            LinesWater[i].transform.position = InstantiationLinePos;                    // pass into newly created object 
        }
    }

    void InstantiateLogs()
    {
        Vector3 InstantiationLinePos = new Vector3();
        for (int i = 0; i < Settings.DefaultLanes[2] + Settings.ExtraLanes[2]; i++)
        {
            float x_coord;
            for (int j = 0; j < LogAmountPerLane; j++)
            {
                if (i % 2 == 0)                                     // at each second lane log will be facing the other direction
                {
                    x_coord = -GameBoard.RespawnPoint + (1 + j) * ((GameBoard.RespawnPoint * 2) / (LogAmountPerLane + 1)) + GameBoard.RollDeviation()*1.5F;                
                    InstantiationLinePos.Set(x_coord, Settings.DefaultLanes[0] + Settings.ExtraLanes[0] + Settings.DefaultLanes[1] + Settings.ExtraLanes[1] + i - 3, -1);
                    GameObject NewLog = Instantiate(LogPrefab);
                    NewLog.transform.position = InstantiationLinePos;                
                    Logs.Add(NewLog);
                }
            }
        }
    }

    void InstatiateLilys()
    {
        Vector3 InstantiationLinePos = new Vector3();
        for (int i = 0; i < Settings.DefaultLanes[2] + Settings.ExtraLanes[2]; i++)
        {
            float x_coord;
            for (int j = 0; j < LilyAmountPerLane; j++)
            {
                if(i % 2 != 0)                                     // at each second lane log will be facing the other direction
                {
                    x_coord = -GameBoard.RespawnPoint + (1 + j) * ((GameBoard.RespawnPoint * 2) / (LilyAmountPerLane + 1)) + GameBoard.RollDeviation();
                    InstantiationLinePos.Set(x_coord, Settings.DefaultLanes[0] + Settings.ExtraLanes[0] + Settings.DefaultLanes[1] + Settings.ExtraLanes[1] + i - 3, -1);
                    GameObject NewLily = Instantiate(LilyPrefab);
                    NewLily.transform.position = InstantiationLinePos;
                    NewLily.GetComponent<SpriteRenderer>().flipX = true;
                    Lilys.Add(NewLily);
                }
            }
        }
    }

    IEnumerator SinkingSequence(GameObject floatingobj)
    {
        Color fadestep = new Color(0, 0, 0, .1F);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 2));        // Random delay from selecting to starting sequence
            
            // Warning sequence
            for (int a = 0; a < 3; a++)
            {
                for (int i = 0; i < 7; i++)
                {
                    floatingobj.GetComponent<SpriteRenderer>().color -= fadestep;
                    yield return new WaitForSeconds(.08F);
                }

                for (int i = 0; i < 7; i++)
                {
                    floatingobj.GetComponent<SpriteRenderer>().color += fadestep;
                    yield return new WaitForSeconds(.08F);
                }
            }

            // Sinking sequence
            for(int i = 0; i < 10; i++)
                {
                floatingobj.GetComponent<SpriteRenderer>().color -= fadestep;
                yield return new WaitForSeconds(.08F);
            }
            floatingobj.SetActive(false);

            // Underwater
            yield return new WaitForSeconds(1F);

            // Emerging sequence
            floatingobj.SetActive(true);
            for (int i = 0; i < 10; i++)
                {
                floatingobj.GetComponent<SpriteRenderer>().color += fadestep;
                yield return new WaitForSeconds(.08F);
            }

        }



    }
}
