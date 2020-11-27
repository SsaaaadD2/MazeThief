using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]
public class GameController : MonoBehaviour
{
    [SerializeField] private FpsMovement player;
    [SerializeField] private Text timeLabel;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text instruction;
    [SerializeField] private int maxCols;
    [SerializeField] private int maxRows;

    private MazeConstructor generator;

    private DateTime starttime;
    private int timeLimit;
    private int reduceLimitBy;

    private int score;
    private bool goalReached;

    // Start is called before the first frame update
    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        GlobalVars.maxCols = maxCols;
        GlobalVars.maxRows = maxRows;
        StartGame();
    }

    private void StartGame()
    {
        timeLimit = 80;
        reduceLimitBy = 5;
        score = 0;


        timeLabel.text = timeLimit.ToString();
        scoreLabel.text = score.ToString();
        instruction.text = "Get to the treasure!";
        instruction.enabled = true;
        StartNewMaze();

    }

    private void StartNewMaze()
    {
        generator.GenerateNewMaze(maxRows, maxCols, OnStartTrigger, OnGoalTrigger);
        float x = generator.startCol * generator.hallWidth;
        float y = 1.0f;
        float z = generator.startRow * generator.hallHeight;

        player.transform.position = new Vector3(x, y, z);
        player.enabled = true;
        goalReached = false;

        timeLimit -= reduceLimitBy;
        Invoke("FadeText", 4f);
        starttime = DateTime.Now;
    }

    private void FadeText()
    {
        instruction.CrossFadeAlpha(0, 0.5f, false);
    }


    // Update is called once per frame
    void Update()
    {
        if (!player.enabled)
        {
            return;
        }
        int timeUsed = (int)(DateTime.Now - starttime).TotalSeconds;
        if (timeLimit - timeUsed >= 0)
        {
            timeLabel.text = (timeLimit - timeUsed).ToString();
        }
        else
        {
            timeLabel.text = "TIME UP";
            player.enabled = false;
            Invoke("StartGame", 4f);
        }
    }

    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Won!");
        goalReached = true;

        score += 1;
        scoreLabel.text = score.ToString();
        instruction.text = "Get back home!";
        instruction.enabled = true;
        instruction.CrossFadeAlpha(1, 0.5f, false);
        Invoke("FadeText", 4f);

        Destroy(trigger);
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        if (goalReached)
        {
            Debug.Log("Start again");
            player.enabled = false;
            Invoke("StartNewMaze", 4f);
        }
    }
}
