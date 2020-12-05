using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetection : MonoBehaviour
{
    public bool hasDetected;
    public float fieldOfView;

    private MeshRenderer textMeshRenderer;
    private bool isTransparent;
    private SphereCollider sphereCollider;
    private AIController controller;

    void Start()
    {
        textMeshRenderer = GetComponentInChildren<MeshRenderer>();
        textMeshRenderer.enabled = false;
        sphereCollider = GetComponent<SphereCollider>();
        hasDetected = false;
        isTransparent = true;
        controller = GetComponentInParent<AIController>();
    }
    // Start is called before the first frame update
    void Update()
    {

        if ((hasDetected && isTransparent) || (!hasDetected && !isTransparent))
        {
            ChangeTextAlpha();
        }
        textMeshRenderer.transform.LookAt(Camera.main.transform);

    }

    void ChangeTextAlpha()
    {
        textMeshRenderer.enabled = (textMeshRenderer.enabled == true ? false : true);
        isTransparent = !isTransparent;
    }

    void OnTriggerEnter(Collider other)
    {
        controller.PlayerInRange();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);
            if (angle <= fieldOfView / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction.normalized, out hit, sphereCollider.radius))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        hasDetected = true;
                    }
                }
            }
        }
    }

    //Not using feature of guard losing track of player

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         hasDetected = false;
    //         controller.PlayerOutOfRange();
    //     }
    // }
}
