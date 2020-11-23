using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material treasureMat;
    [SerializeField] private Material startMat;

    public int[,] data { get; private set; }

    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;

    public float hallWidth { get; private set; }
    public float hallHeight { get; private set; }

    public int startRow { get; private set; }
    public int startCol { get; private set; }

    public int goalRow { get; private set; }
    public int goalCol { get; private set; }


    void Awake()
    {
        //Walls surronding an empty cell in the centre
        data = new int[,] {
            {1,1,1},
            {1,0,1},
            {1,1,1}
        };

        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();
    }

    public void DisposeOldMaze()
    {
        GameObject[] mazes = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject maze in mazes)
        {
            Destroy(maze);
        }
    }

    //Find the first empty space available and start there
    public void FindStartPosition()
    {
        int[,] maze = data;
        int maxRows = data.GetUpperBound(0);
        int maxCols = data.GetUpperBound(1);
        for (int i = 0; i <= maxRows; i++)
        {
            for (int j = 0; j <= maxCols; j++)
            {
                if (maze[i, j] == 0)
                {
                    startRow = i;
                    startCol = j;
                    return;
                }
            }
        }
    }

    //Find the last empty space available and place the treasure there
    public void FindGoalPosition()
    {
        int[,] maze = data;
        int maxRows = data.GetUpperBound(0);
        int maxCols = data.GetUpperBound(1);
        for (int i = maxRows; i >= 0; i--)
        {
            for (int j = maxCols; j >= 0; j--)
            {
                if (maze[i, j] == 0)
                {
                    goalRow = i;
                    goalCol = j;
                    return;
                }
            }
        }
    }

    //Create object for start trigger and fill its properties
    public void PlaceStartTrigger(TriggerEventHandler callback)
    {
        GameObject gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gObj.transform.position = new Vector3(startCol * hallWidth, .5f, startRow * hallHeight);
        gObj.name = "Start Trigger";
        gObj.tag = "Generated";

        gObj.GetComponent<MeshRenderer>().sharedMaterial = startMat;
        gObj.GetComponent<BoxCollider>().isTrigger = true;

        TriggerEventRouter tc = gObj.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    //Create object for treasure and fill its properties
    public void PlaceGoalTrigger(TriggerEventHandler callback)
    {
        GameObject gObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gObj.transform.position = new Vector3(goalCol * hallWidth, .5f, startRow * goalCol);
        gObj.name = "Treasure";
        gObj.tag = "Generated";

        gObj.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;
        gObj.GetComponent<BoxCollider>().isTrigger = true;

        TriggerEventRouter tc = gObj.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }


    public void GenerateNewMaze(int maxRows, int maxCols,
                TriggerEventHandler startCallback = null, TriggerEventHandler goalCallback = null)
    {
        if (maxCols % 2 == 0 || maxRows % 2 == 0)
        {
            //This is because the maze is surrounded by walls
            Debug.LogWarning("Odd numbers work better for maze size");
        }
        DisposeOldMaze();

        data = dataGenerator.FromDimensions(maxRows, maxCols);
        FindStartPosition();
        FindGoalPosition();

        hallHeight = meshGenerator.height;
        hallWidth = meshGenerator.width;

        DisplayMaze();

        PlaceStartTrigger(startCallback);
        PlaceGoalTrigger(goalCallback);
    }

    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mazeMat1, mazeMat2 };
    }

    //This method simply draws out a design of the maze on paper
    // "==" represents a wall and "..." represents an open space
    void OnGUI()
    {
        if (showDebug == false)
        {
            return;
        }
        int[,] maze = data;

        //Similar to doing maze[0].Length
        int rowMax = maze.GetUpperBound(0);
        int colMax = maze.GetUpperBound(1);

        string msg = "";

        for (int i = 0; i <= rowMax; i++)
        {
            for (int j = 0; j <= colMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    msg += "....";
                }
                else
                {
                    msg += "==";
                }
            }
            msg += "\n";
        }

        //Draw a rectangle on the GUI with the maze
        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }
}
