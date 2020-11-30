using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material treasureMat;
    [SerializeField] private Material startMat;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;

    public int[,] data { get; private set; }

    private MazeDataGenerator dataGenerator;
    private List<List<int>> freeSpots;

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
        freeSpots = new List<List<int>>();
    }

    public void DisposeOldMaze()
    {
        GameObject[] mazes = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject maze in mazes)
        {
            Destroy(maze);
        }
    }

    //Find all the empty spots
    //Place start position at first empty spot
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
                    List<int> free = new List<int>();
                    free.Add(i);
                    free.Add(j);
                    freeSpots.Add(free);
                }
            }
        }
        startRow = freeSpots[0][0];
        startCol = freeSpots[0][1];
    }

    //Place treasure at last empty spot
    public void FindGoalPosition()
    {
        goalRow = freeSpots[freeSpots.Count - 1][0];
        goalCol = freeSpots[freeSpots.Count - 1][1];
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
        GlobalVars.goalPos = new Vector3(gObj.transform.position.x, 1, gObj.transform.position.z);
        gObj.name = "Treasure";
        gObj.tag = "Generated";
        gObj.layer = LayerMask.NameToLayer("AIGuard");
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

        hallHeight = floor.GetComponentInChildren<MeshRenderer>().bounds.size[0];
        hallWidth = floor.GetComponentInChildren<MeshRenderer>().bounds.size[1];

        GlobalVars.hallHeight = hallHeight;
        GlobalVars.hallWidth = hallWidth;

        DisplayMaze();

        PlaceStartTrigger(startCallback);
        PlaceGoalTrigger(goalCallback);
        GlobalVars.freeSpots = freeSpots;
    }

    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        CreateMaze(go);
    }

    void CreateMaze(GameObject parent)
    {
        int rMax = data.GetUpperBound(0);
        int cMax = data.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                //Create floors and ceilings
                if (data[i, j] != 1)
                {
                    Instantiate(floor, new Vector3(j * hallWidth, 0, i * hallWidth),
                        Quaternion.LookRotation(Vector3.up), parent.transform);
                    Instantiate(floor, new Vector3(j * hallWidth, hallHeight, i * hallWidth),
                       Quaternion.LookRotation(Vector3.down), parent.transform);
                }
                //Walls next to blocked cells
                //If a wall behind
                if (i - 1 < 0 || data[i - 1, j] == 1)
                {
                    Instantiate(wall,
                     new Vector3(j * hallWidth, 0, (i - 0.5f) * hallWidth),
                     Quaternion.LookRotation(Vector3.forward), parent.transform);
                }

                //Wall to the right
                if (j + 1 > cMax || data[i, j + 1] == 1)
                {
                    Instantiate(wall,
                     new Vector3((j + .5f) * hallWidth, 0, i * hallWidth),
                     Quaternion.LookRotation(Vector3.left), parent.transform);
                }

                //Wall to the left
                if (j - 1 < 0 || data[i, j - 1] == 1)
                {
                    Instantiate(wall,
                     new Vector3((j - .5f) * hallWidth, 0, i * hallWidth),
                     Quaternion.LookRotation(Vector3.right), parent.transform);
                }

                //Wall in front
                if (i + 1 > rMax || data[i + 1, j] == 1)
                {
                    Instantiate(wall,
                        new Vector3(j * hallWidth, 0, (i + .5f) * hallWidth),
                        Quaternion.LookRotation(Vector3.back), parent.transform);
                }
            }
        }
    }
}
