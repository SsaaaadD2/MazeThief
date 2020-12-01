using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    private const float damping = 0.1f;
    private NavMeshAgent agent;
    private Animator animator;
    private AIDetection detection;

    void Start()
    {
        animator = GetComponent<Animator>();
        detection = GetComponent<AIDetection>();
    }

    void Update()
    {
        if (agent == null)
        {
            //Because the NavMeshAgent on the guard is added in code (in script AIController) after the maze is generated
            //And not in Start
            //We cannot set this variable in Start
            //Thus we check each frame until the NavMeshAgent is assigned
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            //Run if the guard has seen the player, walk otherwise
            float speedPercent = (agent.velocity.magnitude / agent.speed) * (detection.hasDetected == true ? 1 : 0.7f);
            animator.SetFloat("speedPercent", speedPercent, damping, Time.deltaTime);
        }
    }
}
