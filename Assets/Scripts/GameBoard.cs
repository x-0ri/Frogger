﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public GameObject Car;
    public GameObject ScoreCollider;
    public SoundHandlerGame SoundHandler;

    public GameObject[] UI_Lives = new GameObject[3];
    public GameObject[] UI_Passes = new GameObject[3];

    public Slider UI_Time_Slider;
    public TextMeshProUGUI UI_Text_ScoreCount;
    public Color FullColor = new Color(1, 1, 1, 1);

    static readonly Vector3[] AI_move_vector = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0) };            // AI_Move[0] - to right        AI_Move[1] - to left
    public bool TimerIsActive;
    float GameTime;
    public static float RespawnPoint = 12;

    void Start()
    {
        Debug.Log("Game started : Difficulty = " + Settings.Difficulty);
        GameTime = 20 - (Settings.Difficulty - 1); // -1 second for each difficulty level
        TimerIsActive = true;
        ResetTimer();
        UI_Text_ScoreCount.text = Settings.ScoreCount.ToString();
        StartCoroutine(GameTimer());
        SoundHandler.PlayMusic();
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
        if (arg_Player.transform.position.x < 9.5 && arg_Player.transform.position.x > -9.5)    // to prevent carrying player outside the board
        {
            int dir;                                                                                                //
            if ((arg_Player.transform.position.y + Settings.DefaultLanes[0] + Settings.ExtraLanes[0] + Settings.DefaultLanes[1] + Settings.ExtraLanes[1] - 1) % 2 == 0) dir = 0;               // to determine direction of movement 
            else dir = 1;                                                                                           // int is passed to pick vector from array

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
        float d = Random.Range(-1F, 1F);
        return d;
    }

    public void ResetTimer()
    {
        //Debug.Log("Restarting Timer");
        UI_Time_Slider.maxValue = GameTime;
        UI_Time_Slider.value = UI_Time_Slider.maxValue;
    }

    public void AddScore(bool PlayerHasPassed)
    {
        if (PlayerHasPassed) 
        { 
            Settings.ScoreCount += 100 + (Mathf.CeilToInt(UI_Time_Slider.value) * 10);
            UI_Text_ScoreCount.text = Settings.ScoreCount.ToString(); 
        }
        else
        {
            Settings.ScoreCount += 5;
            UI_Text_ScoreCount.text = Settings.ScoreCount.ToString();
        }
    }

    public void ResetScoreColliderPosition()
    {
        Vector3 movescorecollider = ScoreCollider.transform.position;
        movescorecollider.y = -3;
        ScoreCollider.transform.position = movescorecollider;
    }
}
