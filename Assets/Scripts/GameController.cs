using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]
public class GameController : MonoBehaviour
{
    private MazeConstructor generator;
    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        generator.GenerateNewMaze(13, 15);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
