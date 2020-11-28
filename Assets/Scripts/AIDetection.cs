using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetection : MonoBehaviour
{
    public GameObject detectedText;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(detectedText, 
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
    }
}
