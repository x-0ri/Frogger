using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public GameObject Car;
    public Slider UI_Time_Slider;
    public GameObject[] UI_Lives = new GameObject[3];
    public GameObject[] UI_Passes = new GameObject[3];
    static readonly Vector3[] AI_move_vector = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };            // AI_Move[0] - to right        AI_Move[1] - to left
    public bool TimerIsActive;
    float GameTime;
    public static float RespawnPoint = 12;
    public Color FullColor = new Color(1, 1, 1, 1);

    void Start()
    {
        GameTime = 20;
        TimerIsActive = true;
        ResetTimer();
        StartCoroutine(GameTimer());
    }
    public static void RespawnObject(GameObject arg_obj)
    {
        Vector3 respawn_pos = arg_obj.transform.position;      // cannot be done directly as transform.position.x *= -1;
        respawn_pos.x *= -1;
        arg_obj.transform.position = respawn_pos;
    }

    public static void AI_Move(GameObject TargetObject, float Velocity)                // This function is used to move objects (any)
    {
        int dir;                                                                       //
        if (TargetObject.GetComponent<SpriteRenderer>().flipX == true) dir = 1;        // to determine direction of movement 
        else dir = 0;                                                                  // int is passed to pick vector from array

        TargetObject.transform.position += (GameBoard.AI_move_vector[dir] * Velocity);
    }

    public static void Carry_Player_On_Water(GameObject arg_Player, float Velocity)         // should be called only for player
    {
        if (arg_Player.transform.position.x < 9 && arg_Player.transform.position.x > -9)    // to prevent carrying player outside the board
        {
            int dir;                                                                        //
            if (arg_Player.transform.position.y % 2 == 0) dir = 0;                          // to determine direction of movement 
            else dir = 1;                                                                   // int is passed to pick vector from array

            arg_Player.transform.position += (GameBoard.AI_move_vector[dir] * Velocity);
            Player.move_to += (GameBoard.AI_move_vector[dir] * Velocity);                   // !!! Player's target position also needs to be modified in same way as current position !!!
        }  
    }
    public IEnumerator GameTimer()
    {
        while (TimerIsActive) 
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

    public void ResetTimer()
    {
        Debug.Log("Restarting Timer");
        UI_Time_Slider.maxValue = GameTime;
        UI_Time_Slider.value = UI_Time_Slider.maxValue;
    }
}
