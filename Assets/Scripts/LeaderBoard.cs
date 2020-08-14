using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


public static class LeaderBoard
{
    public static readonly int leaderboardsize = 5;

    [SerializeField] public static int[] leaderboard = new int[leaderboardsize];             // 0 - highest score
    [SerializeField] private static string filename = "ldb.sav";

    private static BinaryFormatter Binary_Formatter = new BinaryFormatter();

    public static void AddToLeaderBoard(int AddedScore)
    {
        for (int i = 0; i < leaderboardsize; i++)
        {
            if (AddedScore >= leaderboard[i])
            {
                MakePlace(i);
                leaderboard[i] = AddedScore;
                Debug.Log("Wrote " + AddedScore + " to position : " + i);
                break;  // stop completely to not overwrite everything
            }
        }
        Save();
    }
    public static void MakePlace(int index) // moves down leaderboard, abandoning lowest score
    {
        bool EncounteredZero = false;               // for breaking loop to not overwrite whole leaderboard
        for (int i = index; i < leaderboardsize - 1; i++)
        {
            if (leaderboard[i + 1] == 0) EncounteredZero = true;    // if lower score is zero, break loop after rewriting score lower
            leaderboard[i + 1] = leaderboard[i];
            if (EncounteredZero) break;
        }
    }

    public static void Save()
    {
        Debug.Log("Saving...");

        var F = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);      // open filestream F
        Binary_Formatter.Serialize(F, leaderboard);                                     // use Binary Formatter to serialize variable "leaderboard" to file stream F 
        F.Close();                                                                      // close filestream F

        Debug.Log("Filestream closed : " + filename);
    }

    public static void Load()
    {
        var F = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read);
        Debug.Log("Filestream created : " + filename);

        if (F.Length > 0)   // if file has any content (was written)
        {
            Debug.Log("File found : " + filename);

            leaderboard = (int[])Binary_Formatter.Deserialize(F);                       // deserialize from filestream F to "leaderboard variable" as int[] type
            F.Close();

            Debug.Log("Filestream closed : " + filename);

        }
        else                // file length = 0 (was just created by this function's filestream)
        {
            Debug.Log("File not found, created new");
            //Save();
        }
    }
}