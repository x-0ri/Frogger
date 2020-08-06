using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameBoard GameBoardScript;
    public GameObject player;
    public GameObject[] UI_Lives = new GameObject[3];
    public int lives;
    private float velocity = 0.1F;

    bool InFrontOfFence, AtBackOfFence, OnWater, OnWaterObject;

    private int direction;               // Direction selector  // 0 - none     1 - up      2 - down    3 - left    4 - right
    readonly Vector3[] move = { new Vector3(0, 0, 0),           // 0 - none
                                new Vector3(0, 1, 0),           // 1 - up
                                new Vector3(0, -1, 0),          // 2 - down
                                new Vector3(-1, 0, 0),          // 3 - left
                                new Vector3(1, 0, 0)};          // 4 - right

    
    public static Vector3 move_to;                    // For keeping target position during movement of player    
    Vector3 playerspawn = new Vector3(0, -4, -3);

    void Start()
    {
        Reset_Player_State();
        direction = 0;
        move_to = new Vector3();
        lives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (direction == 0)        // if player is not moving - read input keys for player
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && !InFrontOfFence)       // ... && not in front of fence
            {
                direction = 1;
                Set_Move_To();
                //Debug.Log("Player moving to : (" + move_to.x + "," + move_to.y + ")");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && !AtBackOfFence && player.transform.position.y > -4 )  // ... && not at back of fence
            {
                direction = 2;
                Set_Move_To();
                //Debug.Log("Player moving to : (" + move_to.x + "," + move_to.y + ")");
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && player.transform.position.x > -9)
            {
                direction = 3;
                Set_Move_To();
                //Debug.Log("Player moving to : (" + move_to.x + "," + move_to.y + ")");                
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && player.transform.position.x < 9)
            {
                direction = 4;
                Set_Move_To();
                //Debug.Log("Player moving to : (" + move_to.x + "," + move_to.y + ")");                
            }

            if (OnWater)
            {
                if (OnWaterObject)
                {
                    GameBoard.Carry_Player_On_Water(player, Script_Water.v_log);
                }

                else Respawn();
            }
        }
        else         
        {
            DoMovement();
        }

        if (GameBoardScript.UI_Time_Slider.value == 0)
        {
            Respawn();
            GameBoardScript.ResetTimer();
        }


    }

    void DoMovement()
    {
        player.transform.position += (move[direction] * velocity);                      // move player in selected direction
        if (player.transform.position == move_to)                                       // if player reached position to move to
        {
            player.transform.position = move_to;        // !!! comparing two Vector3 does have some approximation / rounding and results in leftover values like 0,0001 etc. This line only sets position to whole value.
            direction = 0;
        }
    }

    void Set_Move_To()
    {
        move_to = player.transform.position + move[direction];        
    }

    private void OnTriggerEnter2D(Collider2D collision)     // function for handling trigger entering (getting hit goes here)
    {
        if (collision.CompareTag("KillCollider"))           // check if player collided with object that has "KillCollider" tag
        {
            Debug.Log("Hit");
            Respawn();            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)      // function for handling trigger entering (staying on something goes here)
    {
        if (collision.CompareTag("FenceFront"))
        {
            InFrontOfFence = true;
            Debug.Log("Player is in front of the fence");
        }

        if (collision.CompareTag("FenceBack"))
        {
            AtBackOfFence = true;
            Debug.Log("Player is at back of the fence");
        }

        if(collision.CompareTag("WaterCollider"))
        {
            OnWater = true;
            Debug.Log("Player is on water");
        }

        if (collision.CompareTag("FloatingObject"))
        {
            OnWaterObject = true;
            Debug.Log("Player is on floating object");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      // function for handling trigger exiting (extiting something goes here)
    {
        if (collision.CompareTag("FenceFront"))
        {
            InFrontOfFence = false;
            Debug.Log("Player is not in front of the fence");
        }

        if (collision.CompareTag("FenceBack"))
        {
            AtBackOfFence = false;
            Debug.Log("Player is not at back of the fence");
        }
        if (collision.CompareTag("WaterCollider"))
        {
            OnWater = false;
            Debug.Log("Player is not on water");
        }

        if (collision.CompareTag("FloatingObject"))
        {
            OnWaterObject = false;
            Debug.Log("Player is not on floating object");
        }
    }

    public void Respawn()
    {
        lives--;
        Reset_Player_State();   // make sure any existing state bools are not true
        
        if (lives > 0)          // if player has lives left
        {
            UI_Lives[lives].SetActive(false);              // uses int variable "lives" to disable lives 3 and 2 (2 and 1 in array)
            direction = 0;
            player.transform.position = playerspawn;
            GameBoardScript.ResetTimer();
        }
        else 
        {
            UI_Lives[lives].SetActive(false);              // uses int variable "lives" to disable life 1 (0 in array)
            player.SetActive(false);
            StopCoroutine("Timer01s");
            // Debug.Log("YOU DIED");
        }   
    }

    void Reset_Player_State()
    {
        InFrontOfFence = false;
        AtBackOfFence = false;
        OnWater = false;
        OnWaterObject = false;
    }
}
