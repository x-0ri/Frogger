using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameBoard GameBoardScript;
    public GameObject player;
    public int lives;
    public int passes;
    private float velocity = 0.1F;

    bool InFrontOfFence, AtBackOfFence, OnWater, OnWaterObjectLily, OnWaterObjectLog, HasPassed;

    private int direction;               // Direction selector  // 0 - none     1 - up      2 - down    3 - left    4 - right
    readonly Vector3[] move = { new Vector3(0, 0, 0),           // 0 - none
                                new Vector3(0, 1, 0),           // 1 - up
                                new Vector3(0, -1, 0),          // 2 - down
                                new Vector3(-1, 0, 0),          // 3 - left
                                new Vector3(1, 0, 0)};          // 4 - right

    
    public static Vector3 move_to;                    // For keeping target position during movement of player    
    Vector3 playerspawn = new Vector3(0, -4, -3);
    Vector3 playerduringrespawn = new Vector3(-10, -10, -3);

    void Start()
    {
        Reset_Player();
        direction = 0;
        move_to = new Vector3();
        lives = 3;
        passes = 0;
    }

    void Update()
    {        
        if (direction == 0)        // if player is not moving
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

            if (HasPassed)
            {
                StartCoroutine(Event_Passed_To_Other_Side());
                return;
            }

            if (OnWater)
            {
                if (OnWaterObjectLog || OnWaterObjectLily)
                {
                    GameBoard.Carry_Player_On_Water(player, Script_Water.v_log);
                }
                
                else
                {
                    StartCoroutine(Event_Death());
                }
            }

        }
        else         
        {
            DoMovement();
        }

        if (GameBoardScript.UI_Time_Slider.value == 0)
        {
            StartCoroutine(Event_Death());
        }
    }

    void DoMovement()
    {
        player.transform.position += (move[direction] * velocity);      // move player in selected direction
        if (player.transform.position == move_to)                       // if player reached position to move to
        {
            player.transform.position = move_to;                        // !!! comparing two Vector3 does have some approximation / rounding and results in leftover values like 0,0001 etc. This line only sets position to whole value.
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
            StartCoroutine(Event_Death());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)      // function for handling trigger entering (staying on something goes here)
    {
        if (direction == 0)
        {
            if (collision.CompareTag("FenceFront"))
            {
                InFrontOfFence = true;
                //Debug.Log("Player is in front of the fence");
            }

            if (collision.CompareTag("FenceBack"))
            {
                AtBackOfFence = true;
                //Debug.Log("Player is at back of the fence");
            }

            if (collision.CompareTag("WaterCollider"))
            {
                OnWater = true;
                //Debug.Log("Player is on water");
            }

            if (collision.CompareTag("FloatingObjectLog"))
            {
                OnWaterObjectLog = true;
                //Debug.Log("Player is on log");
            }

            if (collision.CompareTag("FloatingObjectLily"))
            {
                OnWaterObjectLily = true;
                //Debug.Log("Player is on lily");
            }

            if (collision.CompareTag("FinishCollider"))
            {
                Debug.Log("Passed");
                HasPassed = true;
                StartCoroutine(Event_Passed_To_Other_Side());
            }

            if (collision.CompareTag("ScoreCollider"))
            {
                Vector3 movescorecollider = GameBoardScript.ScoreCollider.transform.position;
                movescorecollider.y++;
                GameBoardScript.ScoreCollider.transform.position = movescorecollider;

                Debug.Log("Score + 5");
                GameBoardScript.AddScore(false);                // false - add score normally
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      // function for handling trigger exiting (extiting something goes here)
    {
        if (collision.CompareTag("FenceFront"))
        {
            InFrontOfFence = false;
            //Debug.Log("Player is not in front of the fence");
        }

        if (collision.CompareTag("FenceBack"))
        {
            AtBackOfFence = false;
            //Debug.Log("Player is not at back of the fence");
        }
        if (collision.CompareTag("WaterCollider"))
        {
            OnWater = false;
            //Debug.Log("Player is not on water");
        }

        if (collision.CompareTag("FloatingObjectLog"))
        {
            OnWaterObjectLog = false;
            //Debug.Log("Player is not on log");
        }

        if(collision.CompareTag("FloatingObjectLily"))
        {
            OnWaterObjectLily = false;
            //Debug.Log("Player is not on lily");
        }
    }

    void Reset_Player()
    {
        InFrontOfFence = false;
        AtBackOfFence = false;
        OnWater = false;
        OnWaterObjectLily = false;
        OnWaterObjectLog = false;
        HasPassed = false;
        direction = 0;
        GameBoardScript.ResetScoreColliderPosition();
    }

    IEnumerator Event_Death()
    {
        // After being hit
        lives--;
        Reset_Player();
        player.transform.position = playerduringrespawn;    // !!! Note : this has to be done by moving player out of gameboard so he cannot move. 
                                                            // Deactivating player by SetActive(false) causes code to not get back to this coroutine, 
                                                            // since it deactivates object that started this coroutine (? possible cause)
        GameBoardScript.ResetTimer();
        GameBoardScript.TimerIsActive = false;

        if (lives > 0)                                      // if player has lives left
        {
            GameBoardScript.UI_Lives[lives].SetActive(false);               // uses int variable "lives" to disable lives 3 and 2 (2 and 1 in array)
            
            // Respawn delay
            yield return new WaitForSeconds(3F);

            // Reset player position and start new timer        
            player.transform.position = playerspawn;
            GameBoardScript.TimerIsActive = true;
            StartCoroutine(GameBoardScript.GameTimer());        // coroutine has to be started over since it exited "while" loop and ended
        }
        else
        {
            GameBoardScript.UI_Lives[lives].SetActive(false);               // uses int variable "lives" to disable life 1 (0 in array)
            player.SetActive(false);
        }        
    }

    IEnumerator Event_Passed_To_Other_Side()
    {
        if (passes < 3)
        {
            GameBoardScript.UI_Passes[passes].GetComponent<SpriteRenderer>().color = GameBoardScript.FullColor;
            passes++;   // increase after UI update so it can pass value to array ( 2 - 0 )

            Reset_Player();
            player.transform.position = playerduringrespawn;    // !!! Note : this has to be done by moving player out of gameboard so he cannot move. 
                                                                // Deactivating player by SetActive(false) causes code to not get back to this coroutine, 
                                                                // since it deactivates object that started this coroutine (? possible cause)

            GameBoardScript.AddScore(true);                     // true - add score for finishing  
            GameBoardScript.ResetTimer();
            GameBoardScript.TimerIsActive = false;
            
            // Respawn delay
            yield return new WaitForSeconds(3F);

            // Reset player position and start new timer        
            GameBoardScript.TimerIsActive = true;
            StartCoroutine(GameBoardScript.GameTimer());        // coroutine has to be started over since it exited "while" loop and ended
            player.transform.position = playerspawn;
        } 
    }
}
