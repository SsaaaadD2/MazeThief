using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMeshGenerator
{
    public float width;
    public float height;

    public MazeMeshGenerator()
    {
        width = GlobalVars.hallWidth;
        height = GlobalVars.hallHeight;
    }

    public Mesh FromData(int[,] data)
    {
        Mesh maze = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> UVs = new List<Vector2>();

        maze.subMeshCount = 2;
        List<int> floorTriangles = new List<int>();
        List<int> wallTriangles = new List<int>();

        int rMax = data.GetUpperBound(0);
        int cMax = data.GetUpperBound(1);
        float halfHeight = height * 0.5f;

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                //If not a wall
                if (data[i, j] != 1)
                {
                    //Floor tiles
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, 0, i * width),
                        Quaternion.LookRotation(Vector3.up),
                        new Vector3(width, width, 1)
                    ), ref vertices, ref UVs, ref floorTriangles);

                    //Roof tiles
                    AddQuad(Matrix4x4.TRS(
                       new Vector3(j * width, height, i * width),
                       Quaternion.LookRotation(Vector3.down),
                       new Vector3(width, width, 1)
                   ), ref vertices, ref UVs, ref floorTriangles);

                    //Walls next to blocked cells
                    //If a wall behind
                    if (data[i - 1, j] == 1 || i - 1 < 0)
                    {
                        AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, halfHeight, (i - 0.5f) * width),
                        Quaternion.LookRotation(Vector3.forward),
                        new Vector3(width, height, 1)
                    ), ref vertices, ref UVs, ref wallTriangles);
                    }

                    //Wall to the right
                    if (data[i, j + 1] == 1 || j + 1 > cMax)
                    {
                        AddQuad(Matrix4x4.TRS(
                        new Vector3((j + .5f) * width, halfHeight, i * width),
                        Quaternion.LookRotation(Vector3.left),
                        new Vector3(width, height, 1)
                    ), ref vertices, ref UVs, ref wallTriangles);
                    }

                    //Wall to the left
                    if (data[i, j - 1] == 1 || j - 1 < 0)
                    {
                        AddQuad(Matrix4x4.TRS(
                        new Vector3((j - .5f) * width, halfHeight, i * width),
                        Quaternion.LookRotation(Vector3.right),
                        new Vector3(width, height, 1)
                    ), ref vertices, ref UVs, ref wallTriangles);
                    }

                    //Wall in front
                    if (data[i + 1, j] == 1 || i + 1 > rMax)
                    {
                        AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, halfHeight, (i + .5f) * width),
                        Quaternion.LookRotation(Vector3.back),
                        new Vector3(width, height, 1)
                    ), ref vertices, ref UVs, ref wallTriangles);
                    }
                }
            }
        }


        maze.vertices = vertices.ToArray();
        maze.uv = UVs.ToArray();
        maze.SetTriangles(floorTriangles, 0);
        maze.SetTriangles(wallTriangles, 1);

        maze.RecalculateNormals();

        return maze;
    }

    void AddQuad(Matrix4x4 matrix, ref List<Vector3> vertices, ref List<Vector2> UVs, ref List<int> triangles)
    {
        int index = vertices.Count;

        Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
        Vector3 vert2 = new Vector3(-.5f, .5f, 0);
        Vector3 vert3 = new Vector3(.5f, .5f, 0);
        Vector3 vert4 = new Vector3(.5f, -.5f, 0);

        //Four vertices in a quadrant
        vertices.Add(matrix.MultiplyPoint3x4(vert1));
        vertices.Add(matrix.MultiplyPoint3x4(vert2));
        vertices.Add(matrix.MultiplyPoint3x4(vert3));
        vertices.Add(matrix.MultiplyPoint3x4(vert4));

        //The uv's aka the placements of each vertex
        //UV (1,0) corresponds to vertext (-.5,-.5, 0) aka vertex 0 (for triangle)
        //UV (1,1) corresponds to vertex (-.5,.5,0) aka vertex 1 (for triangle)
        //etc
        UVs.Add(new Vector2(1, 0));
        UVs.Add(new Vector2(1, 1));
        UVs.Add(new Vector2(0, 1));
        UVs.Add(new Vector2(0, 0));

        //The triangles in the quadrant consist of these vertices (vertex 0,1,2 in this and vertex 3,2,0 in the next)
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index);

        triangles.Add(index + 3);
        triangles.Add(index + 2);
        triangles.Add(index);
    }
}
