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


    public void GenerateNewMaze(int maxRows, int maxCols)
    {
        if (maxCols % 2 == 0 || maxRows % 2 == 0)
        {
            //This is because the maze is surrounded by walls
            Debug.LogWarning("Odd numbers work better for maze size");
        }
        data = dataGenerator.FromDimensions(maxRows, maxCols);
        DisplayMaze();
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
