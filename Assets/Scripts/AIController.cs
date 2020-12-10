using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private GameObject player;
    private bool targetSet;
    private bool playerInRange;
    private Vector3 destination;

    private NavMeshAgent agent;
    private AIDetection detection;
    private ParticleSystem hitParticles;

    private const float SPEED_RUN = 7f;
    private const float SPEED_WALK = 4f;
    private const float SPEED_SLOWDOWN = 2f;

    public bool caughtPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("player= " + player.name);
        detection = GetComponentInChildren<AIDetection>();
        gameObject.tag = "Generated";

        playerInRange = false;
        hitParticles = GetComponent<ParticleSystem>();
        targetSet = false;
        caughtPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetSet && GlobalVars.freeSpots.Count > 0)
        {
            targetSet = true;
            agent = gameObject.AddComponent<NavMeshAgent>();
            int position = Random.Range(0, GlobalVars.freeSpots.Count);
            Vector3 pos = new Vector3(GlobalVars.hallHeight * GlobalVars.freeSpots[position][0],
                            1, GlobalVars.hallWidth * GlobalVars.freeSpots[position][1]);
            agent.Warp(pos);
            agent.stoppingDistance = 3f;

            //For some reason these values become messed up so I force them correct
            agent.baseOffset = 0;
            //GenerateNewDestination();
            agent.speed = SPEED_WALK;
        }
        if (targetSet)
        {
            //if (playerInRange)
            //{

            if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance
                && agent.speed != SPEED_SLOWDOWN)
            {
                caughtPlayer = true;
            }
            agent.SetDestination(player.transform.position);

            //Guard should run once he sees the player, and walk otherwise
            //This only applies if he is not hit with the speed gun
            if (detection.hasDetected == true && agent.speed == SPEED_WALK)
            {
                agent.speed = SPEED_RUN;
            }
            else if (detection.hasDetected == false && agent.speed == SPEED_RUN)
            {
                agent.speed = SPEED_WALK;
            }

            // }
            // //If player is not within range, walk to a random location in search
            // //If close to destination, generate a new location
            // else
            // {
            //     if (Vector3.Distance(transform.position, destination) <= 10)
            //     {
            //         GenerateNewDestination();
            //     }
            // }
        }
    }

    public void SlowDown()
    {
        agent.speed = SPEED_SLOWDOWN;
        hitParticles.Play();

        //If guard was hit by slowdown gun, slow down for 5 seconds
        Invoke("SpeedUp", 5f);
    }

    private void SpeedUp()
    {
        agent.speed = SPEED_WALK;
    }

    //Decided not to use feature of guard going to a random spot

    // public void PlayerInRange()
    // {
    //     //Once the player is found, guard is always chasing
    //     playerInRange = true;
    // }

    //Decided not to use the feature of a guard losing track of the player

    // public void PlayerOutOfRange()
    // {
    //     Invoke("LostSight", 5f);
    // }

    // private void LostSight()
    // {
    //     playerInRange = false;
    //     GenerateNewDestination();
    // }

    // private void GenerateNewDestination()
    // {
    //     Debug.Log("New destination");
    //     int pos = Random.Range(0, GlobalVars.freeSpots.Count);
    //     List<int> coords = GlobalVars.freeSpots[pos];
    //     destination = new Vector3(coords[0] * GlobalVars.hallHeight, 0, coords[1] * GlobalVars.hallWidth);
    //     agent.SetDestination(destination);
    // }


}
