using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static float MusicVolume;
    public static float EffectsVolume;

    public static int Difficulty;
    public static int[] ExtraLanes = new int[3];        // [0] - Road   [1] - Grass [2] - Water
    public static int[] DefaultLanes = { 4, 1, 3 };     // [0] - Road   [1] - Grass [2] - Water
    public static int ScoreCount;
}
