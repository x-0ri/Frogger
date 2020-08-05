using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MidGrass : MonoBehaviour
{
    // Line 1 ----> -3 y coord
    public static int GrassLanes = 1;
    public int FenceAmountPerLane = 4;

    public GameObject LineGrassPrefab;                                                     // prefab to clone from
    public GameObject SnakePrefab;
    public GameObject FencePrefab;

    public static GameObject[] LinesGrass = new GameObject[GrassLanes];                    // to store lines in array
    List<GameObject> Snakes = new List<GameObject>();
    List<GameObject> Fences = new List<GameObject>();

    private float v = 0.003F;
    private bool snakeflip = false;
    void Start()
    {
        InstantiateMidGrass();
        InstantiateFences();
        InstantiateSnake();
        StartCoroutine("SnakeFlip");        // just for fancy snake anim
    }

    void Update()
    {
        foreach (GameObject Snake in Snakes)
        {
            GameBoard.AI_Move(Snake, v);         // Move obj C with velocity of v  
            Snake.GetComponent<SpriteRenderer>().flipY = snakeflip;
            if (Mathf.Abs(Snake.transform.position.x) >= GameBoard.RespawnPoint)
            {
                GameBoard.RespawnObject(Snake);
            }
        }
    }

    void InstantiateMidGrass()
    {
        Vector3 InstantiationLinePos = new Vector3();                                   // initialize temporary Vector
        for (int i = 0; i < GrassLanes; i++)
        {
            InstantiationLinePos.Set(0, Script_Road.RoadLanes + i - 3, -1);             // Start counting from amount of Road Lines.
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
        Vector3 InstantiationPos;                                           // no need for "new Vector3()" keyword because Vector3.Set is not used
        for (int i = 0; i < FenceAmountPerLane; i++)
        {
            GameObject NewFence = Instantiate(FencePrefab);

            InstantiationPos = NewFence.transform.position;
            InstantiationPos.x = Mathf.CeilToInt(Random.Range(-9, 9));      // CeilToInt is used to create "Grid" for fence placement

            NewFence.transform.position = InstantiationPos;
            Fences.Add(NewFence);
        }
    }

    IEnumerator SnakeFlip()
    {
        for (; ; )    // endless "for" loop
        {
            yield return new WaitForSeconds(0.5F);
            snakeflip = !snakeflip;
        }
    }
}
