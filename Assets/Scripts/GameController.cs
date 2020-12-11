using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using static GlobalVars;

[RequireComponent(typeof(MazeConstructor))]
public class GameController : MonoBehaviour
{
    [SerializeField] private FpsMovement player;
    [SerializeField] private Text timeLabel;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text instruction;
    [SerializeField] private int maxCols;
    [SerializeField] private int maxRows;

    public NavMeshSurface navMeshSurface;

    //The generic AIController prefab to instantiate each time
    public AIController guardAgent;
    //public NavMeshAgent playerAgent;

    private MazeConstructor generator;

    private DateTime starttime;
    private int timeLimit;
    private int reduceLimitBy;

    private int score;
    private bool goalReached;

    //The specific instance against which we check if we were caught
    private AIController guardAgentInstance;

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
        timeLimit = 90;
        reduceLimitBy = 10;
        score = 0;


        timeLabel.text = timeLimit.ToString();
        scoreLabel.text = score.ToString();
        StartNewMaze();

    }

    private void StartNewMaze()
    {
        FadeText();
        generator.GenerateNewMaze(maxRows, maxCols, OnStartTrigger, OnGoalTrigger, OnGunTrigger);
        navMeshSurface.BuildNavMesh();
        float x = generator.startCol * generator.hallWidth;
        float y = 1.0f;
        float z = generator.startRow * generator.hallHeight;


        player.transform.position = new Vector3(x, y, z);
        player.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        player.enabled = true;
        player.DisableGun();
        goalReached = false;

        timeLimit -= reduceLimitBy;
        if (timeLimit >= 30)
        {

            Invoke("CreateGuard", 20f);
        }
        instruction.text = "Get to the treasure!";
        instruction.CrossFadeAlpha(1, 0.5f, false);
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

        if (guardAgentInstance && guardAgentInstance.caughtPlayer)
        {
            instruction.text = "The guard caught you!";
            instruction.CrossFadeAlpha(1, 0.5f, false);
            player.enabled = false;
            Invoke("StartGame", 4f);
        }
        else if (timeLimit - timeUsed >= 0)
        {

            timeLabel.text = (timeLimit - timeUsed).ToString();
        }
        else
        {
            instruction.text = "TIME UP";
            instruction.CrossFadeAlpha(1, 0.5f, false);
            player.enabled = false;
            Invoke("StartGame", 4f);
        }
    }

    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        if (other.tag == "Player")
        {
            goalReached = true;
            instruction.text = "Get back home!";
            instruction.CrossFadeAlpha(1, 0.5f, false);
            Invoke("FadeText", 4f);

            Destroy(trigger);
        }

    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        if ((other.tag == "Player" && goalReached) || (int.Parse(timeLabel.text.ToString()) <= 20 && goalReached))
        {
            score += 1;
            scoreLabel.text = score.ToString();
            player.enabled = false;
            instruction.text = "You made it!";
            instruction.CrossFadeAlpha(1, 0.5f, false);
            Invoke("StartNewMaze", 4f);
        }
    }

    private void OnGunTrigger(GameObject trigger, GameObject other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<FpsMovement>().EnableGun();
            instruction.text = "Gun picked up";
            instruction.CrossFadeAlpha(1, 0.5f, false);
            Invoke("FadeText", 4f);
            Destroy(trigger);
        }
    }

    private void CreateGuard()
    {
        guardAgentInstance = Instantiate(guardAgent, Vector3.zero, Quaternion.identity);
        instruction.text = "Guard has been released";
        instruction.CrossFadeAlpha(1, 0.5f, false);
        Invoke("FadeText", 4f);
    }
}
