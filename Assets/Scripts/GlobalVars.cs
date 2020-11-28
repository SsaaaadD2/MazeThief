using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars
{
    public static List<List<int>> freeSpots = new List<List<int>>();
    public static int maxRows;
    public static int maxCols;

    public static float hallWidth = 3.75f;
    public static float hallHeight = 3.5f;

    public static Vector3 goalPos;
}
