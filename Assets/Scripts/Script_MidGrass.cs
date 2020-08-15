using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MidGrass : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public static int GrassLanes = 1;
    int FenceAmountPerLane;
    static int MaxFenceAmountPerLane = 19;
    public GameObject LineGrassPrefab;                                          // prefab to clone from
    public GameObject SnakePrefab;
    public GameObject FencePrefab;

    List<GameObject> Snakes = new List<GameObject>();
    List<GameObject> Fences = new List<GameObject>();

    public static GameObject[] LinesGrass = new GameObject[GrassLanes];         // to store lines in array

    bool[,] FenceGrid = new bool[GrassLanes, MaxFenceAmountPerLane];                    // to prevent fence overlapping with usage of while loop

    private float v_snake;
    private float FenceYPositionOffset = 0.15F;
    private bool snakeflip = false;

    void Start()
    {
        v_snake = 0.005F * (1 + (Settings.Difficulty / 2F));             // difficulty scaling - +50% speed per difficulty (diminishing returns)
        FenceAmountPerLane = Mathf.Min(6 + (Settings.Difficulty) * 2, 16);      // difficulty scaling - rising with difficulty
        SetFenceGridState();

        InstantiateMidGrass();
        InstantiateFences();
        InstantiateSnake();

        StartCoroutine(SnakeFlip());        // just for fancy snake anim
    }

    void Update()
    {
        foreach (GameObject Snake in Snakes)
        {
            GameBoard.AI_Move(Snake, v_snake);         // Move obj C with velocity of v  
            Snake.GetComponent<SpriteRenderer>().flipY = snakeflip;
            if (Mathf.Abs(Snake.transform.position.x) >= GameBoard.RespawnPoint)
            {
                GameBoard.RespawnObject(Snake);
            }
        }
    }
    void SetFenceGridState() 
    {
        for (int i = 0; i < GrassLanes; i++)
        {
            for (int j = 0; j < MaxFenceAmountPerLane; j++)
            {
                FenceGrid[i,j] = false;
            }
        }
    }

    void InstantiateMidGrass()
    {
        Vector3 InstantiationLinePos = new Vector3();                                   // initialize temporary Vector
        for (int i = 0; i < GrassLanes; i++)
        {
            InstantiationLinePos.Set(0, Script_Road.RoadLanes + i - 3, 0);              // Start counting from amount of Road Lines.
            LinesGrass[i] = Instantiate(LineGrassPrefab);                               // instantiate prefabs to GameObject array
            LinesGrass[i].transform.position = InstantiationLinePos;                    // pass into newly created object 
        }
    }

    void InstantiateSnake()
    {
        Vector3 InstantiationLinePos = new Vector3();
        InstantiationLinePos.Set(0, Script_Road.RoadLanes - 3, -1);
        GameObject NewSnake = Instantiate(SnakePrefab);
        NewSnake.transform.position = InstantiationLinePos;
        Snakes.Add(NewSnake);
    }

    void InstantiateFences()
    {
        for (int i = 0; i < GrassLanes; i++)
        {
            Vector3 InstantiationPos = new Vector3();                                          
            int RolledPosition;

            for (int j = 0; j < FenceAmountPerLane; j++)
            {
                RolledPosition = Mathf.CeilToInt(Random.Range(0, MaxFenceAmountPerLane - 1));
                while (FenceGrid[i,RolledPosition] == true)
                {
                    RolledPosition = Mathf.CeilToInt(Random.Range(0, MaxFenceAmountPerLane - 1));
                    if (FenceGrid[i,RolledPosition] == false)
                    {
                        FenceGrid[i,RolledPosition] = true;
                        break;
                    }
                }

                GameObject NewFence = Instantiate(FencePrefab);
                InstantiationPos.Set(RolledPosition - 9, Script_Road.RoadLanes + i - 3 - FenceYPositionOffset, -4);
                NewFence.transform.position = InstantiationPos;
                Fences.Add(NewFence);
                FenceGrid[i,RolledPosition] = true;
            }
        }
    }

    IEnumerator SnakeFlip()
    {
        while (true)
        {            
            snakeflip = !snakeflip;
            yield return new WaitForSeconds(0.4F / (1 + (Settings.Difficulty / 20F))); // match speed with flip frequency
        }
    }
}
