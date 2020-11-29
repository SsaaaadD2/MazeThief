using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private GameObject player;
    private bool targetSet;
    private NavMeshAgent agent;
    private AIDetection detection;

    private const float SPEED_RUN = 7f;
    private const float SPEED_WALK = 4f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        detection = GetComponent<AIDetection>();

        targetSet = false;
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
        }
        if (targetSet)
        {
            agent.SetDestination(player.transform.position);

            //Guard should run once he sees the player, and walk otherwise
            if (detection.hasDetected == true && agent.speed != SPEED_RUN)
            {
                agent.speed = SPEED_RUN;
            }
            else if (detection.hasDetected == false && agent.speed != SPEED_WALK)
            {
                agent.speed = SPEED_WALK;
            }
        }


    }
}
