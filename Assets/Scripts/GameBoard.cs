using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public GameObject Car;
    public Slider UI_Time_Slider;
    static readonly Vector3[] AI_move_vector = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };            // AI_Move[0] - to right        AI_Move[1] - to left

    float GameTime;
    public static float RespawnPoint = 12;

    void Start()
    {
        GameTime = 10;
        UI_Time_Slider.maxValue = GameTime;
        UI_Time_Slider.value = UI_Time_Slider.maxValue;
        StartCoroutine("Timer001s");
    }

    void Update()
    {

    }

    public static void RespawnObject(GameObject arg_obj)
    {
        Vector3 respawn_pos = arg_obj.transform.position;      // cannot be done directly as transform.position.x *= -1;
        respawn_pos.x *= -1;
        arg_obj.transform.position = respawn_pos;
    }

    public static void AI_Move(GameObject TargetObject, float Velocity)         // This function is used to move objects
    {
        int dir;                                                            //
        if (TargetObject.GetComponent<SpriteRenderer>().flipX == true) dir = 1;        // to determine direction of movement 
        else dir = 0;                                                       // int is passed to pick vector from array

        TargetObject.transform.position += (GameBoard.AI_move_vector[dir] * Velocity);
    }
    IEnumerator Timer001s()
    {
        for (; ; )    // endless "for" loop
        {
            yield return new WaitForSeconds(0.01F);
            UI_Time_Slider.value -= 0.01F;
            // Debug.Log("Time left : " + UI_Time_Slider.value);
        }
    }

    public static float RollDeviation()
    {
        float d = Random.Range(-1.5F, 1.5F);
        return d;
    }
}
