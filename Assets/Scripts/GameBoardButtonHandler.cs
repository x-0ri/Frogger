using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameBoardButtonHandler : MonoBehaviour
{
    public Player PlayerScript;
    public void BackToMainMenu()
    {
        if (PlayerScript.Winner)
        {
            Settings.Difficulty++;              // increase difficulty level
            SceneManager.LoadScene("Game");     // reload scene with higher difficulty            
        }

        else
        {
            LeaderBoard.AddToLeaderBoard(Settings.ScoreCount);  // Saving is included in this function
            Settings.Difficulty = 1;            // reset difficulty level
            Settings.ScoreCount = 0;
            SceneManager.LoadScene("MainMenu"); // back to menu           
            // some score table here
        }
    }
}
