using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    //Will tell the generation algorithm the chance of generating an empty space
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = 0.1f;
    }

    //This method will generate a maze given the dimensions
    public int[,] FromDimensions(int rows, int cols)
    {
        int[,] maze = new int[rows, cols];
        int rowMax = maze.GetUpperBound(0);
        int colMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rowMax; i++)
        {
            for (int j = 0; j <= colMax; j++)
            {
                //Maze should be surrounded by walls
                if (i == 0 || i == rowMax || j == 0 || j == colMax)
                {
                    maze[i, j] = 1;
                }
                //Maze should operate on every other cell, not every single cell
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    //Random chance for an empty space
                    if (Random.value > placementThreshold)
                    {
                        //Place a wall in this cell, and one in an adjacent cell in a random direction
                        maze[i, j] = 1;
                        int a = Random.value < 0.5f ? 0 : (Random.value < 0.5f ? 1 : -1);
                        int b = a != 0 ? 0 : (Random.value < 0.5f ? 1 : -1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }
        return maze;
    }
}

