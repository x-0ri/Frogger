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
            Settings.Difficulty++;                            // increase difficulty level
            Settings.ExtraLanes[Settings.Difficulty%3]++;     // add extra lane on next level load
            
            SceneManager.LoadScene("Game");                   // reload scene with higher difficulty            
        }

        else
        {
            LeaderBoard.AddToLeaderBoard(Settings.ScoreCount);  // Saving is included in this function
            Settings.Difficulty = 1;            // reset difficulty level
            Settings.ScoreCount = 0;

            for (int i = 0; i < 3; i++)         // reset extra lanes
            {
                Settings.ExtraLanes[i] = 0;
            }
            SceneManager.LoadScene("MainMenu"); // back to menu           
            // some score table here
        }
    }
}
