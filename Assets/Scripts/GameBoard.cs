using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public GameObject Car;
    public Slider UI_Time_Slider;
    readonly float SpawnDistance = 15;

    readonly Vector3[] AI_move_vector = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };            // AI_Move[0] - to right        AI_Move[1] - to left
    private float v = 0.01F;    

    float GameTime;
    
    void Start()
    {
        GameTime = 100;
        UI_Time_Slider.maxValue = GameTime;
        UI_Time_Slider.value = UI_Time_Slider.maxValue;
        StartCoroutine("Timer01s");
        Car = SpawnCar(1, 1);      // spawn Car at left side on lane 1        
    }

    void Update()
    {
        AI_Move(Car, v, 1);    // AI_Move(Selected object  ,   velocity of movement    ,   side 0 - to right   1 - to left)

        if (Car.transform.position.x < - SpawnDistance)
        {
            RespawnObject(Car);
        }
    }

    void AI_Move(GameObject arg_obj, float arg_v, int arg_dir)
    {
        if(arg_obj != null) arg_obj.transform.position += (AI_move_vector[arg_dir] * arg_v);       // attempt to move only when object is not empty
    }

    public GameObject SpawnCar(int arg_side, int arg_lane)              // arg_side and arg_lane are passed to GetSpawnPosition func
    {
        GameObject funcCar = Instantiate(Car);                          // instantiate object to be returned
        if (arg_side == 1)
        {
            funcCar.GetComponent<SpriteRenderer>().flipX = true;
        }
        funcCar.transform.position = GetSpawnPosition(arg_side, arg_lane);
        return funcCar;
    }

    public void RespawnObject(GameObject arg_obj)
    {
        Vector3 respawn_pos = arg_obj.transform.position;      // cannot be done directly in transform.position.x
        respawn_pos.x *= -1;
        arg_obj.transform.position = respawn_pos;
    }
    
    Vector3 GetSpawnPosition(int arg_side, int arg_lane)             // arg_side = -1 ----> left side ,   arg_line = 1 ----> first lane
    {
        Vector3 SpawnPoint = new Vector3(SpawnDistance * arg_side, arg_lane - 4, -1);   // -4 and -1 are position constants !!
        return SpawnPoint;
    }

    IEnumerator Timer01s()
    {
        for (;;) 
        { 
        yield return new WaitForSeconds(0.1F);
        UI_Time_Slider.value--;
        //Debug.Log("Time left : " + UI_Time_Slider.value);
        }
    }

}
