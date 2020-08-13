using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MidGrass : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public static int GrassLanes = 1;
    int FenceAmountPerLane;
    
    public GameObject LineGrassPrefab;                                          // prefab to clone from
    public GameObject SnakePrefab;
    public GameObject FencePrefab;

    List<GameObject> Snakes = new List<GameObject>();
    List<GameObject> Fences = new List<GameObject>();

    public static GameObject[] LinesGrass = new GameObject[GrassLanes];         // to store lines in array

    bool[] FenceGrid = new bool[19];                    // to prevent fence overlapping with usage of while loop

    private float v_snake;
    private bool snakeflip = false;

    void Start()
    {
        v_snake = 0.005F * (1 + (Settings.Difficulty / 10) * 0.5F);             // difficulty scaling - +50% speed per difficulty (diminishing returns)
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
        for (int i = 0; i < 19; i++)
        {
            FenceGrid[i] = false;
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
            Vector3 InstantiationPos;                                           // no need for "new Vector3()" keyword because Vector3.Set is not used
            int RolledPosition;

            for (int j = 0; j < FenceAmountPerLane; j++)
            {
                RolledPosition = Mathf.CeilToInt(Random.Range(0, 18));
                while (FenceGrid[RolledPosition] == true)
                {
                    RolledPosition = Mathf.CeilToInt(Random.Range(0, 18));
                    if (FenceGrid[RolledPosition] == false)
                    {
                        FenceGrid[RolledPosition] = true;
                        break;
                    }
                }

                GameObject NewFence = Instantiate(FencePrefab);
                InstantiationPos = NewFence.transform.position;
                InstantiationPos.x = RolledPosition - 9;                          // -9 is position offset constant 

                NewFence.transform.position = InstantiationPos;
                Fences.Add(NewFence);
                FenceGrid[RolledPosition] = true;
            }
        }
    }

    IEnumerator SnakeFlip()
    {
        while (true)
        {            
            snakeflip = !snakeflip;
            yield return new WaitForSeconds(0.4F / (1 + (Settings.Difficulty / 10) * 0.5F)); // match speed with flip frequency
        }
    }
}
