using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private GameObject player;
    private bool targetSet;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");


        targetSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetSet && GlobalVars.freeSpots.Count > 0)
        {
            targetSet = true;
            gameObject.AddComponent<NavMeshAgent>();
            int position = Random.Range(0, GlobalVars.freeSpots.Count);
            Vector3 pos = new Vector3(GlobalVars.hallHeight * GlobalVars.freeSpots[position][0],
                            1, GlobalVars.hallWidth * GlobalVars.freeSpots[position][1]);
            GetComponent<NavMeshAgent>().Warp(pos);
            GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        }
    }
}
